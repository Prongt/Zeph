using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Growable : Aspects
{
    [SerializeField] private Material GrowingMaterial;
    [SerializeField] private GameObject GrowingParticleEffect;
    private Animator myAnim;

    
    public Type[] componentTypes = new Type[]
    {
        typeof(AudioSource),
        typeof(Animator)
    };

    
    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

   protected override void Initialize()
    {
        base.Initialize();
        myAnim = GetComponent<Animator>();
    }
    
    public override void Promote(Transform source = null)
    {
        base.Promote(source);
        myAnim.SetBool("Growing", true);
        //gameObject.GetComponent<Renderer>().material = GrowingMaterial;
    }

    public override void Negate(Transform source = null)
    {
        //Stop growing
        //Debug.Log("Shrinking");
        base.Promote(source);
    }
}