using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;

public class FallReset : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.GetComponent<PlayerMove>().enabled = false;
            Physics.gravity = new Vector3(0,-9.81f,0);
            GravityRift.useNewGravity = false;
            other.transform.position = CheckpointManager.curCheckpoint.transform.position;
            other.transform.rotation = CheckpointManager.curCheckpoint.transform.rotation;
            StartCoroutine(Delay());
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(1);
        GameObject.Find("Zeph").GetComponent<PlayerMove>().enabled = true;
    }
}
