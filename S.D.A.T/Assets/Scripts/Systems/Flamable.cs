using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private GameObject burningParticleEffect;

    public override void Initialize()
    {
        base.Initialize();
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
