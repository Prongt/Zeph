using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class of for all aspects
/// </summary>
public abstract class Aspects : MonoBehaviour
{
    [HideInInspector] public AspectType AspectType;

    [SerializeField] private UnityEvent onPromote = default;
    [SerializeField] private UnityEvent onNegate = default;
    protected Element element;
    protected bool HasGrowSoundBeenPlayed = false;
    
    [SerializeField] private float coolDownTime = 0.0f;
    protected bool isCoolDownComplete = true;

    private WaitForSeconds coolDownWaitForSeconds;
    private void Awake()
    {
        Initialize();
    }

    protected virtual void Initialize()
    {
        // Enum.TryParse(GetType().Name, out AspectType aspectType);
        // //Debug.Log(aspectType);
        // AspectType = aspectType;
        // Debug.Log(AspectType);
        coolDownWaitForSeconds = new WaitForSeconds(coolDownTime);
    }

    public abstract Type[] RequiredComponents();



    public virtual void Promote(Transform source = null, Element element = null)
    {
        
        if (isCoolDownComplete == false)
        {
            //Debug.Log("return");
            return;
        }
        else
        {
            StartCoroutine(CoolDownCoroutine());
        }
        
        
        if (element != null)
        {
            this.element = element;
        }
        onPromote.Invoke();
    }

    IEnumerator CoolDownCoroutine()
    {
        isCoolDownComplete = false;
        
        //yield return new WaitForSeconds(coolDownTime);
        yield return coolDownWaitForSeconds;

        isCoolDownComplete = true;
    }


    public virtual void Negate(Transform source = null)
    {
        onNegate.Invoke();
    }
}


[Serializable]
public enum AspectType
{
    Flamable,
    Growable,
    Orbitable,
    GravityRift,
    Distortion,
    FadeRift,
    Story,
    Chargeable,
    Map,
    Pullable,
    VineBridge,
    GrowablePlant,
    GrowableTree,
    PillarPull,
    MiniBridgeGrow,
    RopeBridge
}