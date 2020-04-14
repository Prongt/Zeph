using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Story : Aspects
{
    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.Story;
        
    }

    public Type[] componentTypes = new Type[]
    {
        typeof(FireflyController)
    };
    
    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        
        GetComponentInChildren<FireflyController>().interacted = true;
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
    }
    
}
