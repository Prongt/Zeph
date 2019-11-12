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
    private Collider[] colliders = new Collider[25];

    public Type[] componentTypes = new Type[]
    {
        typeof(AudioSource),
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
        
            
//        var collider = gameObject.GetComponent<SphereCollider>();
//        collider.isTrigger = true;
//        collider.radius = fireSpreadRange;
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
//            for (int i = 0; i < objectsToBurn.Count; i++)
//            {
//                objectsToBurn[i].ApplyElement(element);
//            }

            colliders = new Collider[25];
            Physics.OverlapSphereNonAlloc(transform.position, fireSpreadRange, colliders);
            for (int i = 0; i < colliders.Length; i++)
            {
                var collisionObj = colliders[i];
                    
                if (collisionObj)
                {
                    var obj = collisionObj.GetComponent<Interactable>();
                    if (obj)
                    {
//                        float objY = obj.transform.position.y;
//                        float playerY = transform.position.y;

//                        if (Mathf.Abs(objY - playerY) < height)
//                        {
//                            obj.ApplyElement(element, gameObject.transform);
//                        }
                        obj.ApplyElement(element, transform);
                    }
                }
            }
        }
    }

//    private void OnTriggerEnter(Collider other)
//    {
//        var obj = other.gameObject.GetComponent<Interactable>();
//        if (obj != null)
//        {
//            if (!objectsToBurn.Contains(obj))
//            {
//                objectsToBurn.Add(obj);
//            }
//        }
//    }
//
//    private void OnTriggerExit(Collider other)
//    {
//        var obj = other.GetComponent<Interactable>();
//        if (obj)
//        {
//            if (objectsToBurn.Contains(obj))
//            {
//                objectsToBurn.Remove(obj);
//            }
//        }
//    }
}