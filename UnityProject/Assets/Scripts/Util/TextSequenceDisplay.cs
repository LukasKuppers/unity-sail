using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextSequenceDisplay : MonoBehaviour
{
    [SerializeField]
    private GameObject textDisplayObject;
    [SerializeField]
    private string textSourceFileName;
    [SerializeField]
    private float fadeTime = 1f;

    private TextMeshProUGUI textDisplay;

    private string[] textSequence;
    private int currentIndex = -1;

    private void Start()
    {
        textDisplay = textDisplayObject.GetComponent<TextMeshProUGUI>();

        if (textSourceFileName != null && textSourceFileName != "")
            SetTextSourceFile(textSourceFileName);
    }

    public void SetTextSourceFile(string fileName)
    {
        textSourceFileName = fileName;
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
        if (currentIndex != -1 && currentIndex < textSequence.Length - 1)
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

        if (textDisplay == null)
            Start();

        textDisplay.CrossFadeAlpha(0f, fadeTime, false);
        textDisplay.text = text;
        textDisplay.CrossFadeAlpha(1f, fadeTime, false);
    }
}
