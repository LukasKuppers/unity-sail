using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryNoteSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject storyNoteManager;
    [SerializeField]
    private GameObject storyNotePrefab;
    [SerializeField]
    private int noteIndex = 0;

    private void Start()
    {
        GameObject note = Instantiate(storyNotePrefab, transform.position, transform.rotation, transform);
        StoryNotePickup noteData = note.GetComponent<StoryNotePickup>();

        noteData.InitParameters(storyNoteManager, noteIndex);
    }
}
