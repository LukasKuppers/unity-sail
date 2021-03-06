using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPage : MonoBehaviour
{
    [SerializeField]
    private GameObject applyButton;
    [SerializeField]
    private GameObject uiScaleSliderObject;

    private Button applyBtn;
    private Slider uiScaleSlider;

    private GameMenuModal menu;
    private OptionsManager optionsManager;

    private void Awake()
    {
        menu = transform.parent.gameObject.GetComponent<GameMenuModal>();
        optionsManager = FindObjectOfType<OptionsManager>();

        applyBtn = applyButton.GetComponent<Button>();
        uiScaleSlider = uiScaleSliderObject.GetComponent<Slider>();

        applyBtn.onClick.AddListener(ApplyValues);

        InitValues();
    }

    private void InitValues()
    {
        float uiScale = float.Parse(optionsManager.GetOption(Option.UI_SCALE));

        uiScaleSlider.value = Mathf.InverseLerp(0.5f, 1.0f, uiScale);
    }

    private void ApplyValues()
    {
        float uiScale = Mathf.Lerp(0.5f, 1.0f, uiScaleSlider.value);

        optionsManager.WriteOption(Option.UI_SCALE, uiScale.ToString());

        menu.ClosePages();
    }
}
