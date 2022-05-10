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
            modalData.InitParameters(gameLoader);
        }
    }
}
