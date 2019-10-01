using System;
using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private GameObject burningParticleEffect;

    public override void Initialize()
    {
        Enum.TryParse(this.GetType().Name, out AspectType aspectType);
        AspectType = aspectType;
    }

    private void Awake()
    {
        Initialize();
    }

    public override void Promote()
    {
        Debug.Log("On Fire");
        GetComponent<Renderer>().material = burnedMaterial;
    }

    public override void Negate()
    {
        //Extingushed
        Debug.Log("Extingushed");
    }
}

//Fire
/*
 * burning material
 * burning particle effect
 * burned material
 *
 * burn rate (10% health per second)
 */