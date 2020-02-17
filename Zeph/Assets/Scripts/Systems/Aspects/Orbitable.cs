using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.Experimental.VFX;

public class Orbitable : Aspects
{
    //Direction between object and source/Player
    private Vector3 direction;
    
    //Everything controlling orbit and throw values
    [Header("Pull/Push")]
    [SerializeField] private FloatReference pullForce;
    [SerializeField] private FloatReference maxThrowForce;
    public float throwForce = 0.5f;
    private Transform throwParent;
    
    [Header("Orbit Vars")]
    [SerializeField] public float orbitSize = 3;
    private bool orbitDirection = true;
    private Transform centerPoint = null;
    [SerializeField] private float rotSpeed;
    private float rotIncrease = 10;
    
    //Objects rigidbody
    private Rigidbody myRB;

    private bool delay = false;

    //Bools controlling if the object orbits or is thrown
    private bool orbiting;
    private bool throwable;

    //Time the object takes to reach the desired position on the radius
    private float radiusSpeed =  10f;
    
    //VFX of the orbiting affect
    //[SerializeField] private VisualEffect orbitEffect;
    
    [Header("Extras")]
    [SerializeField] private StudioEventEmitter collisionSoundEventEmitter;
    [SerializeField] private ParticleSystem firefly;
    private ParticleSystem.EmissionModule fireflyRate;
    

    public Type[] componentTypes = new Type[]
    {
        typeof(Rigidbody),
    };


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    void Start()
    {
        myRB = GetComponent<Rigidbody>();

        if (!gameObject.CompareTag("Log"))
        {
            if (firefly != null)
            {
                fireflyRate = firefly.emission;
            }
        }
        else
        {
            firefly = null;
        }

        //TODO find better solution
        centerPoint = GameObject.FindWithTag("OrbitPoint").transform;
        
        if (collisionSoundEventEmitter == null)
        {
            collisionSoundEventEmitter = GetComponent<StudioEventEmitter>();
        }

        if (!GameObject.Find("Throw Parents"))
        {
            GameObject parent = new GameObject("Throw Parents");
        }

        throwParent = GameObject.Find("Throw Parents").transform;
    }

    void Update()
    {
        if (rotSpeed <= throwForce * 10)
        {
            rotSpeed = throwForce * 10;
        }
        if (rotSpeed <= -throwForce * 10)
        {
            rotSpeed = throwForce * 10;
        } 
        
    }

    //Orbit function called on late update to allow time for the player to move first
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
        //Initial pull to center
        direction = source.transform.position - transform.position;

        if (firefly != null)
        {
            fireflyRate.rateOverTime = 0;
        }
        
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
    }

    void Orbit()
    {
        //Setting parent means the object does trail behind the player.
        gameObject.transform.SetParent(GameObject.Find("Zeph").transform);
        
        //Sake of ease constraints added
        myRB.constraints = RigidbodyConstraints.FreezeRotation;
        myRB.useGravity = false;
        
        //The orbiting code. Rotates around a point, gets a desired position, moves towards that desired position. forces the object to be on the right y level
        if (orbitDirection)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, rotSpeed * Time.deltaTime);
            
            var desiredPosition = (transform.position - centerPoint.position).normalized * orbitSize + centerPoint.position;
            
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
            
            //Might not need this anymore
            //transform.position = new Vector3(transform.position.x, centerPoint.position.y, transform.position.z);
        }
        else
        {
            transform.RotateAround(centerPoint.position, Vector3.up, -rotSpeed * Time.deltaTime);
            
            var desiredPosition = (transform.position - centerPoint.position).normalized * orbitSize + centerPoint.position;
            
            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);
            
            //Might not need this anymore
            //transform.position = new Vector3(transform.position.x, centerPoint.position.y, transform.position.z);
        }

        //This speeds up the orbit
        if (throwForce <= maxThrowForce.Value)
        {
            throwForce += 1 * Time.deltaTime;
        }
    }

    void Throw()
    {
        if (throwParent)
        {
            gameObject.transform.SetParent(throwParent);
        }
        
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

            if (orbiting)
            {
                if (collisionSoundEventEmitter != null)
                {
                    if (collisionSoundEventEmitter.IsPlaying())
                    {
                        collisionSoundEventEmitter.Stop();
                    }
                    collisionSoundEventEmitter.Play();
                }
            }
        }

        
        //This Part is for the pillar. If object is heavy and hits the floor it comes to a dead stop.
        if (gameObject.CompareTag("Heavy") && other.gameObject.CompareTag("Floor"))
        {
            myRB.constraints = RigidbodyConstraints.FreezeRotation;
            myRB.constraints = RigidbodyConstraints.FreezePosition;
        }
    }
}