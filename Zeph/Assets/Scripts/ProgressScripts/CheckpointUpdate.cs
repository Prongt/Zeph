using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointUpdate : CheckpointManager
{
    public bool hitCheckpoint;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("checkPoint Updated");
            if (checkPointCount < 3 && !hitCheckpoint)
            {
                checkPointCount++;
                print(checkPointCount);
                hitCheckpoint = true;
            }
            
            curCheckpoint = checkpoints[checkPointCount];
            
        }
    }
}
