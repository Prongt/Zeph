using System;
using UnityEngine;

public class Growable : Aspects
{
    [SerializeField] private Material GrowingMaterial;
    [SerializeField] private GameObject GrowingParticleEffect;


    public override void Promote()
    {
        //Growing
        Debug.Log("Growing");
    }

    public override void Negate()
    {
        //Stop growing
        Debug.Log("Shrinking");
    }
}