﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Chargeable : Aspects
{
    private float intensity;
    [SerializeField] private Material emissiveCrystal;
    [SerializeField] private Renderer myRend;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    
    protected override void Initialize()
    {
        base.Initialize();

    }

    public Type[] componentTypes = new Type[]
    {
        
    };
    
    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        myRend.material = emissiveCrystal;
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
    }

}