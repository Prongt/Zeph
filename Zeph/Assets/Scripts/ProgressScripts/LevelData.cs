/// <summary>
/// Level data type used for the progression system
/// </summary>
[System.Serializable]
public class LevelData
{
    public int level;

    public LevelData(LevelProgress curLevel)
    {
        level = curLevel.playerProgress;
    }
}
