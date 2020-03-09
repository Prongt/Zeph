using System;
using Movement;
using UnityEngine;

public class WaterKnockBack : MonoBehaviour
{
    private void OnCollisionEnter(Collision col)
    {
        if (col.collider.CompareTag("Player"))
        {
            var pMove = col.collider.GetComponent<PlayerMoveRigidbody>();
        }
    }
}
