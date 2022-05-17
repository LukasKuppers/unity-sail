using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryNotesManager : MonoBehaviour
{
    private List<int> discoveredNotes;
    private Dictionary<int, StoryNote> allNotes;

    private void Start()
    {
        discoveredNotes = new List<int>() { 0 };

        allNotes = new Dictionary<int, StoryNote>();
        DelimmitedTextLoader noteLoader = new DelimmitedTextLoader("StoryNotes", '\t');
        string[][] rawData = noteLoader.GetDataNoHeader();
        foreach (string[] row in rawData)
        {
            StoryNote note = new StoryNote()
            {
                index = int.Parse(row[0]),
                name = row[1],
                content = row[2]
            };
            allNotes.Add(note.index, note);
        }
    }

    public List<int> GetDiscoveredNoteIndicies()
    {
        return discoveredNotes;
    }

    public StoryNote GetNote(int noteIndex)
    {
        if (discoveredNotes.Contains(noteIndex) && allNotes.ContainsKey(noteIndex))
        {
            return allNotes[noteIndex];
        }
        Debug.LogWarning("StoryNotesManager:GetNote: note is not discovered or doesn't exist");
        return null;
    }

    public void SetDiscoveredNotes(List<int> discoveredNotes)
    {
        if (discoveredNotes != null)
            this.discoveredNotes = discoveredNotes;

        if (!this.discoveredNotes.Contains(0))
            this.discoveredNotes.Add(0);
    }

    public void DiscoverNote(int noteID)
    {
        if (!discoveredNotes.Contains(noteID))
        {
            discoveredNotes.Add(noteID);
        }
    }
}

[System.Serializable]
public class StoryNote
{
    public int index;

    public string name;

    public string content;
}
