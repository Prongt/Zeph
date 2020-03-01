using UnityEngine;

public class PlayerMoveRigidbody : MonoBehaviour
{
    private Rigidbody body;

    private Vector3 groundContactNormal, steepNormal;

    private int groundContactCount, steepContactCount;

    private bool hasScheduledJump;

    [SerializeField] [Range(0f, 10f)] private float jumpHeight = 2f;

    [SerializeField] [Range(0f, 100f)] private float maxAcceleration = 10f, maxAirAcceleration = 1f;

    [SerializeField] [Range(0, 90)] private float maxGroundAngle = 25f;

    [SerializeField] [Range(0f, 100f)] private float maxSpeed = 10f;


    private float minGroundDotProduct;

    private Vector2 playerInput;

    [SerializeField] [Range(0f, 5f)] private float rotationMultiplier = 1f;

    private Vector3 velocity, desiredVelocity;

    private bool OnGround => groundContactCount > 0;

    private void Awake()
    {
        body = GetComponent<Rigidbody>();
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }

    private void Update()
    {
        playerInput.x = -Input.GetAxis("Horizontal");
        playerInput.y = -Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        desiredVelocity =
            new Vector3(playerInput.x, 0f, playerInput.y) * maxSpeed;

        if (Input.GetButtonDown("Jump")) hasScheduledJump = true;
    }

    private void FixedUpdate()
    {
        UpdateGroundContacts();
        AdjustVelocity();

        if (hasScheduledJump)
        {
            hasScheduledJump = false;
            Jump();
        }

        body.velocity = velocity;
        Rotate();
        ResetContactCounts();
    }

    private void Rotate()
    {
        if ((playerInput.magnitude < 0.01f)) return;
        
        var targetRotation = Mathf.Atan2(playerInput.x, playerInput.y) * Mathf.Rad2Deg;
        var vel = body.velocity.magnitude;

        var localEulerAngles = transform.localEulerAngles;
        var angle = Mathf.SmoothDampAngle(localEulerAngles.y, targetRotation, ref vel,
            rotationMultiplier * Time.deltaTime);

        var rot = localEulerAngles;
        rot.y = angle;
        localEulerAngles = rot;
        transform.localEulerAngles = localEulerAngles;
    }

    private void ResetContactCounts()
    {
        groundContactCount = 0;
        steepContactCount = 0;
    }

    private void UpdateGroundContacts()
    {
        velocity = body.velocity;
        
        
        if (OnGround)
        {
            groundContactNormal.Normalize();
        }
        else
        {
            //TODO might be redundant
            groundContactNormal = Vector3.up;
        }
    }

    private void AdjustVelocity()
    {
        var xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        var zAxis = ProjectOnContactPlane(Vector3.forward).normalized;

        var currentX = Vector3.Dot(velocity, xAxis);
        var currentZ = Vector3.Dot(velocity, zAxis);

        var acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        var maxSpeedChange = acceleration * Time.deltaTime;

        var newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);
        var newZ =
            Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    private void Jump()
    {
        Vector3 jumpDirection;
        if (OnGround)
            jumpDirection = groundContactNormal;
        else
            return;

        var jumpSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * jumpHeight);
        jumpDirection = (jumpDirection + Vector3.up).normalized;
        var alignedSpeed = Vector3.Dot(velocity, jumpDirection);
        if (alignedSpeed > 0f) jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);
        velocity += jumpDirection * jumpSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        EvaluateCollision(collision);
    }

    private void EvaluateCollision(Collision collision)
    {
        for (var i = 0; i < collision.contactCount; i++)
        {
            var normal = collision.GetContact(i).normal;
            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount++;
                groundContactNormal = normal;
            }
            else if (normal.y > -0.01f)
            {
                steepContactCount++;
                steepNormal = normal;
            }
        }
    }

    private Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - groundContactNormal * Vector3.Dot(vector, groundContactNormal);
    }
}