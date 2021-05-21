using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpenWindowButton : MonoBehaviour
{
    [SerializeField]
    private GameObject window;

    private Button button;

    private void Start()
    {
        button = gameObject.GetComponent<Button>();

        button.onClick.AddListener(OpenModal);
    }

    private void OpenModal()
    {
        GameObject canvas = GameObject.FindWithTag("Canvas");
        Instantiate(window, canvas.transform);
    }
}
