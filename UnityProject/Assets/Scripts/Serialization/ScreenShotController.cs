using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenShotController : MonoBehaviour
{
    private static readonly string FILE_PATH = "screenshots";

    private PlayerInputManager inputManager;

    private void Start()
    {
        inputManager = InputReference.GetInputManager();

        inputManager.AddInputListener(InputEvent.TAKE_SCREENSHOT, TakeScreenshot);
    }

    private void TakeScreenshot()
    {
        string name = GenerateFileName();
        ScreenCapture.CaptureScreenshot(name);
        UnityEditor.AssetDatabase.Refresh();
    }

    private string GenerateFileName()
    {
        var currentDateTime = System.DateTime.Now;
        string datetime = currentDateTime.ToString("yyyy-MM-dd HH-mm-ss");
        return $"{Application.dataPath}/{FILE_PATH}/capture_{datetime}.png";
    }
}
