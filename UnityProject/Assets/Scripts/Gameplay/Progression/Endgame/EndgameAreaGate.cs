using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndgameAreaGate : MonoBehaviour
{
    [SerializeField]
    private float openedHeight;
    [SerializeField]
    private float closedHeight;
    [SerializeField]
    private float animationTime = 5f;

    // gate is open by default
    private bool isOpen = true;
    private bool animationInProgress = false;

    private void Start()
    {
        SetYPos(openedHeight);
    }

    public void SetStateNoAnim(bool isOpen)
    {
        float gateHeight = isOpen ? openedHeight : closedHeight;

        SetYPos(gateHeight);
        this.isOpen = isOpen;
    }

    public void CloseGate()
    {
        if (isOpen && !animationInProgress)
            StartCoroutine(TransitionToHeight(closedHeight));
        else if (animationInProgress)
            Debug.LogWarning("EndgameArenaGate:CloseGate: Animation in progress, close cancelled");
    }

    public void OpenGate()
    {
        if (!isOpen && !animationInProgress)
            StartCoroutine(TransitionToHeight(openedHeight));
        else if (animationInProgress)
            Debug.LogWarning("EndgameArenaGate:OpenGate: Animation in progress, open cancelled");
    }

    private IEnumerator TransitionToHeight(float height)
    {
        float originalHeight = transform.position.y;

        if (!animationInProgress && originalHeight != height)
        {
            animationInProgress = true;

            int numFrames = (int)(animationTime / Time.smoothDeltaTime);
            for (int i = 0; i < numFrames; i++)
            {
                float animPercent = (float)i / numFrames;
                float animHeight = Mathf.Lerp(originalHeight, height, animPercent);
                SetYPos(animHeight);

                yield return null;
            }

            SetYPos(height);
        }
        animationInProgress = false;
        isOpen = height == openedHeight;
    }

    private void SetYPos(float yPos)
    {
        transform.position = new Vector3(transform.position.x, yPos, transform.position.z);
    }
}
