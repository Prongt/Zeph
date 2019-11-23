 using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRift : Aspects
{

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
    }

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
    
    public Type[] componentTypes = new Type[]
    {

    };
}
