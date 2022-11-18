using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LeaderboardEntry : MonoBehaviour
{
    [SerializeField]
    private GameObject scoreTextObject;
    [SerializeField]
    private GameObject nameTextObject;

    private TextMeshProUGUI scoreText;
    private TextMeshProUGUI nameText;

    public void InitParameters(int score, string name)
    {
        scoreText = scoreTextObject.GetComponent<TextMeshProUGUI>();
        nameText = nameTextObject.GetComponent<TextMeshProUGUI>();

        scoreText.text = score.ToString();
        nameText.text = name;
    }

    public void InitParameters(string name)
    {
        nameText = nameTextObject.GetComponent<TextMeshProUGUI>();
        nameText.text = name;
    }
}
