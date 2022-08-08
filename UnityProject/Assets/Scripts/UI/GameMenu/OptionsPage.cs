using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class OptionsPage : MonoBehaviour
{
    [SerializeField]
    private GameObject applyButton;
    [SerializeField]
    private GameObject uiScaleSliderObject;
    [SerializeField]
    private GameObject masterVolumeSliderObject;
    [SerializeField]
    private GameObject displayModeDropdownObject;
    [SerializeField]
    private GameObject resolutionDropdownObject;

    private Button applyBtn;
    private Slider uiScaleSlider;
    private Slider masterVolumeSlider;
    private TMP_Dropdown displayModeDropdown;
    private TMP_Dropdown resolutionDropdown;

    private GameMenuModal menu;
    private OptionsManager optionsManager;

    private void Awake()
    {
        menu = transform.parent.gameObject.GetComponent<GameMenuModal>();
        optionsManager = FindObjectOfType<OptionsManager>();

        applyBtn = applyButton.GetComponent<Button>();
        uiScaleSlider = uiScaleSliderObject.GetComponent<Slider>();
        masterVolumeSlider = masterVolumeSliderObject.GetComponent<Slider>();
        displayModeDropdown = displayModeDropdownObject.GetComponent<TMP_Dropdown>();
        resolutionDropdown = resolutionDropdownObject.GetComponent<TMP_Dropdown>();

        applyBtn.onClick.AddListener(ApplyValues);

        InitValues();
    }

    private void InitValues()
    {
        InitUIScale();
        InitMasterVolume();
        InitDisplaySettings();
    }

    private void ApplyValues()
    {
        ApplyUIScale();
        ApplyMasterVolume();

        menu.ClosePages();
    }

    private void InitUIScale()
    {
        float uiScale = float.Parse(optionsManager.GetOption(Option.UI_SCALE));
        uiScaleSlider.value = Mathf.InverseLerp(0.5f, 1.0f, uiScale);
    }

    private void ApplyUIScale()
    {
        float uiScale = Mathf.Lerp(0.5f, 1.0f, uiScaleSlider.value);
        optionsManager.WriteOption(Option.UI_SCALE, uiScale.ToString());
    }

    private void InitMasterVolume()
    {
        float volume = float.Parse(optionsManager.GetOption(Option.MASTER_VOLUME));
        masterVolumeSlider.value = volume;
    }

    private void ApplyMasterVolume()
    {
        float volume = masterVolumeSlider.value;
        optionsManager.WriteOption(Option.MASTER_VOLUME, volume.ToString());
    }

    private void InitDisplaySettings()
    {
        List<TMP_Dropdown.OptionData> resList = new List<TMP_Dropdown.OptionData>();
        foreach (Resolution res in Screen.resolutions)
        {
            string resStr = $"{res.width}x{res.height}";
            TMP_Dropdown.OptionData newRes = new TMP_Dropdown.OptionData(resStr);
            resList.Add(newRes);
        }
        resolutionDropdown.AddOptions(resList);
    }
}
