using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointUpdate : CheckpointManager
{
    private bool hitCheckpoint;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hitCheckpoint)
        {
            if (checkPointCount < checkpoints.Count)
            {
                checkPointCount++;
            }
            
            curCheckpoint = checkpoints[checkPointCount];
            hitCheckpoint = true;
        }
    }
}
