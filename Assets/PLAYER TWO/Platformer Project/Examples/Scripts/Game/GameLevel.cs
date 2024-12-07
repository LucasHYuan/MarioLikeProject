using System;
using UnityEngine;

[Serializable]
public class GameLevel
{
    public bool locked;
    public string scene;
    public string name;
    public string description;
    public Sprite image;

    public int coins { get; protected set; }
    public float time {  get; protected set; }
    public static readonly int StarsPerLevel = 3;
    public bool[] stars { get; protected set; } = new bool[StarsPerLevel];




}
