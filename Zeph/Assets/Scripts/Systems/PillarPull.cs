using System;
using System.Collections;
using System.Collections.Generic;
using Boo.Lang.Environments;
using UnityEngine;

public class PillarPull : Aspects
{
    private Animator myAnim;
    
    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
    
    public Type[] componentTypes = new Type[]
    {
        typeof(Animator),
    };
    
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }
    

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        Vector3 direction = gameObject.transform.position - source.position;
        var dot = Vector3.Dot(direction, gameObject.transform.right);
        if (gameObject.CompareTag("SnowPillar"))
        {
            dot = Vector3.Dot(direction, gameObject.transform.forward);
        }
        print(dot);
        if (dot > 0)
        {
            myAnim.SetBool("Fall", true);
        }
    }

    public override void Negate(Transform source = null)
    {
        base.Negate(source);
    }
    
}
