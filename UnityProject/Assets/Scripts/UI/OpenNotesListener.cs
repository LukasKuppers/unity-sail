using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OpenNotesListener : MonoBehaviour
{
    [SerializeField]
    private GameObject uIParent;
    [SerializeField]
    private GameObject notesManager;
    [SerializeField]
    private GameObject notesModalPrefab;
    
    private void Start()
    {
        PlayerInputManager inputManager = InputReference.GetInputManager();
        inputManager.AddInputListener(InputEvent.OPEN_NOTES, OpenModal);
    }

    private void OpenModal()
    {
        if (PlayerSceneInteraction.InteractionEnabled())
        {
            GameObject modal = Instantiate(notesModalPrefab, uIParent.transform);
            NotesModal modalData = modal.GetComponent<NotesModal>();
            modalData.InitParameters(notesManager);

            AudioManager.GetInstance().Play(SoundMap.TURN_PAGE);
        }
    }
}
