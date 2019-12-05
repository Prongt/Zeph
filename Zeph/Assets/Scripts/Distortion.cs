using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Distortion : Aspects
{
    [SerializeField] private Animator disAnim1;
    [SerializeField] private bool useDynamicMeshCollider = false;

    private SkinnedMeshRenderer meshRenderer;
    private bool animating = false;

    [SerializeField] private bool onGround;
    
    // Start is called before the first frame update
    void Start()
    {
        meshRenderer = GetComponent<SkinnedMeshRenderer>();
        if (meshRenderer == null)
        {
            Debug.Log("No SkinnedMeshRenderer on " + gameObject.name + " id: " + gameObject.GetInstanceID());
        }
        
        //meshRenderer.b
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source,element);
        
        animating = !animating;
        
        if (disAnim1 != null)
        disAnim1.SetBool("Animate", animating);
    }
    
    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Not pushed
        //Debug.Log("Not being pushed");
    }
    
    
    public Type[] componentTypes = new Type[]
    {
        
    };

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
}
