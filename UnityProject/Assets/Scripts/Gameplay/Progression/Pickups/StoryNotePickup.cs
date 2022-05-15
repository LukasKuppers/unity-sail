using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryNotePickup : MonoBehaviour, IClickableObject
{
    [SerializeField]
    private GameObject storyNoteManager;
    [SerializeField]
    private int noteIndex = 0;

    private StoryNotesManager notesManager;

    private void Start()
    {
        InitNoteManager();
    }

    public void InitParameters(GameObject storyNoteManager, int noteIndex)
    {
        this.noteIndex = noteIndex;
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
            List<int> discoveredNotes = notesManager.GetDiscoveredNoteIndicies();
            if (discoveredNotes != null && discoveredNotes.Contains(noteIndex))
            {
                Destroy(gameObject);
            }
        }
    }
}
