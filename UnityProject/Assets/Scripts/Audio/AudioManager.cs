using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.Events;

public class AudioManager : MonoBehaviour
{
    [SerializeField]
    private Sound[] sounds;

    private Dictionary<string, AudioSource> audioSources;

    private static AudioManager singletonInstance;

    public static AudioManager GetInstance()
    {
        if (singletonInstance == null)
            singletonInstance = FindObjectOfType<AudioManager>();

        return singletonInstance;
    }

    private void Awake()
    {
        audioSources = new Dictionary<string, AudioSource>();

        foreach (Sound sound in sounds)
        {
            AudioSource source = gameObject.AddComponent<AudioSource>();

            source.clip = sound.clip;
            source.volume = sound.volume;
            source.pitch = sound.pitch;
            source.loop = sound.loop;

            audioSources.Add(sound.name, source);
        }
    }

    public void Play(string soundName)
    {
        FetchSource(soundName, (source) =>
        {
            source.Play();
        });
    }

    public void PlayWithCompletionCallback(string soundName, UnityAction callback)
    {
        FetchSource(soundName, (source) =>
        {
            source.Play();
            StartCoroutine(SoundClipCompletionCallback(source.clip.length, callback));
        });
    }

    public void Pause(string soundName)
    {
        FetchSource(soundName, (source) =>
        {
            source.Pause();
        });
    }

    private AudioSource FetchSource(string soundName, UnityAction<AudioSource> callback)
    {
        if (audioSources.ContainsKey(soundName))
        {
            AudioSource source = audioSources[soundName];
            callback.Invoke(source);
            return source;
        }
        else
            Debug.LogWarning($"AudioManager:Play: Sound {soundName} does not exist");
        return null;
    }

    private IEnumerator SoundClipCompletionCallback(float clipTime, UnityAction callback)
    {
        yield return new WaitForSeconds(clipTime);
        callback.Invoke();
    }
}
