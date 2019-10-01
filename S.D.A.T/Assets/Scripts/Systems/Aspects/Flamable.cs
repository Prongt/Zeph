using System;
using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private GameObject burningParticleEffect;

    private Material baseMaterial;

    public override void Initialize()
    {
        base.Initialize();
        baseMaterial = GetComponent<Renderer>().material;
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
        GetComponent<Renderer>().material = baseMaterial;
    }
}