using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Rendering;

public class Chargeable : Aspects
{
    private float intensity;
    [Tooltip("The material used if the crystal has been charged")]
    [SerializeField] private Material emissiveCrystal;
    [Tooltip("The crystal's renderer")]
    [SerializeField] private Renderer myRend;
    private Material startMat;
    
    [Header("Crystal State")]
    [Tooltip("Change this if you want the crystal to be charged and emitting light")]
    public bool charged = false;
    [Tooltip("Change this if you want the crystal to be think its attached to a mirror")]
    public bool attached = false;
    
    void Start()
    {
        //Gets the original material
        startMat = myRend.material;
    }

    void Update()
    {
        //Changes if inspector values are changed
        if (charged)
        { 
            myRend.material = emissiveCrystal;
        }
        else
        {
            myRend.material = startMat;
        }
    }
    
    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.Chargeable;

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
        charged = true;
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
    }

    //Function for the mirrors to activate using Unity Events
    public void Charge()
    {
        if (attached)
        {
            charged = true;
        }
    }

}
