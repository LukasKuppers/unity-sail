using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryNotePickup : MonoBehaviour, IClickableObject
{
    [SerializeField]
    private GameObject storyNoteManager;
    [SerializeField]
    private int noteIndex;

    private StoryNotesManager notesManager;

    private void Start()
    {
        InitNoteManager();
    }

    public void SetStoryNoteManager(GameObject storyNoteManager)
    {
        this.storyNoteManager = storyNoteManager;
        InitNoteManager();
    }

    public void Interact(string interactionLockKey)
    {
        notesManager.DiscoverNote(noteIndex);
        PlayerAttackMode.EnableAttack(interactionLockKey);
        Destroy(gameObject);
    }

    private void InitNoteManager()
    {
        if (storyNoteManager != null)
        {
            notesManager = storyNoteManager.GetComponent<StoryNotesManager>();
            if (notesManager.GetDiscoveredNoteIndicies().Contains(noteIndex))
            {
                Destroy(gameObject);
            }
        }
    }
}
