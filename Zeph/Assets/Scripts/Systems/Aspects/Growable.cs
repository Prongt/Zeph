﻿using System;
using FMODUnity;
using UnityEngine;

public class Growable : Aspects
{
    protected Animator Animator;
    protected Renderer Renderer;
    [SerializeField] protected Colliders colliders;
    [SerializeField] protected StudioEventEmitter growSoundEmitter;
    protected bool HasGrown;

    
    
    [SerializeField] private ParticleSystem firefly;
    private ParticleSystem.EmissionModule fireflyRate;

    protected bool LightShining;


    public Type[] componentTypes =
    {
    };
    
    protected static readonly int distort = Animator.StringToHash("Distort");
    protected static readonly int grow = Animator.StringToHash("Grow");
    protected static readonly int growing = Animator.StringToHash("Growing");


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    protected override void Initialize()
    {
        base.Initialize();
        
        if (firefly) fireflyRate = firefly.emission;

        Renderer = GetComponentInChildren<Renderer>();
        Animator = GetComponentInChildren<Animator>();
    }


    public void LightShine()
    {
        LightShining = true;
    }


    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);

        if (firefly) fireflyRate.rateOverTime = 0;

        GrowSound();
    }

    private void GrowSound()
    {
        if (growSoundEmitter)
            if (!HasGrowSoundBeenPlayed)
                growSoundEmitter.Play();


        HasGrowSoundBeenPlayed = true;
    }


    public override void Negate(Transform source = null)
    {
        base.Promote(source);
    }


    [Serializable]
    protected struct Colliders
    {
        public Collider small;
        public Collider grown;
        public Collider distort;
    }
}