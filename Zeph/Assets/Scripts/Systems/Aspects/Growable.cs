using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Growable : Aspects
{
    [SerializeField] private Material GrowingMaterial;
    [SerializeField] private GameObject GrowingParticleEffect;

    
    public Type[] componentTypes = new Type[]
    {
        typeof(AudioSource)
    };


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    public override void Promote(Transform source = null)
    {
        base.Promote(source);
    }

    public override void Negate(Transform source = null)
    {
        //Stop growing
        //Debug.Log("Shrinking");
        base.Promote(source);
    }
}