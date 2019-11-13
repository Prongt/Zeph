using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Orbitable : Aspects
{
    [SerializeField] private FloatReference pullForce;
    [SerializeField] private FloatReference maxThrowForce;
    public float throwForce = 0.5f;
    private Vector3 direction;
    [SerializeField] public float orbitSize = 4;
    private float xSpread;
    private float zSpread;
    [SerializeField] private float yOffset;
    private Transform centerPoint;
    [SerializeField] private float rotSpeed;
    private Rigidbody myRB;
    

    private float timer;
    public bool orbiting;
    public bool throwable;

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
            throwable = true;
        }
        
        rotSpeed = throwForce;
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        //Being pushed
        //Debug.Log("Being pushed");
        //Initial pull to center
        centerPoint = source.transform;
        direction = source.transform.position - transform.position;
        myRB.AddForce(direction * pullForce.Value);

        //Checks to activate functions
        if (orbiting)
        {
            orbiting = false;
            //throwable = true;
        }

        if (throwable)
        {
            Throw();
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
        //This causes the object to orbit
        float x = -Mathf.Cos(timer) * xSpread;
        float z = Mathf.Sin(timer) * zSpread;
        Vector3 pos = new Vector3(x, yOffset, z);
        //transform.forward = centerPoint.transform.forward;
        transform.position = pos + centerPoint.position;

        //This speeds up the orbit
        if (throwForce <= maxThrowForce.Value)
        {
            throwForce += 1 * Time.deltaTime;
        }
    }

    void Throw()
    {
        //Throws object away from the player
        direction = centerPoint.position - transform.position;
        direction = -direction;
        myRB.AddForce(direction * throwForce, ForceMode.Impulse);
        StartCoroutine(Delay());
        throwForce = 0.5f;
    }

    IEnumerator Delay()
    {
        //Delay on checks to make things work smoother
        yield return new WaitForSeconds(0.3f);
        
        if (throwable)
        {
            throwable = false;
        }

        if (!gameObject.CompareTag("Heavy"))
        {
            if (Vector3.Distance(centerPoint.position, transform.position) <= 3)
            {
                orbiting = true;
            }
        }
    }
}