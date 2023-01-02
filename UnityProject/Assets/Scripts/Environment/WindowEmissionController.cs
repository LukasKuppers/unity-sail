using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WindowEmissionController : MonoBehaviour
{
    private static readonly string EMISSION_KEYWORD = "_EMISSION";
    private static readonly string EMISSION_KEY = "_EmissiveColor";
    private static readonly float TRANSITION_TIME = 5.0f;
    private static readonly float OFF_MULTIPLIER = 0f;

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

        StartCoroutine(SetInitColorOnDelay());
    }

    private void TurnOnLights()
    {
        StartCoroutine(TransitionColor(offColor, onColor, OFF_MULTIPLIER, onIntensity));
    }

    private void TurnOffLights()
    {
        StartCoroutine(TransitionColor(onColor, offColor, onIntensity, OFF_MULTIPLIER));
    }

    private IEnumerator TransitionColor(Color oldCol, Color newCol, float oldEm, float newEm)
    {
        int numFrames = (int)(TRANSITION_TIME * 10f);
        float timePerFrame = TRANSITION_TIME / numFrames;

        for (int i = 0; i < numFrames; i++)
        {
            float transitionPercent = (float)i / numFrames;
            Color col = Color.Lerp(oldCol, newCol, transitionPercent);
            float intensity = Mathf.Lerp(oldEm, newEm, transitionPercent);

            SetColor(col, intensity);

            yield return new WaitForSeconds(timePerFrame);
        }

        SetColor(newCol, newEm);
    }

    private IEnumerator SetInitColorOnDelay()
    {
        yield return null;

        if (timeManager.GetTimePercent() < 0.5f)
            SetColor(offColor, OFF_MULTIPLIER);
        else
            SetColor(onColor, onIntensity);
    }

    private void SetColor(Color col, float multiplier)
    {
        windowMaterial.color = col;
        windowMaterial.SetColor(EMISSION_KEY, col * multiplier);
    }
}
