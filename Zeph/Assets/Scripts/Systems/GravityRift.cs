﻿
using System;
using UnityEngine;

public class GravityRift : Aspects
{
    [SerializeField] private Vector3 newGravity;
    public static Vector3 ogGravity = new Vector3(0, -9.81f, 0);

    public static bool useNewGravity = false;

    public bool resetGravity = true;
    // Start is called before the first frame update
    void Awake()
    {
        Debug.LogWarning("Hard setting gravity to default gravity");
        Physics.gravity = new Vector3(0, -9.81f, 0);
        ogGravity = Physics.gravity;
    }
    
    public Type[] componentTypes = new Type[]
    {
        
    };

    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    
    public override void Promote(Transform source = null, Element element = null)
    {
        //Debug.Log("Here " + gameObject.name);
        if (isCoolDownComplete == false)
        {
            Debug.Log("Cool down not complete on " + gameObject.name);
            return;
        }
        base.Promote(source, element);
        
        ChangeGravity();
    }
    
    private void ChangeGravity()
    {
        if (!resetGravity)
        {
            Physics.gravity = newGravity;
        }
        else
        {
            if (!useNewGravity)
            {
                useNewGravity = true;
                Physics.gravity = newGravity;
            }
            else
            {
                useNewGravity = false;
                Physics.gravity = ogGravity;
            }
        }
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Not pushed
        //Debug.Log("Not being pushed");
    }
    
        
}
