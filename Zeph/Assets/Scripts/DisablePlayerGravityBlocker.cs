using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerGravityBlocker : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove.PlayerUsesGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove.PlayerUsesGravity = true;
        }
    }
}
