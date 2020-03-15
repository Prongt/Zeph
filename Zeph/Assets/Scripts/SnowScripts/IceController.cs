﻿using System;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

public class IceController : MonoBehaviour
{
    private Renderer meshRenderer;

    private float desiredValue = -4.7f;
    private float valueToSet;
    private float lerpTime = 1;
    [SerializeField] private float freezeTime = 0.25f;
    [SerializeField] [EventRef] private string freezeEvent = default;
    [SerializeField] private float meltTime = 0.25f;
    [SerializeField] [EventRef] private string meltEvent = default;
    [SerializeField] private bool dontReFreeze = false;
    private static readonly int iceLevel = Shader.PropertyToID("iceLevel");
    private Collider col;


    private void Awake()
    {
        meshRenderer = GetComponent<Renderer>();
        if (!meshRenderer) meshRenderer = GetComponentInChildren<Renderer>();

        col = GetComponent<Collider>();
    }

    private void SetIceOverTime(float value, float time)
    {
        desiredValue = value;
        lerpTime = time;
    }

    private void Update()
    {
        valueToSet = math.lerp(meshRenderer.material.GetFloat(iceLevel), desiredValue, lerpTime * Time.deltaTime);
        meshRenderer.material.SetFloat(iceLevel, valueToSet);
    }

    [ContextMenu("Melt")]
    public void Melt()
    {
        SetIceOverTime(5.2f, meltTime);
        col.isTrigger = true;
        //Debug.Log("Melt");

        if (meltEvent != String.Empty)
        {
            RuntimeManager.PlayOneShot(meltEvent, transform.position);
        }
    }

    [ContextMenu("Freeze")]
    public void Freeze()
    {
        if (dontReFreeze) return;

        SetIceOverTime(-4.7f, freezeTime);
        col.isTrigger = false;
        
        if (freezeEvent != String.Empty)
        {
            RuntimeManager.PlayOneShot(freezeEvent, transform.position);
        }
    }
}