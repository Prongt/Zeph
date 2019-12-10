using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TriggerMusic : MonoBehaviour
{
    [SerializeField] private StudioEventEmitter studioEventEmitter;
    [SerializeField] private bool DontDestroyOnLoad = false;
    private void Start()
    {
        GrabComponents();
        studioEventEmitter.Play();
        if (DontDestroyOnLoad)
        {
            DontDestroyOnLoad(this.gameObject);
        }
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
