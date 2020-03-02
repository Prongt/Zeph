using System;
using System.Collections;
using System.Collections.Generic;
using Movement;
using UnityEngine;
using UnityEngine.Events;

public class TriggerEndSequence : MonoBehaviour
{
    public UnityEvent OnPlayerEnter;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerMove.PlayerMovementEnabled = false;
            OnPlayerEnter.Invoke();
        }
    }
}
