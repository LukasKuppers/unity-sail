using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NotesModal : MonoBehaviour
{
    private static readonly string INTERACTION_LOCK_KEY = "Notes_modal_key";

    [SerializeField]
    private GameObject letterSelectorPrefab;

    [SerializeField]
    private GameObject exitButton;
    [SerializeField]
    private GameObject selectorsContainer;
    [SerializeField]
    private GameObject currentNoteTitleText;
    [SerializeField]
    private GameObject currentNoteContentText;

    private Button exitBtn;
    private TextMeshProUGUI currentNoteTitleT;
    private TextMeshProUGUI currentNoteContentT;

    private StoryNotesManager notesManager;

    private void Awake()
    {
        exitBtn = exitButton.GetComponent<Button>();
        currentNoteTitleT = currentNoteTitleText.GetComponent<TextMeshProUGUI>();
        currentNoteContentT = currentNoteContentText.GetComponent<TextMeshProUGUI>();

        exitBtn.onClick.AddListener(CloseModal);

        PlayerSceneInteraction.DisableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.DisableAttack(INTERACTION_LOCK_KEY);
    }

    public void InitParameters(GameObject storyNotesManager)
    {
        notesManager = storyNotesManager.GetComponent<StoryNotesManager>();

        List<int> discoveredNotes = notesManager.GetDiscoveredNoteIndicies();
        foreach (int noteIndex in discoveredNotes)
        {
            StoryNote noteData = notesManager.GetNote(noteIndex);
            InitSelector(noteData);

        }

        StoryNote defaultNote = notesManager.GetNote(0);
        ViewNote(defaultNote.name, defaultNote.content);
    }

    private void CloseModal()
    {
        PlayerSceneInteraction.EnableInteraction(INTERACTION_LOCK_KEY);
        PlayerAttackMode.EnableAttack(INTERACTION_LOCK_KEY);

        Destroy(gameObject);
    }

    private void InitSelector(StoryNote noteData)
    {
        GameObject selector = Instantiate(letterSelectorPrefab, selectorsContainer.transform);

        TextMeshProUGUI selectorText = selector.GetComponentInChildren<TextMeshProUGUI>();
        Button btn = selector.GetComponent<Button>();

        selectorText.text = noteData.name;
        btn.onClick.AddListener(() => ViewNote(noteData.name, noteData.content));
    }

    private void ViewNote(string name, string content)
    {
        currentNoteTitleT.text = name;
        currentNoteContentT.SetText(content);
    }
}
