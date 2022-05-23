using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowEmissionController : MonoBehaviour
{
    private static readonly string EMISSION_KEYWORD = "_EMISSION";
    private static readonly string EMISSION_KEY = "_EmissiveColor";
    private static readonly float TRANSITION_TIME = 5.0f;

    [SerializeField]
    private GameObject dayNightCycleManager;
    [SerializeField]
    private Material windowMaterial;
    [SerializeField]
    private Color onColor;
    [SerializeField]
    private Color offColor;
    [SerializeField]
    private float onIntensity;

    private DayNightCycle timeManager;

    private void Start()
    {
        timeManager = dayNightCycleManager.GetComponent<DayNightCycle>();
        windowMaterial.EnableKeyword(EMISSION_KEYWORD);

        timeManager.AddRepeatTimeListener(0f, TurnOffLights);
        timeManager.AddRepeatTimeListener(0.5f, TurnOnLights);

        if (timeManager.GetTimePercent() < 0.5f)
            SetColor(offColor);
        else
            SetColor(onColor * onIntensity);
    }

    private void TurnOnLights()
    {
        StartCoroutine(TransitionColor(offColor, onColor * onIntensity));
    }

    private void TurnOffLights()
    {
        StartCoroutine(TransitionColor(onColor * onIntensity, offColor));
    }

    private IEnumerator TransitionColor(Color oldCol, Color newCol)
    {
        int numFrames = (int)(TRANSITION_TIME * 10f);
        float timePerFrame = TRANSITION_TIME / numFrames;

        for (int i = 0; i < numFrames; i++)
        {
            float transitionPercent = (float)i / numFrames;
            Color col = Color.Lerp(oldCol, newCol, transitionPercent);

            SetColor(col);

            yield return new WaitForSeconds(timePerFrame);
        }

        SetColor(newCol);
    }

    private void SetColor(Color col)
    {
        windowMaterial.color = col;
        windowMaterial.SetColor(EMISSION_KEY, col);
    }
}
