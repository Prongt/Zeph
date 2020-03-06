using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarPull : Aspects
{
    private Rigidbody myRB;
    private Vector3 direction;
    [SerializeField] private FloatReference pullForce;
    
    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }
    
    public Type[] componentTypes = new Type[]
    {
        typeof(Rigidbody),
    };
    
    void Start()
    {
        myRB = GetComponent<Rigidbody>();
    }
    

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        direction = source.transform.position - transform.position;
        
        myRB.AddForce(direction * pullForce.Value);
    }

    public override void Negate(Transform source = null)
    {
        base.Negate(source);
    }

    private void OnCollisionEnter(Collision other)
    {
        //This Part is for the pillar. If object is heavy and hits the floor it comes to a dead stop.
        if (gameObject.CompareTag("Heavy") && other.gameObject.CompareTag("Floor"))
        {
            myRB.constraints = RigidbodyConstraints.FreezeRotation;
            myRB.constraints = RigidbodyConstraints.FreezePosition;
        } 
    }

   
}
