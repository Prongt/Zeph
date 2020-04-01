using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

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
}
