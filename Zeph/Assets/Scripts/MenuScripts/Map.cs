using System;
using UnityEngine;

/// <summary>
/// The aspect class for the map gameobject
/// </summary>
public class Map : Aspects
{
    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.Map;
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
