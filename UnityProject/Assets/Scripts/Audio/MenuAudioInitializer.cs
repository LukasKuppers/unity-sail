using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MenuAudioInitializer : MonoBehaviour
{
    [SerializeField]
    private float songLengthSec = 120f;

    private void Start()
    {
        AudioManager.GetInstance().Play(SoundMap.BACKGROUND_AMBIANCE);

        StartCoroutine(PlaySongAfterSeconds(5f, OnSongEnd));
    }

    private void OnSongEnd()
    {
        float pauseDuration = Random.Range(10, songLengthSec);
        StartCoroutine(PlaySongAfterSeconds(pauseDuration, OnSongEnd));
    }

    private IEnumerator PlaySongAfterSeconds(float seconds, UnityAction songEndedCallback)
    {
        yield return new WaitForSeconds(seconds);

        AudioManager.GetInstance().Play(SoundMap.MENU_THEME);

        yield return new WaitForSeconds(songLengthSec);
        songEndedCallback.Invoke();
    }
}
