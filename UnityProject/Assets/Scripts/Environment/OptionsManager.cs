using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    private static readonly Dictionary<Option, string> KEY_MAP = new Dictionary<Option, string>()
    {
        { Option.UI_SCALE, "ui_scale" },
        { Option.MASTER_VOLUME, "master_volume" },
        { Option.MUSIC_VOLUME, "music_volume" }, 
        { Option.DISPLAY_MODE, "display_mode" },
        { Option.DISPLAY_RESOLUTION, "display_resolution" },
        { Option.SMOOTH_CAMERA, "smooth_camera" }
    };

    private static readonly Dictionary<string, string> DEFAULT_OPTIONS = new Dictionary<string, string>()
    {
        { KEY_MAP[Option.UI_SCALE], "1" },
        { KEY_MAP[Option.MASTER_VOLUME], "1" },
        { KEY_MAP[Option.MUSIC_VOLUME], "1" }, 
        { KEY_MAP[Option.DISPLAY_MODE], "true" },
        { KEY_MAP[Option.DISPLAY_RESOLUTION], "1920x1080" },
        { KEY_MAP[Option.SMOOTH_CAMERA], "true" }
    };

    [SerializeField]
    private GameObject hudCanvas;

    private CanvasScaler hudScaler;

    private Dictionary<string, string> options;

    private void Start()
    {
        hudScaler = hudCanvas.GetComponent<CanvasScaler>();

        options = SavedOptionsManager.LoadOptions();
        if (options == null || options.Count == 0)
        {
            options = DEFAULT_OPTIONS;
        }

        foreach (string key in DEFAULT_OPTIONS.Keys)
        {
            if (!options.ContainsKey(key))
            {
                options.Add(key, DEFAULT_OPTIONS[key]);
            }
        }

        ApplyOptionsToScene();
    }

    private void ApplyOptionsToScene()
    {
        ApplyUIScaleToScene();
        ApplyMasterVolumeToScene();
        ApplyMusicVolumeToScene();
        ApplyCameraSmoothingToScene();
        ApplyDisplayModeToScene();
    }

    public void WriteOption(Option optionType, string optionValue)
    {
        options[KEY_MAP[optionType]] = optionValue;

        ApplyOptionsToScene();
        SavedOptionsManager.SaveOptions(options);
    }

    public string GetOption(Option optionType)
    {
        return options[KEY_MAP[optionType]];
    }

    private void ApplyUIScaleToScene()
    {
        float uiScale = float.Parse(options[KEY_MAP[Option.UI_SCALE]]);
        hudScaler.scaleFactor = uiScale;
    }

    private void ApplyMasterVolumeToScene()
    {
        float volume = float.Parse(options[KEY_MAP[Option.MASTER_VOLUME]]);
        AudioListener.volume = volume;
    }

    private void ApplyMusicVolumeToScene()
    {
        float volume = float.Parse(options[KEY_MAP[Option.MUSIC_VOLUME]]);
        AudioManager.GetInstance().SetMusicVolume(volume);
    }

    private void ApplyCameraSmoothingToScene()
    {
        bool smooth = bool.Parse(options[KEY_MAP[Option.SMOOTH_CAMERA]]);

        CameraController camController = Camera.main.gameObject.GetComponent<CameraController>();
        camController.SetCameraSmooth(smooth);
    }

    private void ApplyDisplayModeToScene()
    {
        bool isFullscreen = bool.Parse(options[KEY_MAP[Option.DISPLAY_MODE]]);
        string resStr = options[KEY_MAP[Option.DISPLAY_RESOLUTION]];

        string[] resArr = resStr.Split(new char[] { 'x' });
        int width = int.Parse(resArr[0]);
        int height = int.Parse(resArr[1]);

        FullScreenMode mode = isFullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed;
        
        Screen.SetResolution(width, height, mode);
    }
}

public enum Option
{
    UI_SCALE,
    MASTER_VOLUME, 
    MUSIC_VOLUME, 
    DISPLAY_MODE, 
    DISPLAY_RESOLUTION, 
    SMOOTH_CAMERA
}
