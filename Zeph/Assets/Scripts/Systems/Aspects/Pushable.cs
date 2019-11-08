using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Pushable : Aspects
{
    [SerializeField] private FloatReference pushForce;
    private Vector3 direction;
    [SerializeField] public float orbitSize = 4;
    private float xSpread;
    private float zSpread;
    [SerializeField] private float yOffset;
    private Transform centerPoint;
    [SerializeField] private float rotSpeed;
    private Rigidbody myRB;
    

    private float timer;
    private bool orbiting;
    private bool throwable;

    public Type[] componentTypes = new Type[]
    {
        typeof(Rigidbody),
        typeof(AudioSource)
    };


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    void Start()
    {
        myRB = GetComponent<Rigidbody>();
        xSpread = orbitSize;
        zSpread = orbitSize;
    }

    void Update()
    {
        timer += Time.deltaTime * rotSpeed;
        
        if (orbiting)
        {
            Orbit();
        }

        if (throwable)
        {
            Throw();
        }
    }

    public override void Promote(Transform source = null)
    {
        base.Promote(source);
        //Being pushed
        //Debug.Log("Being pushed");
        centerPoint = source.transform;
        direction = source.transform.position - transform.position;
        myRB.AddForce(direction * pushForce.Value);

        if (orbiting)
        {
            orbiting = false;
            throwable = true;
        }
        
        StartCoroutine(Delay());
        
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Not pushed
        //Debug.Log("Not being pushed");
    }

    void Orbit()
    {
        if (!orbiting)
        {
            return;
        }
        
        float x = -Mathf.Cos(timer) * xSpread;
        float z = Mathf.Sin(timer) * zSpread;
        Vector3 pos = new Vector3(x, yOffset, z);
        transform.position = pos + centerPoint.position;
    }

    void Throw()
    {
        direction = centerPoint.position - transform.position;
        direction = -direction;
        myRB.AddForce(direction * pushForce.Value/20, ForceMode.Impulse);
        StartCoroutine(Delay());
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(0.3f);
        
        if (throwable)
        {
            throwable = false;
        }
        
        if (Vector3.Distance(centerPoint.position, transform.position) <= 3)
        {
            orbiting = true;
        }
    }
}