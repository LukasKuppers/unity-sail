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

    private string[] textSequence;
    private int currentIndex = -1;

    private void Start()
    {
        textDisplay = textDisplayObject.GetComponent<TextMeshProUGUI>();

        DelimmitedTextLoader textLoader = new DelimmitedTextLoader(textSourcFileName, '\t');
        string[][] rawData = textLoader.GetDataNoHeader();
        textSequence = new string[rawData.Length];

        for (int i = 0; i < textSequence.Length; i++)
        {
            textSequence[i] = rawData[i][0];
        }
    }

    public void StartSequenceDisplay()
    {
        currentIndex = 0;
        DisplayText(currentIndex);
    }

    public void IncrementSequence()
    {
        if (currentIndex != -1 && currentIndex < textSequence.Length - 2)
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
