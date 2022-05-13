using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class StoryNotesLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "Story_notes_load_module";

    [SerializeField]
    private GameObject storyNotesManager;

    private StoryNotesManager notesManager;

    private void Start()
    {
        notesManager = storyNotesManager.GetComponent<StoryNotesManager>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public string GetJsonString()
    {
        List<int> discoveredNotesList = notesManager.GetDiscoveredNotes();
        int[] discoveredNotes = discoveredNotesList.ToArray();

        PersistentNotesData notesData = new PersistentNotesData()
        {
            discoveredNotes = discoveredNotes
        };

        string jsonString = JsonUtility.ToJson(notesData);
        return jsonString;
    }

    public void Load(string saveJson)
    {
        JSONNode json = JSON.Parse(saveJson);
        if (json == null)
        {
            InitNewGame();
            return;
        }
        string objectData = json[JSON_KEY].Value;
        PersistentNotesData data = JsonUtility.FromJson<PersistentNotesData>(objectData);

        if (data == null || data.discoveredNotes == null)
        {
            InitNewGame();
            return;
        }

        List<int> noteList = new List<int>();
        foreach (int noteID in data.discoveredNotes)
        {
            noteList.Add(noteID);
        }
        notesManager.SetDiscoveredNotes(noteList);
    }

    private void InitNewGame()
    {
        notesManager.SetDiscoveredNotes(null);
    }
}

[Serializable]
public class PersistentNotesData
{
    public int[] discoveredNotes;
}
