using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsPage : MonoBehaviour
{
    [SerializeField]
    private GameObject applyButton;

    private Button applyBtn;

    private GameMenuModal menu;

    private void Awake()
    {
        menu = transform.parent.gameObject.GetComponent<GameMenuModal>();

        applyBtn = applyButton.GetComponent<Button>();
        applyBtn.onClick.AddListener(menu.ClosePages);
    }
}
