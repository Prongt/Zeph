using System;
using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;

public class FallReset : MonoBehaviour
{
    private PlayerMoveRigidbody playerMoveRigidbody;

    private void Start()
    {
        playerMoveRigidbody = FindObjectOfType<PlayerMoveRigidbody>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerMoveRigidbody.enabled = false;
            Physics.gravity = new Vector3(0,-9.81f,0);
            GravityRift.AltGravityIsActive = false;
            other.transform.position = CheckpointManager.curCheckpoint.transform.position;
            other.transform.rotation = CheckpointManager.curCheckpoint.transform.rotation;
            StartCoroutine(Delay());
        }
    }

    void OnTriggerStay(Collider other)
    {
        if (!other.CompareTag("Player")) return;
        
        playerMoveRigidbody.enabled = false;
        Physics.gravity = new Vector3(0,-9.81f,0);
        GravityRift.AltGravityIsActive = false;
        other.transform.position = CheckpointManager.curCheckpoint.transform.position;
        other.transform.rotation = CheckpointManager.curCheckpoint.transform.rotation;
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        playerMoveRigidbody.enabled = true;
    }
}
