using UnityEngine;

public class Growable : Aspects
{
    [SerializeField] private Material GrowingMaterial;
    [SerializeField] private GameObject GrowingParticleEffect;

    public override void Initialize()
    {
        base.Initialize();
    }
}