using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VineBridge : Growable
{
    private Material mat;
    [SerializeField] private float matX;

    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.VineBridge;
        //Debug.Log("Vine");
    }

    void Start()
    {
        //Finding the inital value for the Material is the issue
        mat = Renderer.material;
        matX = mat.GetFloat("Vector1_B0F27FFD");
        //Setting it is a temp fix for build for Fleadh
        matX = 15;
    }

    
    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        
        BridgePromote();
    }
    
    private void Update()
    {
        // Debug.Log("Why you do this!");
            mat.SetFloat("Vector1_B0F27FFD", matX);

            if (LightShining)
            {
                StartCoroutine(BridgeAppear());
            }

            if (Distortion.IsDistorting)
            {
                Animator.SetBool(distort, true);
            }
            else
            {
                Animator.SetBool(distort, false);
            }

            if (Animator.GetBool(distort) && matX < 1 && matX > -13)
            {
                matX -= 5 * Time.deltaTime;
            }
        
    }
    
    
    
    private void BridgePromote()
    {
        if (matX >= 1)
            {
                if (growSoundEmitter)
                {
                    if (Animator)
                        if (Animator.GetBool(distort))
                        {
                            growSoundEmitter.SetParameter("Distortion", 1.0f);
                        }
                        else
                        {
                            growSoundEmitter.SetParameter("Distortion", 0.0f);
                        }
                }

                Animator.SetBool(growing, true);
                StartCoroutine(BridgeAppear());
            }
    }
    
    //TODO implement a while loop rather than using recursive calls
    IEnumerator BridgeAppear()
    {
        yield return new WaitForSeconds(0f);
        if (Animator != null)
        {
            if (Animator.GetBool(distort) && HasGrown)
            {
                matX = -14;
            }

            if (!Animator.GetBool(distort))
            {
                if (matX >= 1)
                {
                    matX -= 5 * Time.deltaTime;
                    StartCoroutine(BridgeAppear());
                }
                else
                {
                    HasGrown = true;

                    StopCoroutine(BridgeAppear());
                }
            }
            else if (Animator.GetBool(distort))
            {
                if (matX >= -13)
                {
                    matX -= 5 * Time.deltaTime;
                    StartCoroutine(BridgeAppear());
                }
                else
                {
                    HasGrown = true;
                    StopCoroutine(BridgeAppear());
                }
            }
        }
        else
        {
            if (matX >= 1)
            {
                matX -= 5 * Time.deltaTime;
                StartCoroutine(BridgeAppear());
            }
            else
            {
                HasGrown = true;
                StopCoroutine(BridgeAppear());
            }
        }
    }

}