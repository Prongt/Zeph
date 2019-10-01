using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pushable : Aspects
{
    private void OnValidate()
    {
        Enum.TryParse(this.GetType().Name, out AspectType aspectType);
        AspectType = aspectType;
    }

    public override void Promote()
    {
        //Being pushed
        Debug.Log("Being pushed");
    }

    public override void Negate()
    {
        //Not pushed
        Debug.Log("Not being pushed");
    }
}
