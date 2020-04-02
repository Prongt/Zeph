using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointUpdate : CheckpointManager
{
    public bool hitCheckpoint;
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hitCheckpoint)
        {
            print("checkPoint Updated");
            if (checkPointCount < 3)
            {
                   
                checkPointCount++;
                print(checkPointCount); 
            }
            
            curCheckpoint = checkpoints[checkPointCount];
            hitCheckpoint = true;
        }
    }
}
