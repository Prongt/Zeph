using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FMODUnity;
using UnityEngine;

public class Growable : Aspects
{
    //[SerializeField] private Material GrowingMaterial;
    //[SerializeField] private GameObject GrowingParticleEffect;
    public Animator myAnim;
    [SerializeField] private Material mat;
    private float matX;
    [SerializeField] private Animator groundDistort;
    //[SerializeField] private GameObject distortBridge;


    [SerializeField] private bool isTree = false;
    [SerializeField] private Colliders colliders;

    [SerializeField] private StudioEventEmitter growSoundEmitter;

    private SkinnedMeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private Mesh mesh;

    private bool hasGrown = false;

    [SerializeField] private ParticleSystem firefly;
    private ParticleSystem.EmissionModule fireflyRate;


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
            meshCollider = GetComponent<MeshCollider>();

            colliders.small.isTrigger = false;
            colliders.distort.SetActive(false);
            colliders.grown.SetActive(false);

            if (meshRenderer == null)
            {
                Debug.Log("No SkinnedMeshRenderer on " + gameObject.name + " id: " + gameObject.GetInstanceID());
            }
        }
    }

   void Update()
   {
       if (isTree)
       {
           if (myAnim.GetBool("Distort") && myAnim.GetBool("Grow"))
           {
               //meshCollider.sharedMesh = colliders.distort;
               colliders.small.isTrigger = true;
               colliders.distort.SetActive(true);
               colliders.grown.SetActive(false);
           }

           if (myAnim.GetBool("Distort") == false && myAnim.GetBool("Grow"))
           {
               //meshCollider.sharedMesh = colliders.grown;
               colliders.small.isTrigger = true;
               colliders.distort.SetActive(false);
               colliders.grown.SetActive(true);
           }

           if (Distortion.isDistorting)
           {
               myAnim.SetBool("Distort", true);
           }
           else
           {
               myAnim.SetBool("Distort", false);
           }

//           if (useDynamicMeshCollider)
//           {
//               //meshRenderer.sharedMesh.MarkDynamic();
//               meshRenderer.BakeMesh(meshRenderer.sharedMesh);
//               meshCollider.sharedMesh = meshRenderer.sharedMesh;
//
//           }
       }


       if (gameObject.CompareTag("Bridge"))
       {
           mat.SetFloat("Vector1_B0F27FFD", matX);

           if (!groundDistort.GetBool("Distort") && matX <= 1)
           {
               //distortBridge.SetActive(true);
               //gameObject.SetActive(false);
           } else if (groundDistort.GetBool("Distort") && matX <= -13)
           {
               //distortBridge.SetActive(true);
               //gameObject.SetActive(false);
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
//        print("IM GROWING");
        fireflyRate.rateOverTime = 0;

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
                /*gameObject.GetComponent<BoxCollider>().center = new Vector3(-6.721177f,-3.552706e-16f, 0.1f);
                gameObject.GetComponent<BoxCollider>().size = new Vector3(18.35765f,3,0.2f);
                gameObject.GetComponent<BoxCollider>().isTrigger = false;*/
                StopCoroutine(Appear());
            }
        } else if (groundDistort.GetBool("Distort"))
        {
            if (matX >= -13)
            {
                matX -= 5 * Time.deltaTime;
                StartCoroutine(Appear());
            }
            else
            {
                hasGrown = true;
                /*gameObject.GetComponent<BoxCollider>().center = new Vector3(0,2.384186e-08f, 0.1f);
                gameObject.GetComponent<BoxCollider>().size = new Vector3(31.76197f,3,0.2f);
                gameObject.GetComponent<BoxCollider>().isTrigger = false;*/
                StopCoroutine(Appear());
            }
        }
    }

    [Serializable]
    private struct Colliders
    {
        public Collider small;
        public GameObject grown;
        public GameObject distort;
    }
}
