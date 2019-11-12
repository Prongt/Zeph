using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private ParticleSystem burningParticleEffect;

    [SerializeField] private bool canBeSource = false;
    [HideIf("canBeSource", true)] [SerializeField] private float fireSpreadInterval = 1;
    [HideIf("canBeSource", true)] [SerializeField] private bool useBoxCollider = false;
    [HideIf("useBoxCollider", true)] [SerializeField] private Vector3 boxDimensions;
    
    
    [HideIf("useBoxCollider", false, true)] [SerializeField] private float fireSpreadRange = 3;

    private Material baseMaterial;
    private bool isOnFire = false;
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
            
            colliders = new Collider[25];
            if (useBoxCollider)
            {
                
                Physics.OverlapBoxNonAlloc(transform.position, new Vector3(boxDimensions.x/2,boxDimensions.y/2,boxDimensions.z /2), colliders);
            }
            else
            {
                Physics.OverlapSphereNonAlloc(transform.position, fireSpreadRange, colliders);
            }
            
            
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

    private void OnDrawGizmos()
    {
        if (canBeSource == false)
        {
            return;
        }
        if (element != null)
        {
            Gizmos.color = element.DebugColor;
        }
        else
        {
            Gizmos.color = Color.red;
        }

        if (useBoxCollider)
        {
            Gizmos.DrawWireCube(transform.position, boxDimensions);
            
        }
        else
        {
            Gizmos.DrawWireSphere(transform.position, fireSpreadRange);
        }
        
    }
}