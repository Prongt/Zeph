using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DisablePlayerGravityBlocker : MonoBehaviour
{
    public static int count = 0;
    private int myCount;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            count++;
            myCount++;
            PlayerMove.PlayerUsesGravity = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            count--;
            myCount--;
            if (count <= 0)
            {
                PlayerMove.PlayerUsesGravity = true;
            }
           
        }
    }

    private void OnDisable()
    {
        if (myCount > 0)
        {
            myCount--;
            count--;
        }
        if (PlayerMove.PlayerUsesGravity == false)
        {
            PlayerMove.PlayerUsesGravity = true;
        }
    }
}
