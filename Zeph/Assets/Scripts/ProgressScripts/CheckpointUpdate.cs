using UnityEngine;

/// <summary>
/// Updates the checkpoint manager when a checkpoint has been reached by the player
/// </summary>
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
