using System;
using UnityEngine;

[Serializable]
public class GameData
{
    public int retries;
    public LevelData[] levels;
    public string createdAt;
    public string updatedAt;

    public virtual string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
    public static GameData FromJson(string json)
    {
        return JsonUtility.FromJson<GameData>(json);
    }
}