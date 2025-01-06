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

    public int coins { get; set; }
    public float time {  get; set; }
    public static readonly int StarsPerLevel = 3;
    public bool[] stars { get; set; } = new bool[StarsPerLevel];

    public virtual void LoadState(LevelData data)
    {
        locked = data.locked;
        coins = data.coins;
        time = data.time;
        stars = data.stars;
    }

    public static string FormattedTime(float time)
    {
        var minutes = Mathf.FloorToInt(time / 60f);
        var second = Mathf.FloorToInt(time % 60);
        var millisecond = Mathf.FloorToInt((time * 100f) % 100);

        return minutes.ToString("0") + ":" + second.ToString("00") + ":" + millisecond.ToString("00");
    }


    public virtual LevelData ToData()
    {
        return new LevelData()
        {
            locked = this.locked,
            coins = this.coins,
            time = this.time,
            stars = this.stars
        };
    }
}
