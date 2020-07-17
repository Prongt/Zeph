using System.Collections;
using UnityEngine;

/// <summary>
/// When activated the ivy bridge is grown half way and continues growing when a distortion event is activated
/// </summary>
public class VineBridge : Growable
{
    private Material mat;
    [SerializeField] private float matX;
    [SerializeField] private float finalValue;
    [SerializeField] private float finalDistortValue;
    [SerializeField] private float growSpeed = 5;
    [SerializeField] private float distortGrowSpeed = 5;

    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.VineBridge;
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
        
        GetComponentInChildren<FireflyController>().interacted = true;
        
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

            /*if (Animator.GetBool(distort) && matX < finalValue && matX > finalDistortValue)
            {
                matX -= distortGrowSpeed * Time.deltaTime;
            }*/
            
            if (Animator.GetBool(distort) && HasGrown)
            {
                print("I should be growing");
                matX = finalDistortValue;
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
            if (!Animator.GetBool(distort))
            {
                if (matX >= finalValue)
                {
                    matX -= growSpeed * Time.deltaTime;
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
                if (matX >= finalDistortValue)
                {
                    matX -= distortGrowSpeed * Time.deltaTime;
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
            if (matX >= finalValue)
            {
                matX -= growSpeed * Time.deltaTime;
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