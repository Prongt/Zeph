using System;
using System.Collections;
using FMODUnity;
using Movement;
using UnityEngine;

public class Orbitable : Aspects
{
    private Transform centerPoint;

    //VFX of the orbiting affect
    //[SerializeField] private VisualEffect orbitEffect;

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
    //private float rotIncrease = 10;

    //Objects rigidbody
    private Rigidbody myRb;
    private bool orbitDirection = true;

    //Bools controlling if the object orbits or is thrown
    private bool orbiting;

    [Header("Orbit Vars")] [SerializeField]
    public float orbitSize = 3;

    //Everything controlling orbit and throw values
    [Header("Pull/Push")] [SerializeField] private FloatReference pullForce = default;

    //Time the object takes to reach the desired position on the radius
    private readonly float radiusSpeed = 10f;
    [SerializeField] private float rotSpeed;
    private bool throwable;
    public float throwForce = 0.5f;


    private Transform zephTransform;

    private float timeSinceLastOrbiting;


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
        zephTransform = FindObjectOfType<PlayerMoveRigidbody>().transform;
        delayWaitForSeconds = new WaitForSeconds(0.3f);
        myRb = GetComponent<Rigidbody>();

        if (!gameObject.CompareTag("Log"))
        {
            if (firefly != null) fireflyRate = firefly.emission;
        }
        else
        {
            firefly = null;
        }

        //Orbit point is now created at runtime
        if (centerPoint == null)
        {
            centerPoint = new GameObject("Orbit Point").transform;
            var parent = GameObject.Find("Zeph_Animated");
            centerPoint.SetParent(parent.transform);
            centerPoint.localPosition = new Vector3(0, 0.5f, 0);
        }

        if (collisionSoundEventEmitter == null) collisionSoundEventEmitter = GetComponent<StudioEventEmitter>();

        timeSinceLastOrbiting = 50;
    }

    private void Update()
    {
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
        //Setting parent means the object does trail behind the player.
        gameObject.transform.SetParent(zephTransform);

        //Sake of ease constraints added
        myRb.constraints = RigidbodyConstraints.FreezeRotation;
        myRb.useGravity = false;

        //The orbiting code. Rotates around a point, gets a desired position, moves towards that desired position. forces the object to be on the right y level
        if (orbitDirection)
        {
            transform.RotateAround(centerPoint.position, Vector3.up, rotSpeed * Time.deltaTime);

            var desiredPosition = (transform.position - centerPoint.position).normalized * orbitSize +
                                  centerPoint.position;

            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);

            //Might not need this anymore
            transform.position = new Vector3(transform.position.x, centerPoint.position.y, transform.position.z);
        }
        else
        {
            transform.RotateAround(centerPoint.position, Vector3.up, -rotSpeed * Time.deltaTime);

            var desiredPosition = (transform.position - centerPoint.position).normalized * orbitSize +
                                  centerPoint.position;

            transform.position = Vector3.MoveTowards(transform.position, desiredPosition, Time.deltaTime * radiusSpeed);

            //Might not need this anymore
            transform.position = new Vector3(transform.position.x, centerPoint.position.y, transform.position.z);
        }

        //This speeds up the orbit
        if (throwForce <= maxThrowForce.Value) throwForce += 1 * Time.deltaTime;
    }

    private void Throw()
    {
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
        if (Vector3.Distance(centerPoint.position, transform.position) <= 3) orbiting = true;
        if (throwable) throwable = false;
    }


    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.CompareTag("Floor")) return;

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
}