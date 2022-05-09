using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExitGameButton : MonoBehaviour
{
    private Button ExitBtn;

    private void Start()
    {
        ExitBtn = gameObject.GetComponent<Button>();

        ExitBtn.onClick.AddListener(ExitGame);
    }

    private void ExitGame()
    {
        Application.Quit();
    }
}
