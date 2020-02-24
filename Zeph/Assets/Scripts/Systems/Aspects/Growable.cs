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


    [SerializeField] private bool isTree;
    [SerializeField] private Colliders colliders;

    [SerializeField] private StudioEventEmitter growSoundEmitter;

    private SkinnedMeshRenderer meshRenderer;
    private Mesh mesh;

    private bool hasGrown;

    [SerializeField] private ParticleSystem firefly;
    private ParticleSystem.EmissionModule fireflyRate;
    public bool StopFuckingWithMyShaderValues = false;

    private bool lightShining;


    public Type[] componentTypes = new Type[]
    {
    };

    private static readonly int vector1B0F27Ffd = Shader.PropertyToID("Vector1_B0F27FFD");
    private static readonly int distort = Animator.StringToHash("Distort");
    private static readonly int grow = Animator.StringToHash("Grow");
    private static readonly int growing = Animator.StringToHash("Growing");


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
            matX = mat.GetFloat(vector1B0F27Ffd);
        }

        if (isTree)
        {
            myAnim = GetComponent<Animator>();
            meshRenderer = GetComponent<SkinnedMeshRenderer>();

            colliders.small.isTrigger = false;
            colliders.distort.enabled = false;
            colliders.grown.enabled = false;
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
           if (myAnim.GetBool(distort) && myAnim.GetBool(grow))
           {
               colliders.small.isTrigger = true;
               colliders.distort.enabled = true;
               colliders.grown.enabled = false;
           }

           if (myAnim.GetBool(distort) == false && myAnim.GetBool(grow))
           {
               colliders.small.isTrigger = true;
               colliders.distort.enabled = false;
               colliders.grown.enabled = true;
           }

           if (Distortion.IsDistorting)
           {
               myAnim.SetBool(distort, true);
               colliders.small.isTrigger = true;
               colliders.distort.enabled = true;
               colliders.grown.enabled = false;
           }
           else
           {
               myAnim.SetBool(distort, false);
               colliders.small.isTrigger = true;
               colliders.distort.enabled = false;
               colliders.grown.enabled = true;
           }
       }


       if (gameObject.CompareTag("Bridge"))
       {
          // Debug.Log("Why you do this!");
           mat.SetFloat(vector1B0F27Ffd, matX);

           if (lightShining)
           {
               StartCoroutine(Appear());
           }
           
           if (Distortion.IsDistorting)
           {
               myAnim.SetBool(distort, true);
           }
           else
           {
               myAnim.SetBool(distort, false);
           }

           if (myAnim.GetBool(distort) && matX < 1 && matX > -13)
           {
               matX -= 5 * Time.deltaTime;
           }
       }
   }

   public override void Promote(Transform source = null, Element element = null)
   {
       base.Promote(source, element);
       //Debug.Log("Grow Bitch!");

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
           myAnim.SetBool(grow, true);
       }

       if (gameObject.CompareTag("Plant"))
       {
           myAnim.SetBool(growing, true);
           if (colliders.grown)
           {
               colliders.grown.enabled = true;
           }

           //gameObject.GetComponent<Renderer>().material = GrowingMaterial;
           if (gameObject.CompareTag("Bridge"))
           {
               if (matX >= 1)
               {
                   if (growSoundEmitter)
                   {
                       if (groundDistort)
                           if (groundDistort.GetBool(distort))
                           {
                               growSoundEmitter.SetParameter("Distortion", 1.0f);
                           }
                           else
                           {
                               growSoundEmitter.SetParameter("Distortion", 0.0f);
                           }
                   }

                   if (!StopFuckingWithMyShaderValues)
                   {
                       myAnim.SetBool(growing, true);
                       StartCoroutine(Appear());
                   }


               }
           }
       }
   }

   public override void Negate(Transform source = null)
    {
        //Stop growing
        //Debug.Log("Shrinking");
        base.Promote(source);
    }

//TODO implement a while loop rather than using recursive calls
    IEnumerator Appear()
    {
        yield return new WaitForSeconds(0f);
        if (groundDistort != null)
        {
            if (groundDistort.GetBool(distort) && hasGrown)
            {
                matX = -14;
            }

            if (!groundDistort.GetBool(distort))
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
            else if (groundDistort.GetBool(distort))
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
