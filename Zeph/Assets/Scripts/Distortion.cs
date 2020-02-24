﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using FMODUnity;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class Distortion : Aspects
{
    [SerializeField] private Animator myAnim;


    public static bool IsDistorting;
    private bool animating;

    [SerializeField] private bool onGround;
    [SerializeField] private GameObject effect;
    [SerializeField] private GameObject distortionEffect;
    [SerializeField] private StudioEventEmitter distortionEventEmitter;
    [SerializeField] private GameObject chromaticAberration;

    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.Distortion;
        IsDistorting = false;

        myAnim = !gameObject.CompareTag("Rift") ? GetComponent<Animator>() : null;

        CheckIfChromaticAberrationAttached();

        if (!gameObject.CompareTag("Rift"))
        {
            effect = null;
            distortionEffect = null;
        }
    }

    private void Update()
    {
        if (!gameObject.CompareTag("Rift"))
        {
            myAnim.SetBool(distort, IsDistorting);
        }

        if (gameObject.CompareTag("Rift"))
        {
            if (effect) effect.SetActive(!IsDistorting);
            if (distortionEffect) distortionEffect.SetActive(IsDistorting);
        }
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        if (isCoolDownComplete == false)
        {
            Debug.Log("Cool down not complete on " + gameObject.name);
            return;
        }
        base.Promote(source,element);

        if (distortionEventEmitter)
        {
            distortionEventEmitter.Play();
        }

        animating = !animating;

        if (gameObject.CompareTag(("Rift")))
        {
            IsDistorting = !IsDistorting;

            if (CheckIfChromaticAberrationAttached())
            {
                chromaticAberration.SetActive(IsDistorting);
            }
            
        }
    }

    private bool CheckIfChromaticAberrationAttached()
    {
        if (chromaticAberration)
        {
            return true;
        }
        else
        {
            Debug.LogWarning("no Chromatic Aberration object found");
            return false;
        }
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Not pushed
        //Debug.Log("Not being pushed");
    }


    public Type[] componentTypes = new Type[]
    {

    };

    private static readonly int distort = Animator.StringToHash("Distort");

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
}
