using System;
using UnityEngine;

public class Growable : Aspects
{
    [SerializeField] private Material GrowingMaterial;
    [SerializeField] private GameObject GrowingParticleEffect;


    public override void Promote(Transform source = null)
    {
        //Growing
        //Debug.Log("Growing");
    }

    public override void Negate(Transform source = null)
    {
        //Stop growing
        //Debug.Log("Shrinking");
    }
}