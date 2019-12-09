using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Distortion : Aspects
{
    [SerializeField] private Animator myAnim;


    public static bool isDistorting = false;
    private bool animating = false;

    [SerializeField] private bool onGround;
    

    protected override void Initialize()
    {
        base.Initialize();
        isDistorting = false;

        if (!gameObject.CompareTag("Rift"))
        {
            myAnim = GetComponent<Animator>();
        }
        else
        {
            myAnim = null;
        }
    }

    void Update()
    {
        if (!gameObject.CompareTag("Rift"))
        {
            if (isDistorting)
            {
                myAnim.SetBool("Distort", true);
            }
            else
            {
                myAnim.SetBool("Distort", false);
            }
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
        
        
        
        animating = !animating;

        if (gameObject.CompareTag(("Rift")))
        {
            isDistorting = !isDistorting;
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

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
}
