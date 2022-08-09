using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class IslandVisitBanner : MonoBehaviour, IDestructable
{
    [SerializeField]
    private GameObject backgroundObject;
    [SerializeField]
    private GameObject textObject;
    [SerializeField]
    private float fadeOutDuration = 1.0f;

    private Image background;
    private TextMeshProUGUI text;

    private void Start()
    {
        background = backgroundObject.GetComponent<Image>();
        text = textObject.GetComponent<TextMeshProUGUI>();
    }

    public void AddDestructionListener(UnityAction<GameObject> call)
    {
        throw new System.NotImplementedException();
    }

    public void Destroy()
    {
        float numFrames = fadeOutDuration / Time.smoothDeltaTime;

        StartCoroutine(FadeOutCoroutine((int)numFrames));
    }

    private IEnumerator FadeOutCoroutine(int numFrames)
    {
        for (int i = 0; i < numFrames; i++)
        {
            float animPercent = (float)i / (float)numFrames;

            float alpha = 1 - animPercent;
            
            Color bgCol = background.color;
            Color textCol = text.color;
            bgCol.a = alpha;
            textCol.a = alpha;
            background.color = bgCol;
            text.color = textCol;

            yield return null;
        }

        Destroy(gameObject);
    }
}
