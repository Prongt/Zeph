using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class LevelData
{
    public int level;

    public LevelData(LevelProgress curLevel)
    {
        level = curLevel.playerProgress;
    }
}
