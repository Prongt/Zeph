using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : Aspects
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
    }

    public override void Negate(Transform source = null)
    {
        base.Negate(source);
    }
}
