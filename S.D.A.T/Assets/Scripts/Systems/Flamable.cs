using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private GameObject burningParticleEffect;

    public override void Initialize()
    {
        base.Initialize();
    }

    public override void Activate()
    {
        base.Activate();
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