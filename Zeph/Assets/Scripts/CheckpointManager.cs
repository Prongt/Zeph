using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public List<GameObject> checkpoints;
    public static GameObject curCheckpoint;

    // Start is called before the first frame update
    void Start()
    {
        curCheckpoint = checkpoints[0];
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            print("hit Trigger");
            curCheckpoint = checkpoints[1];
        }
    }
}
