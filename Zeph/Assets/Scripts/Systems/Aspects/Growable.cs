using System;
using System.Collections;
using FMODUnity;
using UnityEngine;

public class Growable : Aspects
{
    public Animator myAnim;
    [SerializeField] private Material mat;
    private float matX;
    [SerializeField] private Animator groundDistort;


    [SerializeField] private bool isTree = false;
    [SerializeField] private Colliders colliders;

    [SerializeField] private StudioEventEmitter growSoundEmitter;

    private SkinnedMeshRenderer meshRenderer;
    private Mesh mesh;

    private bool hasGrown = false;

    [SerializeField] private ParticleSystem firefly;
    private ParticleSystem.EmissionModule fireflyRate;

    private bool lightShining;


    public Type[] componentTypes = new Type[]
    {
    };


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

   protected override void Initialize()
    {
        base.Initialize();
        fireflyRate = firefly.emission;

        if (gameObject.CompareTag("Plant"))
        {
            myAnim = GetComponent<Animator>();
        }

        if (gameObject.CompareTag("Bridge"))
        {
            myAnim = GetComponent<Animator>();
            mat = gameObject.GetComponent<SkinnedMeshRenderer>().material;
            matX = mat.GetFloat("Vector1_B0F27FFD");
        }

        if (isTree)
        {
            myAnim = GetComponent<Animator>();
            meshRenderer = GetComponent<SkinnedMeshRenderer>();

            colliders.small.isTrigger = false;
            colliders.distort.enabled = false;
            colliders.grown.enabled = false;

            if (meshRenderer == null)
            {
               //Debug.Log("No SkinnedMeshRenderer on " + gameObject.name + " id: " + gameObject.GetInstanceID());
            }
        }
    }

   public void LightShine()
   {
       lightShining = true;
   }

   void Update()
   {
       if (isTree)
       {
           if (myAnim.GetBool("Distort") && myAnim.GetBool("Grow"))
           {
               colliders.small.isTrigger = true;
               colliders.distort.enabled = true;
               colliders.grown.enabled = false;
           }

           if (myAnim.GetBool("Distort") == false && myAnim.GetBool("Grow"))
           {
               colliders.small.isTrigger = true;
               colliders.distort.enabled = false;
               colliders.grown.enabled = true;
           }

           if (Distortion.isDistorting)
           {
               myAnim.SetBool("Distort", true);
               colliders.small.isTrigger = true;
               colliders.distort.enabled = true;
               colliders.grown.enabled = false;
           }
           else
           {
               myAnim.SetBool("Distort", false);
               colliders.small.isTrigger = true;
               colliders.distort.enabled = false;
               colliders.grown.enabled = true;
           }
       }


       if (gameObject.CompareTag("Bridge"))
       {
           mat.SetFloat("Vector1_B0F27FFD", matX);

           if (lightShining)
           {
               StartCoroutine(Appear());
           }
           
           if (Distortion.isDistorting)
           {
               myAnim.SetBool("Distort", true);
           }
           else
           {
               myAnim.SetBool("Distort", false);
           }

           if (myAnim.GetBool("Distort") && matX < 1 && matX > -13)
           {
               matX -= 5 * Time.deltaTime;
           }
       }
   }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);

        if (firefly)
        {
            fireflyRate.rateOverTime = 0;
        }
        

        if (growSoundEmitter)
        {
            growSoundEmitter.Play();
        }

        if (isTree)
        {
            myAnim.SetBool("Grow", true);
        }

        if (gameObject.CompareTag("Plant"))
        {
            myAnim.SetBool("Growing", true);
            colliders.grown.enabled = true;
        }
        //gameObject.GetComponent<Renderer>().material = GrowingMaterial;
        if (gameObject.CompareTag("Bridge"))
        {
            if (matX >= 1)
            {
                if (growSoundEmitter)
                {
                    if (groundDistort.GetBool("Distort"))
                    {
                        growSoundEmitter.SetParameter("Distortion", 1.0f);
                    }
                    else
                    {
                        growSoundEmitter.SetParameter("Distortion", 0.0f);
                    }
                }


                myAnim.SetBool("Growing", true);
                StartCoroutine(Appear());
            }
        }
    }

    public override void Negate(Transform source = null)
    {
        //Stop growing
        //Debug.Log("Shrinking");
        base.Promote(source);
    }

    IEnumerator Appear()
    {
        yield return new WaitForSeconds(0f);
        if (groundDistort != null)
        {
            if (groundDistort.GetBool("Distort") && hasGrown)
            {
                matX = -14;
            }

            if (!groundDistort.GetBool("Distort"))
            {
                if (matX >= 1)
                {
                    matX -= 5 * Time.deltaTime;
                    StartCoroutine(Appear());
                }
                else
                {
                    hasGrown = true;

                    StopCoroutine(Appear());
                }
            }
            else if (groundDistort.GetBool("Distort"))
            {
                if (matX >= -13)
                {
                    matX -= 5 * Time.deltaTime;
                    StartCoroutine(Appear());
                }
                else
                {
                    hasGrown = true;
                    StopCoroutine(Appear());
                }
            }
        }
        else
        {
            if (matX >= 1)
            {
                matX -= 5 * Time.deltaTime;
                StartCoroutine(Appear());
            }
            else
            {
                hasGrown = true;
                StopCoroutine(Appear());
            }
        }
    }

    [Serializable]
    private struct Colliders
    {
        public Collider small;
        public Collider grown;
        public Collider distort;
    }
}
