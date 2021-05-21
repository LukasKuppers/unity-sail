using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "PlayerLoadModule_data";

    public void Load(string saveJson)
    {
       
    }

    public string SaveJson()
    {
        return "";
    }
}

[Serializable]
public class PlayerData
{
    public Vector3 position;

    public Vector3 rotation;
}

