using System;
using System.Collections;
using System.Collections.Generic;
using TreeEditor;
using UnityEngine;
using UnityEngine.AI;

public class Orbitable : Aspects
{
    [SerializeField] private FloatReference pullForce;
    [SerializeField] private FloatReference maxThrowForce;
    public float throwForce = 0.5f;
    
    private Vector3 direction;
    
    [SerializeField] public float orbitSize = 3;
    private float xSpread;
    private float zSpread;
    [SerializeField] private float yOffset = 0;
    private Transform centerPoint = null;
    [SerializeField] private float rotSpeed;
    private float rotIncrease = 10;
    
    private Rigidbody myRB;
    
    private bool orbitDirection = true;
    private bool delay = false;
    private bool canRotate = true;
    private float savedRotSpeed;
    private Vector3 savedTransform;
    
    private float timer;
    public bool orbiting;
    public bool throwable;

    public float radiusSpeed =  10f;
    

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

        //TODO find better solution
        centerPoint = GameObject.FindWithTag("OrbitPoint").transform;
//        print("Center Point: " + centerPoint.position);
    }

    void Update()
    {
        /*print("The Forward" + centerPoint.forward);
        print("The Direction" + direction);*/
        
        
        
    }

    void LateUpdate()
    {
        if (orbiting)
        {
            Orbit();
            throwable = true;
            delay = true;
        } 
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        //Being pushed
        //Debug.Log("Being pushed");
        //Initial pull to center
        //centerPoint = source.transform;
        direction = source.transform.position - transform.position;


        //Checks to activate functions
        if (orbiting)
        {
            delay = true;
            orbiting = false;
        }
        else
        {
            delay = false;
            myRB.AddForce(direction * pullForce.Value);
        }

        if (throwable)
        {
            Throw();
        }
        
        if (!delay)
        {
            StartCoroutine(Delay());
        }
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
        //Not pushed
        //Debug.Log("Not being pushed");
    }

    void Orbit()
    {
        myRB.constraints = RigidbodyConstraints.FreezeRotation;
        myRB.useGravity = false;
        
        if (orbitDirection)
        {
            if (rotSpeed <= throwForce * 10 && canRotate)
            {
                rotSpeed += rotIncrease * Time.deltaTime;
            }
        }
        else
        {
            if (rotSpeed <= -throwForce * 10 && canRotate)
            {
                rotSpeed -= rotIncrease * Time.deltaTime;
            } 
        }


        if (orbitDirection)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, rotSpeed * Time.deltaTime);
            var desiredPosition = (transform.position - centerPoint.position).normalized * orbitSize +
                                  centerPoint.position;
            //desiredPosition = new Vector3(desiredPosition.x, centerPoint.position.y, desiredPosition.z);
//            print("Desired Pos: " + desiredPosition);
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
            transform.position = new Vector3(transform.position.x, centerPoint.position.y, transform.position.z);
        }
        else
        {
            transform.RotateAround(centerPoint.position, Vector3.up, -rotSpeed * Time.deltaTime);
            var desiredPosition = (transform.position - centerPoint.position).normalized * orbitSize +
                                  centerPoint.position;
            //desiredPosition = new Vector3(desiredPosition.x, centerPoint.position.y, desiredPosition.z);
           // print("Desired Pos: " + desiredPosition);
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
            transform.position = new Vector3(transform.position.x, centerPoint.position.y, transform.position.z);
        }

        //This speeds up the orbit
        
        if (throwForce <= maxThrowForce.Value)
        {
            throwForce += 1 * Time.deltaTime;
        }
    }

    void Throw()
    {
        myRB.constraints = RigidbodyConstraints.None;
        myRB.useGravity = true;
        
        //Resets the rotation speed
        rotSpeed = 0;
        
        //Throws object away from the player
        direction = centerPoint.forward + transform.forward;
        myRB.AddForce(centerPoint.forward * throwForce, ForceMode.Impulse);
        throwForce = 0.5f;
        
    }

    IEnumerator Delay()
    {
        //Delay on checks to make things work smoother
        yield return new WaitForSeconds(0.3f);
        
        if (!gameObject.CompareTag("Heavy"))
        {
            if (Vector3.Distance(centerPoint.position, transform.position) <= 3)
            {
                orbiting = true;
            }
        }
        
        if (throwable)
        {
            throwable = false;
        }
    }
    

    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag("Floor"))
        {
            orbitDirection = !orbitDirection;
        }
    }
}