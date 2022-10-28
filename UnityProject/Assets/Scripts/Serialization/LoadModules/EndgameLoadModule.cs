using UnityEngine;
using System;
using SimpleJSON;

public class EndgameLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "EndgameSequence_data";

    private EndgameManager endgameManager;

    private void Start()
    {
        endgameManager = gameObject.GetComponent<EndgameManager>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public void Load(string saveJson)
    {
        JSONNode json = JSON.Parse(saveJson);
        if (json == null)
            return;

        string objectData = json[JSON_KEY].Value;
        EndgameSequenceData data = JsonUtility.FromJson<EndgameSequenceData>(objectData);

        if (data == null)
            return;

        if (data.playerBeatEndgame)
        {
            endgameManager.SetPlayerBeatEndgame();
            return;
        }

        if (!data.playerInEndgame)
        {
            endgameManager.ResetEndgameSequence();
            return;
        }

        endgameManager.SetSequenceIndex(data.stage);
    }

    public string GetJsonString()
    {
        EndgameSequenceData savedData = new EndgameSequenceData()
        {
            stage = endgameManager.GetCurrentSequenceIndex(),
            playerInEndgame = endgameManager.GetPlayerInEndgame(), 
            playerBeatEndgame = endgameManager.PlayerBeatEndgame()
        };

        string jsonString = JsonUtility.ToJson(savedData);
        return jsonString;
    }

    [Serializable]
    public class EndgameSequenceData
    {
        public int stage;

        public bool playerInEndgame;

        public bool playerBeatEndgame;
    }
}
