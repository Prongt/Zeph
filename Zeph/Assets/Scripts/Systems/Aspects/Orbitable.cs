using System;
using System.Collections;
using FMODUnity;
using Movement;
using UnityEngine;

/// <summary>
/// When activated the object can orbit around the player and be thrown by the player
/// </summary>
public class Orbitable : Aspects
{
    public Transform centerPoint;

    [Header("Extras")] [SerializeField] private StudioEventEmitter collisionSoundEventEmitter;

    public Type[] componentTypes =
    {
        typeof(Rigidbody)
    };

    private bool delay;

    private WaitForSeconds delayWaitForSeconds;

    //Direction between object and source/Player
    private Vector3 direction;
    [SerializeField] private ParticleSystem firefly;
    private ParticleSystem.EmissionModule fireflyRate;

    [SerializeField] private FloatReference maxThrowForce = default;
    
    private Rigidbody myRb;
    private bool orbitDirection = true;

    //Bools controlling if the object orbits or is thrown
    private bool orbiting;
    public bool pullable = true;

    [Header("Orbit Vars")] [SerializeField]
    public float orbitSize = 3;

    //Everything controlling orbit and throw values
    [Header("Pull/Push")] [SerializeField] private FloatReference pullForce = default;

    //Time the object takes to reach the desired position on the radius
    public float radiusSpeed = 10f;
    [SerializeField] private float rotSpeed;
    private bool throwable;
    public float throwForce = 0.5f;


    private Transform zephTransform;

    private float timeSinceLastOrbiting;

    private Vector3 relativeDistance = Vector3.zero;


    public override Type[] RequiredComponents()
    {
        return componentTypes;
    }

    protected override void Initialize()
    {
        base.Initialize();
        AspectType = AspectType.Orbitable;
    }

    private void Start()
    {
        //Finds player and checks for fireflies on current object
        zephTransform = GameObject.FindWithTag("Player").transform;
        delayWaitForSeconds = new WaitForSeconds(0.2f);
        myRb = GetComponent<Rigidbody>();

        /*if (!gameObject.CompareTag("Log"))
        {
            if (firefly != null) fireflyRate = firefly.emission;
        }
        else
        {
            firefly = null;
        }*/

        //Orbit point is now created at runtime
        //TODO try find orbit point after creation to stop duplicates
        if (centerPoint == null)
        {
            centerPoint = new GameObject("Orbit Point").transform;
            var parent = PlayerMoveRigidbody.orbitPoint;
            centerPoint.SetParent(parent.transform);
            centerPoint.localPosition = new Vector3(0, 1.5f, 0);
        }

        if (collisionSoundEventEmitter == null) collisionSoundEventEmitter = GetComponent<StudioEventEmitter>();

        timeSinceLastOrbiting = 50;
    }

    private void Update()
    {
        //Increases in the rotation speed in both directions
        if (rotSpeed <= throwForce * 10) rotSpeed = throwForce * 10;
        if (rotSpeed <= -throwForce * 10) rotSpeed = throwForce * 10;

        if (orbiting)
        {
            timeSinceLastOrbiting = 0;
        }
        else
        {
            timeSinceLastOrbiting += Time.deltaTime;
        }
    }

    //Orbit function called on late update to allow time for the player to move first
    private void LateUpdate()
    {
        if (!orbiting) return;

        Orbit();
        throwable = true;
        delay = true;
    }

    public override void Promote(Transform source = null, Element element = null)
    {
        base.Promote(source, element);
        //Initial pull to center
        direction = source.transform.position - transform.position;
        relativeDistance = transform.position - centerPoint.position;

        if (firefly != null) fireflyRate.rateOverTime = 0;

        //Checks to activate functions
        if (orbiting)
        {
            delay = true;

            orbiting = false;
        }
        else
        {
            delay = false;
            myRb.AddForce(direction * pullForce.Value);
        }

        if (throwable) Throw();

        if (!delay) StartCoroutine(Delay());
    }

    public override void Negate(Transform source = null)
    {
        base.Promote(source);
    }

    private void Orbit()
    {
        //Physics.IgnoreLayerCollision(9,18,true);

        //Setting parent means the object does trail behind the player.
        gameObject.transform.SetParent(zephTransform);

        //Sake of ease constraints added
        myRb.constraints = RigidbodyConstraints.FreezeRotation;
        myRb.useGravity = false;

        //Gets a relative distance to keep the orbiting object at.
        //Rotates around the centerpoint created earlier at the defined speed.
        //Forces the object to rotate at a higher y-value level.
        //Updates the relative position.
        if (orbitDirection)
        {
            transform.position = centerPoint.position + relativeDistance;
            transform.RotateAround(centerPoint.position, Vector3.up,rotSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, centerPoint.position.y, transform.position.z);
            relativeDistance = transform.position - centerPoint.position;
        }
        else
        {
            transform.position = centerPoint.position + relativeDistance;
            transform.RotateAround(centerPoint.position, Vector3.up, -rotSpeed * Time.deltaTime);
            transform.position = new Vector3(transform.position.x, centerPoint.position.y, transform.position.z);
            relativeDistance = transform.position - centerPoint.position;
        }

        //This speeds up the orbit
        if (throwForce <= maxThrowForce.Value) throwForce += 1 * Time.deltaTime;

        //Ensures object is thrown if ends up orbiting out of promotion range
        if (Input.GetButton("OrbitPower"))
        {
            Throw();
        }
    }

    private void Throw()
    {
        //Unsets any previously set constraints
        orbiting = false;
        transform.parent = null;

        myRb.constraints = RigidbodyConstraints.None;
        myRb.useGravity = true;

        //Resets the rotation speed
        rotSpeed = 0;

        //Throws object away from the player
        direction = centerPoint.forward + transform.forward;
        myRb.AddForce(centerPoint.forward * throwForce, ForceMode.Impulse);
        throwForce = 0.5f;
    }

    private IEnumerator Delay()
    {
        //Delay on checks to make things work smoother
        yield return delayWaitForSeconds;
        if (Vector3.Distance(centerPoint.position, transform.position) <= 3 && pullable)
        {
            orbiting = true;
        }

        if (throwable)
        {
            throwable = false;
        }
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pullable = false;
        }
        
        if (other.gameObject.CompareTag("Floor")) return;

        //switches which way the object is orbiting
        orbitDirection = !orbitDirection;

        if (orbiting)
        {
            PlayCollisionSound();
        }
        else
        {
            if (!other.collider.CompareTag("Player") && timeSinceLastOrbiting < 3f)
            {
                PlayCollisionSound();
            }
        }
        
    }

    private void PlayCollisionSound()
    {
        if (collisionSoundEventEmitter == null) return;

        if (collisionSoundEventEmitter.IsPlaying()) collisionSoundEventEmitter.Stop();
        collisionSoundEventEmitter.Play();
    }

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pullable = false;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            pullable = true;
        }
    }
}