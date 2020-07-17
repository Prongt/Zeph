using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages the progress checkpoints for each level
/// </summary>
public class CheckpointManager : MonoBehaviour
{
    public static List<GameObject> checkpoints;
    [Tooltip("Set your checkpoints here")]
    public List<GameObject> setCheckpoints; 
    public static GameObject curCheckpoint;
    protected static int checkPointCount = 0;
    
    
    void Start()
    {
        checkpoints = setCheckpoints;
        curCheckpoint = checkpoints[checkPointCount];
    }

    private void Update()
    {
        checkpoints = setCheckpoints;
        curCheckpoint = checkpoints[checkPointCount];
    }
}
