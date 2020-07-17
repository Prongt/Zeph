using System;
using UnityEngine;

/// <summary>
/// When activated the animation for the rope bridge is triggered
/// </summary>
public class RopeBridge : Aspects
{
    private Animator myAnim;
    private readonly string raise = "Raise";
    
    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
    
    public Type[] componentTypes =
    {
      typeof(FireflyController)
    };
    
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        //print("RAISE");
        myAnim.SetBool(raise, true);
    }

    public override void Negate(Transform source = null)
    {
        base.Negate(source);
    }
}
