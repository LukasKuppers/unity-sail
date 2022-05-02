using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Start()
    {
        moon = moonObject.GetComponent<Light>();
        StartCoroutine(DayTimeCycle());
    }

    public int GetTime()
    {
        return time;
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
        sunObject.transform.rotation = Quaternion.Euler(rot, 0, 0);
    }

    private void SetMoonRotation(float dayPercent)
    {
        float monthRot = (day / 30) * 360f;
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

            float dayPercentCompletion = time / (float)dayDurationSec;
            SetMoonRotation(dayPercentCompletion);

            yield return new WaitForSeconds(1.0f);
        }
    }
}
