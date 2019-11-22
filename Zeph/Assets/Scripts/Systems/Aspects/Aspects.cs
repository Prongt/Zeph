﻿using System;
using UnityEngine;
using UnityEngine.Events;

public abstract class Aspects : MonoBehaviour
{
    [HideInInspector] public AspectType AspectType;

    [SerializeField] private UnityEvent onPromote;
    [SerializeField] private UnityEvent onNegate;
    protected Element element;
    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        Enum.TryParse(GetType().Name, out AspectType aspectType);
        //Debug.Log(aspectType);
        AspectType = aspectType;
        
    }

    public abstract Type[] RequiredComponents();



    public virtual void Promote(Transform source = null, Element element = null)
    {
        if (element != null)
        {
            this.element = element;
        }
        onPromote.Invoke();
    }


    public virtual void Negate(Transform source = null)
    {
        onNegate.Invoke();
    }
}


[Serializable]
public enum AspectType
{
    Flamable,
    Growable,
    Orbitable,
    GravityRift,
    Distortion,
    FadeRift,
    Story
}