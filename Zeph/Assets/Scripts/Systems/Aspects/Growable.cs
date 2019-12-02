using System;
using System.Collections;
using System.Runtime.CompilerServices;
using FMODUnity;
using UnityEngine;

public class Growable : Aspects
{
    [SerializeField] private Material GrowingMaterial;
    [SerializeField] private GameObject GrowingParticleEffect;
    public Animator myAnim;
    [SerializeField] private Material mat;
    private float matX;
    [SerializeField] private Animator groundDistort;
    [SerializeField] private GameObject distortBridge;

    
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
        if (gameObject.CompareTag("Plant"))
        {
            myAnim = GetComponent<Animator>();
        }

        if (gameObject.CompareTag("Bridge"))
        {
            mat = gameObject.GetComponent<MeshRenderer>().material;
            matX = mat.GetFloat("Vector1_D0BABF75");
        }
    }

   void Update()
   {
       if (gameObject.CompareTag("Bridge"))
       {
           mat.SetFloat("Vector1_D0BABF75", matX);
           
           if (!groundDistort.GetBool("Animate") && matX <= 1)
           {
               distortBridge.SetActive(true);
               gameObject.SetActive(false);
           } else if (groundDistort.GetBool("Animate") && matX <= -13)
           {
               distortBridge.SetActive(true);
               gameObject.SetActive(false);
           }
       }
   }
    
    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        print("IM GROWING");
        if (gameObject.CompareTag("Plant"))
        {
            myAnim.SetBool("Growing", true);
        }
        //gameObject.GetComponent<Renderer>().material = GrowingMaterial;
        if (gameObject.CompareTag("Bridge"))
        {
            if (matX >= 1)
            {
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
        yield return new WaitForSeconds(0.1f);
        if (!groundDistort.GetBool("Animate"))
        {
            if (matX >= 1)
            {
                matX -= 15 * Time.deltaTime;
                StartCoroutine(Appear());
            }
            else
            {
                gameObject.GetComponent<BoxCollider>().center = new Vector3(-6.721177f,-3.552706e-16f, 0.1f);
                gameObject.GetComponent<BoxCollider>().size = new Vector3(18.35765f,3,0.2f);
                gameObject.GetComponent<BoxCollider>().isTrigger = false;
                StopCoroutine(Appear());
            }
        } else if (groundDistort.GetBool("Animate"))
        {
            if (matX >= -13)
            {
                matX -= 15 * Time.deltaTime;
                StartCoroutine(Appear());
            }
            else
            {
                gameObject.GetComponent<BoxCollider>().center = new Vector3(0,2.384186e-08f, 0.1f);
                gameObject.GetComponent<BoxCollider>().size = new Vector3(31.76197f,3,0.2f);
                gameObject.GetComponent<BoxCollider>().isTrigger = false;
                StopCoroutine(Appear());
            }
        }
    }
}