using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pushable : Aspects
{
    [SerializeField] private FloatReference pushForce;
    private Vector3 direction;
    
    public Type[] componentTypes = new Type[]
    {
        typeof(Rigidbody),
        typeof(AudioSource)
    };


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }


    public override void Promote(Transform source = null)
    {
        //Being pushed
        //Debug.Log("Being pushed");
        direction = source.transform.position - transform.position;
        direction = -direction;
        Rigidbody myRB = GetComponent<Rigidbody>();
        myRB.AddForce(direction * pushForce.Value);
    }

    public override void Negate(Transform source = null)
    {
        //Not pushed
        //Debug.Log("Not being pushed");
    }
}