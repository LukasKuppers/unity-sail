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
    [SerializeField]
    private GameObject smoothCamToggleObject;

    private Button applyBtn;
    private Slider uiScaleSlider;
    private Slider masterVolumeSlider;
    private TMP_Dropdown displayModeDropdown;
    private TMP_Dropdown resolutionDropdown;
    private Toggle smoothCamToggle;

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
        smoothCamToggle = smoothCamToggleObject.GetComponent<Toggle>();

        applyBtn.onClick.AddListener(ApplyValues);

        InitValues();
    }

    private void InitValues()
    {
        InitUIScale();
        InitMasterVolume();
        InitCamSmoothing();
        InitDisplaySettings();
    }

    private void ApplyValues()
    {
        ApplyUIScale();
        ApplyMasterVolume();
        ApplyCamSmoothing();
        ApplyDisplaySettings();

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

    private void InitCamSmoothing()
    {
        bool smooth = bool.Parse(optionsManager.GetOption(Option.SMOOTH_CAMERA));
        smoothCamToggle.isOn = smooth;
    }

    private void ApplyCamSmoothing()
    {
        bool smooth = smoothCamToggle.isOn;
        optionsManager.WriteOption(Option.SMOOTH_CAMERA, smooth.ToString());
    }

    private void InitDisplaySettings()
    {
        List<TMP_Dropdown.OptionData> resList = new List<TMP_Dropdown.OptionData>();
        foreach (Resolution res in Screen.resolutions)
        {
            string resStr = ResToString(res);
            TMP_Dropdown.OptionData newRes = new TMP_Dropdown.OptionData(resStr);
            resList.Add(newRes);
        }
        resolutionDropdown.AddOptions(resList);

        resolutionDropdown.value = resolutionDropdown.options.FindIndex(
            option => option.text == ResToString(Screen.currentResolution));

        string currentMode = Screen.fullScreenMode == FullScreenMode.FullScreenWindow ? "Fullscreen" : "Windowed";
        displayModeDropdown.value = displayModeDropdown.options.FindIndex(
            option => option.text == currentMode);
    }

    private void ApplyDisplaySettings()
    {
        string resolution = resolutionDropdown.options[resolutionDropdown.value].text;
        
        string dispMode = displayModeDropdown.options[displayModeDropdown.value].text;
        bool isFullscreen = dispMode == "Fullscreen";

        optionsManager.WriteOption(Option.DISPLAY_MODE, isFullscreen.ToString());
        optionsManager.WriteOption(Option.DISPLAY_RESOLUTION, resolution);
    }

    private string ResToString(Resolution res)
    {
        return $"{res.width}x{res.height}";
    }
}
