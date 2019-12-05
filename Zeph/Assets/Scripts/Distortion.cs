using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Distortion : Aspects
{
    [SerializeField] private Animator disAnim1;


    public static bool isDistorting = false;
    private bool animating = false;

    [SerializeField] private bool onGround;

    protected override void Initialize()
    {
        base.Initialize();
        isDistorting = false;
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source,element);
        
        animating = !animating;

        isDistorting = !isDistorting;
        
        if (disAnim1 != null)
        disAnim1.SetBool("Animate", animating);
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

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
}
