using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StoryNotesManager : MonoBehaviour
{
    private List<int> discoveredNotes;

    private void Start()
    {
        discoveredNotes = new List<int>();
    }

    public List<int> GetDiscoveredNotes()
    {
        return discoveredNotes;
    }

    public void SetDiscoveredNotes(List<int> discoveredNotes)
    {
        if (discoveredNotes != null)
            this.discoveredNotes = discoveredNotes;
    }

    public void DiscoverNote(int noteID)
    {
        if (!discoveredNotes.Contains(noteID))
        {
            discoveredNotes.Add(noteID);
        }
    }
}
