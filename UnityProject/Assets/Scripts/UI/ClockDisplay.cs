using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ClockDisplay : MonoBehaviour
{
    private static readonly float UPDATE_WAIT_TIME = 1.0f;

    [SerializeField]
    private GameObject dayNightCycleManager;
    [SerializeField]
    private GameObject hourHand;
    [SerializeField]
    private GameObject minuteHand;
    [SerializeField]
    private GameObject dayText;

    private TextMeshProUGUI dayT;
    private DayNightCycle timeManager;

    private void Start()
    {
        dayT = dayText.GetComponent<TextMeshProUGUI>();
        timeManager = dayNightCycleManager.GetComponent<DayNightCycle>();

        StartCoroutine(UpdateClock());
    }

    private IEnumerator UpdateClock()
    {
        while (true)
        {
            dayT.SetText(timeManager.GetDay().ToString());

            float dayPercent = timeManager.GetTimePercent();
            SetMinuteHandRotation(dayPercent);
            SetHourHandRotation(dayPercent);

            yield return new WaitForSeconds(UPDATE_WAIT_TIME);
        }
    }

    // time percent = 0 => sunrise, we will map this to 6:00 AM

    private void SetMinuteHandRotation(float dayPercent)
    {
        float rotation = (dayPercent * -8640f) + 90f;
        SetHandRotation(minuteHand, rotation);
    }

    private void SetHourHandRotation(float dayPercent)
    {
        float rotation = (dayPercent * -720f) - 90f;
        SetHandRotation(hourHand, rotation);
    }

    private void SetHandRotation(GameObject hand, float rotation)
    {
        Quaternion rot = Quaternion.Euler(0, 0, rotation);
        hand.transform.rotation = rot;
    }
}
