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

        if (textSourcFileName != null && textSourcFileName != "")
            SetTextSourceFile(textSourcFileName);
    }

    public void SetTextSourceFile(string fileName)
    {
        textSourcFileName = fileName;
        DelimmitedTextLoader textLoader = new DelimmitedTextLoader(fileName, '\t');
        string[][] rawData = textLoader.GetData();
        textSequence = new string[rawData.Length];

        for (int i = 0; i < textSequence.Length; i++)
        {
            textSequence[i] = rawData[i][0];
        }

    }

    public void StartSequenceDisplay()
    {
        if (textSequence != null)
        {
            currentIndex = 0;
            DisplayText(currentIndex);
        }
        else
            Debug.LogWarning("TextSequenceDisplay:StartSequenceDisplay: text source file not set");
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
