using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;

public class TimeLoadModule : MonoBehaviour, ILoadModule
{
    private static readonly string JSON_KEY = "day_time_data";

    [SerializeField]
    private GameObject dayCycleManager;

    private DayNightCycle dayNightManager;

    private void Start()
    {
        dayNightManager = dayCycleManager.GetComponent<DayNightCycle>();
    }

    public string GetJsonKey()
    {
        return JSON_KEY;
    }

    public string GetJsonString()
    {
        int day = dayNightManager.GetDay();
        int time = dayNightManager.GetTime();

        DayTimeData saveData = new DayTimeData
        {
            dayInMonth = day,
            timeInDay = time
        };

        string jsonString = JsonUtility.ToJson(saveData);
        return jsonString;
    }

    public void Load(string saveJson)
    {
        JSONNode json = JSON.Parse(saveJson);
        if (json == null || saveJson == null || saveJson == "")
        {
            InitDayTime();
            return;
        }
        string objectData = json[JSON_KEY].Value;
        DayTimeData data = JsonUtility.FromJson<DayTimeData>(objectData);

        if (data == null)
        {
            InitDayTime();
            return;
        }

        dayNightManager.SetDay(data.dayInMonth);
        dayNightManager.SetTime(data.timeInDay);
    }

    private void InitDayTime()
    {
        dayNightManager.SetDay(0);
        dayNightManager.SetTime(0);
    }
}

[Serializable]
public class DayTimeData
{
    public int dayInMonth;

    public int timeInDay;
}
