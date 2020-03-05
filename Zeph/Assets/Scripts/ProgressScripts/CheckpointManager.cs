using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<GameObject> checkpoints;
    public static GameObject curCheckpoint;
    
    //This script is put on a trigger currently. Works best for 2 checkpoint levels right now
    //this is put on the second checkpoint
    
    //TODO Rewrite code to be more flexible for more checkpoints.
    
    void Start()
    {
        curCheckpoint = checkpoints[0];
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            curCheckpoint = checkpoints[1];
        }
    }
}
