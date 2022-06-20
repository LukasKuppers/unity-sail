using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonAudio : MonoBehaviour
{
    [SerializeField]
    private string soundName;

    private AudioManager audioManager;

    private void Start()
    {
        audioManager = AudioManager.GetInstance();
        Button button = gameObject.GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(PlayClickSound);
    }

    private void PlayClickSound()
    {
        audioManager.Play(soundName);
    }
}
