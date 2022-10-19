using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    private float combatDuration = 5.0f;
    [SerializeField]
    private float maxAmbientTrackWaitTime = 180f;
    [SerializeField]
    private float trackFadeOutTime = 1f;

    [SerializeField]
    private string[] ambientTrackNames;
    [SerializeField]
    private string[] dangerTrackNames;
    [SerializeField]
    private string[] combatTrackNames;

    private enum States { SILENT, AMBIENT, DANGER, COMBAT };

    private States state;
    private string runningTrack = null;

    private Coroutine combatDurationCoroutine;
    private bool ambientQueueTimerRunning = false;

    private AudioManager audioManager;
    private static MusicManager singletonInstance;

    public static MusicManager GetInstance()
    {
        if (singletonInstance == null)
            singletonInstance = FindObjectOfType<MusicManager>();

        return singletonInstance;
    }

    private void Start()
    {
        audioManager = AudioManager.GetInstance();

        state = States.SILENT;
        StopAndQueueAmbient();
    }

    public void StopMusic()
    {
        if (runningTrack != null)
            audioManager.Pause(runningTrack, trackFadeOutTime);

        runningTrack = null;
        state = States.SILENT;
        StopAndQueueAmbient();
    }

    public void SpotEnemy()
    {
        if (state == States.SILENT || 
            state == States.AMBIENT)
        {
            PlayMusic(dangerTrackNames);
            state = States.DANGER;
        }
    }

    public void TriggerCombat()
    {
        if (state == States.SILENT ||
            state == States.AMBIENT ||
            state == States.DANGER)
        {
            PlayMusic(combatTrackNames);
            state = States.COMBAT;
        }

        if (combatDurationCoroutine != null)
            StopCoroutine(combatDurationCoroutine);

        combatDurationCoroutine = StartCoroutine(CombatDurationTimer());
    }

    // nobody can call EscapeEnemy(), instead, it is called when 
    // ships no longer call EnterCombat()
    private void EscapeEnemy()
    {
        if (state == States.COMBAT)
        {
            StopAndQueueAmbient();
            state = States.SILENT;
        }
    }

    public void ExitEnemyVicinity()
    {
        if (state == States.DANGER)
        {
            StopAndQueueAmbient();
            state = States.SILENT;
        }
    }

    private void PlayMusic(string[] candidateTrackNames)
    {
        if (runningTrack != null)
            audioManager.Pause(runningTrack, trackFadeOutTime);

        int randTrackIndex = Random.Range(0, candidateTrackNames.Length);
        string track = candidateTrackNames[randTrackIndex];

        audioManager.Play(track);
        runningTrack = track;
    }

    private void StopAndQueueAmbient()
    {
        if (runningTrack != null)
            audioManager.Pause(runningTrack, trackFadeOutTime);

        runningTrack = null;
        float randWaitTime = Random.Range(30f, maxAmbientTrackWaitTime);

        if (!ambientQueueTimerRunning)
            StartCoroutine(QueueAmbientTrack(randWaitTime));

    }

    private void OnAmbientTrackCompletion()
    {
        if (state == States.AMBIENT)
        {
            runningTrack = null;
            state = States.SILENT;
            StopAndQueueAmbient();
        }
    }

    private IEnumerator CombatDurationTimer()
    {
        yield return new WaitForSeconds(combatDuration);
        EscapeEnemy();
    }

    private IEnumerator QueueAmbientTrack(float waitTime)
    {
        ambientQueueTimerRunning = true;
        yield return new WaitForSeconds(waitTime);

        if (state == States.SILENT)
        {
            int randTrackIndex = Random.Range(0, ambientTrackNames.Length);
            string track = ambientTrackNames[randTrackIndex];
            audioManager.PlayWithCompletionCallback(track, OnAmbientTrackCompletion);
            runningTrack = track;

            state = States.AMBIENT;
        }

        ambientQueueTimerRunning = false;
    }
}
