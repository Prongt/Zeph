 using System;
 using UnityEngine;

 /// <summary>
 /// When activated objects in the scene are enabled or disabled via unity event
 /// </summary>
public class FadeRift : Aspects
{
    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.FadeRift;
    }

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
