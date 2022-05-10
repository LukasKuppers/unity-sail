using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsManager : MonoBehaviour
{
    private static readonly Dictionary<Option, string> KEY_MAP = new Dictionary<Option, string>()
    {
        { Option.UI_SCALE, "ui_scale" }
    };

    private static readonly Dictionary<string, string> DEFAULT_OPTIONS = new Dictionary<string, string>()
    {
        { KEY_MAP[Option.UI_SCALE], "1" }
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

        ApplyOptionsToScene();
    }

    private void ApplyOptionsToScene()
    {
        float uiScale = float.Parse(options[KEY_MAP[Option.UI_SCALE]]);

        hudScaler.scaleFactor = uiScale;
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
}

public enum Option
{
    UI_SCALE
}
