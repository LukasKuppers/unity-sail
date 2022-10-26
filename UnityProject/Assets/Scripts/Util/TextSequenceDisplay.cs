using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSequenceDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject textDisplayObject;
    [SerializeField]
    private string textSourcFileName;

    private TextMeshProUGUI textDisplay;

    private Dictionary<int, string> textSequence;
    private int currentIndex = -1;

    private void Start()
    {
        textDisplay = textDisplayObject.GetComponent<TextMeshProUGUI>();

        DelimmitedTextLoader textLoader = new DelimmitedTextLoader(textSourcFileName, '\t');
        string[][] rawData = textLoader.GetDataNoHeader();
        textSequence = new Dictionary<int, string>();

        foreach (string[] row in rawData)
        {
            textSequence.Add(int.Parse(row[0]), row[1]);
        }
    }

    public void StartSequenceDisplay()
    {
        currentIndex = 0;
        DisplayText(currentIndex);
    }

    public void IncrementSequence()
    {
        if (currentIndex != -1 && currentIndex < textSequence.Count - 2)
        {
            currentIndex++;
            DisplayText(currentIndex);
        }
    }

    public void EndSequenceDisplay()
    {
        textDisplay.text = "";
    }

    private void DisplayText(int index)
    {
        string text = textSequence[index];
        textDisplay.text = text;
    }
}
