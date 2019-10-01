using System;
using UnityEngine;

public class Growable : Aspects
{
    [SerializeField] private Material GrowingMaterial;
    [SerializeField] private GameObject GrowingParticleEffect;

    public override void Initialize()
    {
        Enum.TryParse(this.GetType().Name, out AspectType aspectType);
        AspectType = aspectType;
    }

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