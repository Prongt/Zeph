using System;
using UnityEngine;

/// <summary>
/// When activated a pillar fall animation is triggered 
/// </summary>
public class PillarPull : Aspects
{
    private Animator myAnim;
    public Collider myCol;
    
    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
    
    public Type[] componentTypes = new Type[]
    {
        typeof(Animator),
        typeof(FireflyController)
    };

    private FireflyController fireflyController;

    void Start()
    {
        fireflyController = GetComponentInChildren<FireflyController>();
        myAnim = GetComponent<Animator>();
        if (!gameObject.CompareTag("Heavy"))
        {
            myAnim.SetBool("Large", true);
        }
    }
    

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        
        fireflyController.interacted = true;
        
        if (myCol != null)
        {
            myCol.enabled = false;
        }
        Vector3 direction = gameObject.transform.position - source.position;
        var dot = Vector3.Dot(direction, gameObject.transform.right);
        if (gameObject.CompareTag("SnowPillar"))
        {
            dot = Vector3.Dot(direction, gameObject.transform.forward);
        }
        //print(dot);
        if (dot > 0)
        {
            myAnim.SetBool("Fall", true);
        }
    }

    public override void Negate(Transform source = null)
    {
        base.Negate(source);
    }
    
}
