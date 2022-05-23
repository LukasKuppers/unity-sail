using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DayNightCycle : MonoBehaviour
{
    [SerializeField]
    private GameObject sunObject;
    [SerializeField]
    private GameObject moonObject;
    [SerializeField]
    private int dayDurationSec = 120;
    [SerializeField]
    private float minMoonIntensity = 0f;
    [SerializeField]
    private float maxMoonIntensity = 100000f;
    [SerializeField]
    private float moonIntensityDropoff = 1f;
    [SerializeField]
    private float moonElevationAngle = 45f;

    // time ranges from 0 to dayDuration - 1
    private int time = 0;
    // day ranges from 0 to 29
    private int day = 0;
    private Light moon;

    private int framesSinceLastCycle = 0;

    private Dictionary<(int, int), UnityEvent> dayTimeEvents;
    private Dictionary<int, UnityEvent> repeatTimeEvents;

    private void Start()
    {
        moon = moonObject.GetComponent<Light>();

        if (dayTimeEvents == null)
            dayTimeEvents = new Dictionary<(int, int), UnityEvent>();
        if (repeatTimeEvents == null)
            repeatTimeEvents = new Dictionary<int, UnityEvent>();

        StartCoroutine(DayTimeCycle());
    }

    public void AddDayTimeListener(int day, float time, UnityAction call)
    {
        (int, int) dayTime = (day, (int)(time * dayDurationSec));

        if (dayTimeEvents == null)
            dayTimeEvents = new Dictionary<(int, int), UnityEvent>();

        if (!dayTimeEvents.ContainsKey(dayTime))
            dayTimeEvents.Add(dayTime, new UnityEvent());

        dayTimeEvents[dayTime].AddListener(call);
    }

    public void AddRepeatTimeListener(float time, UnityAction call)
    {
        int timeSec = (int)(time * dayDurationSec);

        if (repeatTimeEvents == null)
            repeatTimeEvents = new Dictionary<int, UnityEvent>();

        if (!repeatTimeEvents.ContainsKey(timeSec))
            repeatTimeEvents.Add(timeSec, new UnityEvent());

        repeatTimeEvents[timeSec].AddListener(call);
    }

    public int GetTime()
    {
        return time;
    }

    public float GetTimePercent()
    {
        return (float)time / dayDurationSec;
    }

    public int GetDay()
    {
        return day;
    }

    public void SetTime(int newTime)
    {
        time = newTime;
    }

    public void SetDay(int newDay)
    {
        day = newDay;
    }

    private void SetSunRotation(float dayPercent)
    {
        float rot = dayPercent * 360f;
        sunObject.transform.rotation = Quaternion.Euler(rot, -90, 0);
    }

    private void SetMoonRotation(float dayPercent)
    {
        float monthRot = day * 12f;
        float dayRot = dayPercent * 12f;
        float rot = monthRot + dayRot;

        moonObject.transform.rotation = Quaternion.Euler(0, rot, 0);
        moonObject.transform.RotateAround(
            moonObject.transform.position,
            moonObject.transform.right, 
            -180f - moonElevationAngle);
    }

    private void SetMoonEmission(float dayPercent)
    {
        float sinValue = (maxMoonIntensity - minMoonIntensity) * Mathf.Sin(dayPercent * 2 * Mathf.PI);
        float intensity = maxMoonIntensity - (moonIntensityDropoff * sinValue);
        intensity = Mathf.Clamp(intensity, minMoonIntensity, maxMoonIntensity);

        moon.intensity = intensity;
    }

    // smooth values
    private void Update()
    {
        float framesPerCycle = 1f / Time.smoothDeltaTime;
        float degPerFrame = 1f / (dayDurationSec * framesPerCycle);
        float dayPercent = (time / (float)dayDurationSec) + (degPerFrame * framesSinceLastCycle);

        SetSunRotation(dayPercent);
        SetMoonEmission(dayPercent);
        framesSinceLastCycle++;
    }

    private IEnumerator DayTimeCycle()
    {
        while (true)
        {
            framesSinceLastCycle = 0;
            time = (time + 1) % dayDurationSec;
            if (time == 0)
            {
                day = (day + 1) % 30;
            }

            if (dayTimeEvents.ContainsKey((day, time)))
                dayTimeEvents[(day, time)].Invoke();
            if (repeatTimeEvents.ContainsKey(time))
                repeatTimeEvents[time].Invoke();

            float dayPercentCompletion = time / (float)dayDurationSec;
            SetMoonRotation(dayPercentCompletion);

            yield return new WaitForSeconds(1.0f);
        }
    }
}
