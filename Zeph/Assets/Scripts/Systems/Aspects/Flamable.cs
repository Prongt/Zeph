using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

public class Flamable : Aspects
{
    [SerializeField] private Material burnedMaterial;
    [SerializeField] private VisualEffect burningParticleEffect;

    [SerializeField] private bool destroyable = false;
    [HideIf("destroyable", true)] [SerializeField] private float destroyTime = 1.0f;

    [SerializeField] private bool canBeSource = false;
    
    [HideIf("canBeSource", true)] [SerializeField] private bool useBoxCollider = false;
    [HideIf("useBoxCollider", true)] [SerializeField] private Vector3 boxDimensions;

    [SerializeField] private ParticleSystem firefly;
    private ParticleSystem.EmissionModule fireflyRate;
    
    [HideIf("useBoxCollider", false, true)] [SerializeField] private float fireSpreadRange = 3;
    [Range(0.01f, 5f)] [SerializeField] private float fireSpreadPerSecond = 0.1f;
    [Range(5, 25)] [SerializeField] private int maxNumberOfAffectableObjects = 15;

    [Header("Audio")] [SerializeField] private StudioEventEmitter fireEventEmitter;

    private Material baseMaterial;
    private bool isOnFire = false;
    private Collider[] colliders = new Collider[10];

    private Vector3 overlapBoxExtents;
    //private List<Collider> colliders = new List<Collider>(25);

    private Animator myAnim;

    private WaitForSeconds fireSpreadWaitForSeconds;

    public Type[] componentTypes = new Type[]
    {
        //typeof(StudioEventEmitter),
        //typeof(Rigidbody)
    };


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
    
    
    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.Flamable;
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            baseMaterial = renderer.material;
        }

        if (burningParticleEffect)
        {
            burningParticleEffect.enabled = true;
            burningParticleEffect.Stop();
        }
        
        overlapBoxExtents = new Vector3(boxDimensions.x / 2, boxDimensions.y / 2, boxDimensions.z / 2);
        colliders = new Collider[maxNumberOfAffectableObjects];
        fireSpreadWaitForSeconds = new WaitForSeconds(fireSpreadPerSecond);

        if (fireSpreadPerSecond < 0.01f)
        {
            //Debug.LogWarning("The fire spread interval on " + gameObject.name + " is too low this may cause performance issues");
        }

        myAnim = GetComponent<Animator>();
        if (firefly)
        {
            if (!gameObject.CompareTag("Log"))
            {
                fireflyRate = firefly.emission;
            }
            else
            {
                firefly = null;
            }
        }
    }


    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        if (!gameObject.CompareTag("Log") && firefly)
        {
            fireflyRate.rateOverTime = 0;
        }

        if (!isOnFire)
        {
            var renderer = GetComponent<Renderer>();
            if (renderer != null)
            {
                renderer.material = burnedMaterial;
            }
            
            //Instantiate(burningParticleEffect.gameObject, gameObject.transform);
            if (burningParticleEffect)
            {
                burningParticleEffect.Play();
            }
            
        }
        
        if (canBeSource && !isOnFire)
        {
            isOnFire = true;
            StartCoroutine(FireSpread());
        }
        isOnFire = true;

        if (destroyable)
        {
//            Debug.Log("Destroying " + gameObject.name + " in " + destroyTime + " seconds");
            myAnim.SetBool("Burning", true);
        }
        else
        {
            if (fireEventEmitter)
            {
                if (!fireEventEmitter.IsPlaying())
                {
                    fireEventEmitter.Play();
                }
            }
        }
    }


    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = baseMaterial;
        }

        isOnFire = false;
    }

    
    IEnumerator FireSpread()
    {
        while (isOnFire)
        {
            yield return fireSpreadWaitForSeconds;
            
            
            
            //colliders = new Collider[maxNumberOfAffectableObjects];

            
            if (useBoxCollider)
            {
                Physics.OverlapBoxNonAlloc(transform.position, overlapBoxExtents, colliders);
            }
            else
            {
                Physics.OverlapSphereNonAlloc(transform.position, fireSpreadRange, colliders);
            }

            foreach (Collider col in colliders)
            {
                if (col)
                {
                    //TODO cache colliders that have been used previously and return before get component is called again
                    var obj = col.GetComponent<Interactable>();
                    if (obj)
                    {
                        obj.ApplyElement(element, transform);
                    }
                }
                
            }
            
            //colliders.Clear();
            
//            for (int i = 0; i < colliders.Length; i++)
//            {
//                var collisionObj = colliders[i];
//                    
//                if (collisionObj)
//                {
//                    var obj = collisionObj.GetComponent<Interactable>();
//                    if (obj)
//                    {
//                        obj.ApplyElement(element, transform);
//                    }
//                }
//            }
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