using System;
using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private ParticleSystem burningParticleEffect;

    private Material baseMaterial;

    public override void Initialize()
    {
        base.Initialize();
        baseMaterial = GetComponent<Renderer>().material;
        burningParticleEffect.Stop();
    }

    public override void Promote(Transform source = null)
    {
        //Debug.Log("On Fire");
        GetComponent<Renderer>().material = burnedMaterial;
        Instantiate(burningParticleEffect.gameObject, gameObject.transform);
        burningParticleEffect.Play();

    }

    public override void Negate(Transform source = null)
    {
        //Extingushed
        //Debug.Log("Extingushed");
        GetComponent<Renderer>().material = baseMaterial;
    }
}