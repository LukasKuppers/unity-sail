using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenMenuListener : MonoBehaviour
{
    [SerializeField]
    private GameObject uIParent;
    [SerializeField]
    private GameObject gameLoader;
    [SerializeField]
    private GameObject menuPrefab;
    [SerializeField]
    private string customQuitBtnText = "";

    private void Start()
    {
        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.OPEN_MENU, OpenMenu);
    }

    private void OpenMenu()
    {
        if (PlayerSceneInteraction.InteractionEnabled())
        {
            GameObject gameMenu = Instantiate(menuPrefab, uIParent.transform);

            GameMenuModal modalData = gameMenu.GetComponent<GameMenuModal>();

            if (customQuitBtnText != null && customQuitBtnText != "")
                modalData.InitParameters(gameLoader, customQuitBtnText);
            else
                modalData.InitParameters(gameLoader);

            AudioManager.GetInstance().Play(SoundMap.TURN_PAGE);
        }
    }
}
