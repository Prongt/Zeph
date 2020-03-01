using UnityEngine;

public class PlayerMoveRigidbody : MonoBehaviour
{
    private new Rigidbody rigidbody;

    private int groundContactCount;

    private Vector3 groundContactNormal;

    private bool hasScheduledJump;

    [SerializeField] [Range(0f, 10f)] private float jumpHeight = 2f;

    [SerializeField] [Range(0f, 100f)] private float acceleration = 10f;
    [SerializeField] [Range(0f, 100f)] private float airAcceleration = 1f;

    [SerializeField] [Range(0, 90)] private float maxGroundAngle = 25f;

    [SerializeField] [Range(0f, 100f)] private float speed = 10f;
    
    private float minGroundDotProduct;

    private Vector2 playerInput;

    [Header("Rotation")] [SerializeField] private Transform zephModel;
    [SerializeField] [Range(0f, 5f)] private float rotationModifier = 1f;

    private Vector3 velocity;
    private Vector3 desiredVelocity;

    private bool OnGround => groundContactCount > 0;
    private bool ZGravity => currentGravity.z < 0 || currentGravity.z > 0;
    private bool YGravity => currentGravity.y < 0 || currentGravity.y > 0;
    
    private Vector3 currentGravity;
    private Vector3 upVector;
    private bool haltMovement;
    

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        haltMovement = false;
        currentGravity = Physics.gravity;
        upVector = -currentGravity.normalized;
        
        minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
    }


    private void Update()
    {
        GravitySwitching();
        
        
        playerInput.x = -Input.GetAxis("Horizontal");
        playerInput.y = -Input.GetAxis("Vertical");
        playerInput = Vector2.ClampMagnitude(playerInput, 1f);

        if (ZGravity)
        {
            desiredVelocity =
                new Vector3(playerInput.x, playerInput.y, 0f) * speed;
        }
        else
        {
            desiredVelocity =
                new Vector3(playerInput.x, 0f, playerInput.y) * speed;
        }
        

        if (Input.GetButtonDown("Jump")) hasScheduledJump = true;
    }

    private void GravitySwitching()
    {
        if (Physics.gravity != currentGravity)
        {
            currentGravity = Physics.gravity;
            upVector = -currentGravity.normalized;
            haltMovement = true;
            transform.up = upVector;
            rigidbody.AddForce(upVector, ForceMode.Impulse);
            haltMovement = false;
        }
    }

    private void FixedUpdate()
    {
        if (haltMovement) return;
        
        velocity = rigidbody.velocity;
        UpdateGroundContacts();
        AdjustVelocity();

        if (hasScheduledJump)
        {
            hasScheduledJump = false;
            Jump();
        }

        rigidbody.velocity = velocity;
        Rotate();
        ResetContactCounts();
    }

    private void Rotate()
    {
        if (playerInput.magnitude < 0.01f) return;

        var targetRotation = Mathf.Atan2(playerInput.x, playerInput.y) * Mathf.Rad2Deg;
        var vel = rigidbody.velocity.magnitude;

        var localEulerAngles = zephModel.localEulerAngles;
        var angle = Mathf.SmoothDampAngle(localEulerAngles.y, targetRotation, ref vel,
            rotationModifier * Time.deltaTime);

        var rot = localEulerAngles;
        rot.y = angle;
        localEulerAngles = rot;
        zephModel.localEulerAngles = localEulerAngles;
    }

    private void ResetContactCounts()
    {
        groundContactCount = 0;
    }

    private void UpdateGroundContacts()
    {
        if (OnGround)
            groundContactNormal.Normalize();
        else
            //TODO might be redundant
            groundContactNormal = upVector;
    }

    private void AdjustVelocity()
    {
        var xAxis = ProjectOnContactPlane(transform.right).normalized;

        var zAxis = ProjectOnContactPlane(transform.forward).normalized;

        var currentX = Vector3.Dot(velocity, xAxis);
        var currentZ = Vector3.Dot(velocity, zAxis);

        var localAcceleration = OnGround ? acceleration : airAcceleration;
        var maxSpeedChange = localAcceleration * Time.deltaTime;

        var newX =
            Mathf.MoveTowards(currentX, desiredVelocity.x, maxSpeedChange);

        float newZ;
        if (ZGravity)
        {
             newZ = Mathf.MoveTowards(currentZ, desiredVelocity.y, maxSpeedChange);
        }
        else
        {
             newZ = Mathf.MoveTowards(currentZ, desiredVelocity.z, maxSpeedChange);
        }
        

        velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
    }

    private void Jump()
    {
        Vector3 jumpDirection;
        if (OnGround)
            jumpDirection = groundContactNormal;
        else
            return;
        
        
        
        float jumpSpeed = 0;
        if (currentGravity.z < 0)
        {
            jumpSpeed = Mathf.Sqrt(-2f * currentGravity.z * jumpHeight);
        }
        else if (currentGravity.z > 0)
        {
            jumpSpeed = Mathf.Sqrt(-2f * -currentGravity.z * jumpHeight);
        }else if (currentGravity.y > 0)
        {
            jumpSpeed = Mathf.Sqrt(-2f * -currentGravity.y * jumpHeight);
        }
        else
        {
            jumpSpeed = Mathf.Sqrt(-2f * currentGravity.y * jumpHeight);
        }
        
        
        jumpDirection = (jumpDirection + upVector).normalized;
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
            
            
            if (ZGravity)
            {
                if (normal.z >= minGroundDotProduct)
                {
                    groundContactCount++;
                    groundContactNormal = normal;
                    continue;
                }
            }

            if (GravityRift.useNewGravity)
            {
                if (normal.y <= minGroundDotProduct)
                {
                    groundContactCount++;
                    groundContactNormal = normal;
                    continue;
                }
            }
            
            if (normal.y >= minGroundDotProduct)
            {
                groundContactCount++;
                groundContactNormal = normal;
            }
        }
    }

    private Vector3 ProjectOnContactPlane(Vector3 vector)
    {
        return vector - groundContactNormal * Vector3.Dot(vector, groundContactNormal);
    }
}