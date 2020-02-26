using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Chargeable : Aspects
{
    private float intensity;
    [SerializeField] private Material emissiveCrystal;
    [SerializeField] private Renderer myRend;
    private Material startMat;
    public bool charged = false;
    public bool attached = false;

    // Start is called before the first frame update
    void Start()
    {
        startMat = myRend.material;
    }

    void Update()
    {
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
        myRend.material = emissiveCrystal;
        charged = true;
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
    }

    public void Charge()
    {
        if (attached)
        {
            charged = true;
        }
    }

}
