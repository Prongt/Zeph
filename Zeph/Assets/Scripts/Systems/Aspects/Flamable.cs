using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;

/// <summary>
/// When activated the object is set on fire and can burn other objects
/// </summary>
public class Flamable : Aspects
{
    [SerializeField] private bool lit;
    
    private static readonly int burning = Animator.StringToHash("Burning");

    [HideIf("useBoxCollider", true)] [SerializeField]
    private Vector3 boxDimensions = default;

    [SerializeField] private Material burnedMaterial = default;
    [SerializeField] private VisualEffect burningParticleEffect = default;

    [SerializeField] private bool canBeSource = false;
    private Collider[] colliders = new Collider[10];
    

    public Type[] componentTypes =
    {
        //typeof(StudioEventEmitter),
        //typeof(Rigidbody)
        typeof(FireflyController)
    };

    [SerializeField] private bool destroyable = false;

    [Header("Audio")] [SerializeField] private StudioEventEmitter fireEventEmitter = default;
    
    [Range(0.01f, 5f)] [SerializeField] private float fireSpreadPerSecond = 0.1f;

    [HideIf("useBoxCollider", false, true)] [SerializeField]
    private float fireSpreadRange = 3;

    private WaitForSeconds fireSpreadWaitForSeconds;

    [Range(5, 25)] [SerializeField] private int maxNumberOfAffectableObjects = 15;
    //private List<Collider> colliders = new List<Collider>(25);

    private Animator myAnim;

    private Vector3 overlapBoxExtents;
    private new Renderer renderer;

    [HideIf("canBeSource", true)] [SerializeField]
    private bool useBoxCollider = false;


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }


    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.Flamable;

        renderer = GetComponent<Renderer>();


        if (burningParticleEffect)
        {
            burningParticleEffect.enabled = true;
            burningParticleEffect.Stop();
        }

        if (lit)
        {
            if (renderer != null) renderer.material = burnedMaterial;

            if (burningParticleEffect) burningParticleEffect.Play();
        }

        overlapBoxExtents = new Vector3(boxDimensions.x / 2, boxDimensions.y / 2, boxDimensions.z / 2);
        colliders = new Collider[maxNumberOfAffectableObjects];
        fireSpreadWaitForSeconds = new WaitForSeconds(fireSpreadPerSecond);


        myAnim = GetComponent<Animator>();
    }


    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);

        GetComponentInChildren<FireflyController>().interacted = true;

        if (!lit)
        {
            if (renderer != null) renderer.material = burnedMaterial;

            if (burningParticleEffect) burningParticleEffect.Play();
        }

        if (canBeSource && !lit)
        {
            lit = true;
            StartCoroutine(FireSpread());
        }

        lit = true;

        if (destroyable)
        {
            myAnim.SetBool(burning, true);
        }
        else
        {
            if (!fireEventEmitter) return;
            if (!fireEventEmitter.IsPlaying()) fireEventEmitter.Play();
        }
    }


    public override void Negate(Transform source = null)
    {
        base.Promote(source);

        // var renderer = GetComponent<Renderer>();
        // if (renderer != null)
        // {
        //     renderer.material = baseMaterial;
        // }
        //
        // isOnFire = false;
    }


    private IEnumerator FireSpread()
    {
        while (lit)
        {
            yield return fireSpreadWaitForSeconds;
            if (useBoxCollider)
                Physics.OverlapBoxNonAlloc(transform.position, overlapBoxExtents, colliders);
            else
                Physics.OverlapSphereNonAlloc(transform.position, fireSpreadRange, colliders);

            foreach (var col in colliders)
            {
                if (!col) continue;

                //TODO cache colliders that have been used previously and return before get component is called again
                var obj = col.GetComponent<Interactable>();
                if (obj) obj.ApplyElement(element, transform);
            }
        }
    }

    private void OnDestroy()
    {
        if (!fireEventEmitter) return;
        if (fireEventEmitter.IsPlaying())
        {
            fireEventEmitter.Stop();
        }
    }

    private void OnDrawGizmos()
    {
        if (canBeSource == false) return;
        Gizmos.color = element != null ? element.DebugColor : Color.red;

        if (useBoxCollider)
            Gizmos.DrawWireCube(transform.position, boxDimensions);
        else
            Gizmos.DrawWireSphere(transform.position, fireSpreadRange);
    }
}