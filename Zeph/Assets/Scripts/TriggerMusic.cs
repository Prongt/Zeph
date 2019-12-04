using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;

public class TriggerMusic : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter studioEventEmitter;

    private void Start()
    {
        GrabComponents();
        studioEventEmitter.Play();
    }

    private void OnValidate()
    {
        GrabComponents();
    }

    private void GrabComponents()
    {
        if (studioEventEmitter == null)
        {
            studioEventEmitter = GetComponent<StudioEventEmitter>();
        }
    }
}
