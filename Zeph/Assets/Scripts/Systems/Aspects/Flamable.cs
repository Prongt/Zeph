using System;
using System.Collections;
using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private ParticleSystem burningParticleEffect;

    private Material baseMaterial;
    
    
    public Type[] componentTypes = new Type[]
    {
        typeof(AudioSource)
    };


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
    
    
    protected override void Initialize()
    {
        base.Initialize();
        baseMaterial = GetComponent<Renderer>().material;
        burningParticleEffect.Stop();
    }

    public override void Promote(Transform source = null)
    {
        base.Promote(source);
        //Debug.Log("On Fire");
        GetComponent<Renderer>().material = burnedMaterial;
        Instantiate(burningParticleEffect.gameObject, gameObject.transform);
        burningParticleEffect.Play();
        StartCoroutine(Burn());
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Extingushed
        //Debug.Log("Extingushed");
        GetComponent<Renderer>().material = baseMaterial;
    }

    IEnumerator Burn()
    {
        //This is broken
        /*if (burnedMaterial.color.a > 1)
        {
            burnedMaterial.color = Color.Lerp(burnedMaterial.color, new Color(burnedMaterial.color.r, burnedMaterial.color.g, burnedMaterial.color.b, 0), 1 * Time.deltaTime);
        }
        StartCoroutine(Burn());*/
        yield return null;
    }
}