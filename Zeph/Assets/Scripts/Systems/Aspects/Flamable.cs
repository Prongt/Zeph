using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private ParticleSystem burningParticleEffect;

    [SerializeField] private bool canBeSource = false;
    [SerializeField] private float fireSpreadInterval = 1;
    [SerializeField] private float fireSpreadRange = 3;
    
    private Material baseMaterial;
    private bool isOnFire = false;
    private List<Interactable> objectsToBurn = new List<Interactable>();

    public Type[] componentTypes = new Type[]
    {
        typeof(AudioSource),
        typeof(SphereCollider),
        typeof(Rigidbody)
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
        
            
        var collider = gameObject.GetComponent<SphereCollider>();
        collider.isTrigger = true;
        collider.radius = fireSpreadRange;
    }



    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        
        if (!isOnFire)
        {
            GetComponent<Renderer>().material = burnedMaterial;
            Instantiate(burningParticleEffect.gameObject, gameObject.transform);
            burningParticleEffect.Play();
        }
        
        if (canBeSource && !isOnFire)
        {
            isOnFire = true;
            StartCoroutine(FireSpread());
        }
        isOnFire = true;
    }


    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        GetComponent<Renderer>().material = baseMaterial;
        isOnFire = false;
    }

    
    IEnumerator FireSpread()
    {
        while (isOnFire)
        {
            yield return new WaitForSeconds(fireSpreadInterval);
            for (int i = 0; i < objectsToBurn.Count; i++)
            {
                objectsToBurn[i].ApplyElement(element);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var obj = other.gameObject.GetComponent<Interactable>();
        if (obj != null)
        {
            if (!objectsToBurn.Contains(obj))
            {
                objectsToBurn.Add(obj);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var obj = other.GetComponent<Interactable>();
        if (obj)
        {
            if (objectsToBurn.Contains(obj))
            {
                objectsToBurn.Remove(obj);
            }
        }
    }
}