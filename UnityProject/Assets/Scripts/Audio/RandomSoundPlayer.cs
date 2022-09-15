using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSoundPlayer : MonoBehaviour
{
    [SerializeField]
    private AudioClip[] soundOptions;
    [SerializeField]
    private bool playOnAwake = false;

    private AudioSource source;

    private void Awake()
    {
        source = gameObject.GetComponent<AudioSource>();

        if (playOnAwake)
            Play();
    }

    public void Play()
    {
        int randSoundIndex = Random.Range(0, soundOptions.Length);
        AudioClip randSound = soundOptions[randSoundIndex];

        source.clip = randSound;
        source.Play();
    }
}
