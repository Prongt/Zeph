using System;
using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Movement
{
    public class PlayerMoveRigidbody : MonoBehaviour
    {
        [Header("Movement")] 
        [SerializeField] [Range(0f, 100f)] private float acceleration = 10f;
        [SerializeField] [Range(0f, 100f)] private float airAcceleration = 1f;
        [SerializeField] [Range(0, 90)] private float maxGroundAngle = 50f;
        [SerializeField] [Range(0f, 100f)] private float speed = 10f;
        [SerializeField] private bool swapMovementAxis;
        [SerializeField] private bool reverseX;
        [SerializeField] private bool reverseY;
        [SerializeField] private float inputLimit = 0.05f;
        [SerializeField] private float baseDrag = 0.5f;
        [SerializeField] private float newDrag = 5f;
        

        [Header("Jumping")] [SerializeField] [Range(0f, 10f)]
        private float jumpHeight = 2f;
        [SerializeField] [Range(0f, 1f)] private float coyoteTime = 0.125f;
        private float timeSinceLastJump;

        [Header("Gravity")] [SerializeField] [Range(0f, 5f)]
        private float gravityFlipTime = 2.0f;

        [Header("Rotation")] [SerializeField] private Transform zephModel;
        [SerializeField] [Range(0f, 5f)] private float rotationModifier = 1f;

        [Header("Animation")] [SerializeField] private Animator zephAnimator;
        [SerializeField] private string moveVariable = "moveSpeed";
        [SerializeField] private string jumpVariable = "IsJumping";
        private static readonly int isDancing = Animator.StringToHash("IsDancing");
        private WaitForSeconds danceWaitForSeconds;

        private float minGroundDotProduct;

        private Vector2 playerInput;
        private new Rigidbody rigidbody;

        private int groundContactCount;

        private Vector3 groundContactNormal;

        private bool hasScheduledJump;
        private Vector3 velocity;
        private Vector3 desiredVelocity;

        private bool OnGround => groundContactCount > 0;
        private bool ZGravity => currentGravity.z < 0 || currentGravity.z > 0;

        private Vector3 currentGravity;
        private Vector3 upVector;
        public static bool HaltMovement;
        


        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            zephAnimator = GetComponentInChildren<Animator>();
            HaltMovement = false;
        }

        private void Start()
        {
            HaltMovement = false;
            currentGravity = Physics.gravity;
            upVector = -currentGravity.normalized;
            minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);
        
            danceWaitForSeconds = new WaitForSeconds(8f);
        }


        private void Update()
        {
            GravitySwitching();
            ManageAnimation();
            HandleInput();

            if (ZGravity)
                desiredVelocity = new Vector3(playerInput.x, playerInput.y, 0f) * speed;
            else
                desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * speed;

            timeSinceLastJump += Time.deltaTime;
            
        }

        private void HandleInput()
        {
            playerInput.x = -Input.GetAxis("Horizontal");
            playerInput.y = -Input.GetAxis("Vertical");

            if (swapMovementAxis)
            {
                var tempInput = playerInput;
                playerInput.x = tempInput.y;
                playerInput.y = tempInput.x;
            }

            if (reverseX)
            {
                playerInput.x = -playerInput.x;
            }

            if (reverseY)
            {
                playerInput.y = -playerInput.y;
            }

            playerInput = Vector2.ClampMagnitude(playerInput, 1f);
            if (Input.GetButtonDown("Jump")) hasScheduledJump = true;

            //Debug.Log(playerInput.x);
        }

        private void GravitySwitching()
        {
            if (Physics.gravity == currentGravity) return;

            currentGravity = Physics.gravity;
            upVector = -currentGravity.normalized;

            StopCoroutine(LerpTransformUp());
            StartCoroutine(LerpTransformUp());
        }

        private IEnumerator LerpTransformUp()
        {
            HaltMovement = true;
            while (Math.Abs(transform.up.y - upVector.y) > 0.05f || Math.Abs(transform.up.x - upVector.x) > 0.05f ||
                   Math.Abs(transform.up.z - upVector.z) > 0.05f)
            {
                rigidbody.Sleep();
                transform.up = Vector3.Lerp(transform.up, upVector, gravityFlipTime * Time.deltaTime);
                yield return null;
            }

            //Snaps to new up vector just in case the lerp didnt finish
            transform.up = upVector;
            HaltMovement = false;
        }

        private void FixedUpdate()
        {
            if (HaltMovement) return;
            velocity = rigidbody.velocity;
            
            if (Math.Abs(playerInput.x) < inputLimit && Math.Abs(playerInput.y) < inputLimit && !hasScheduledJump)
            {
                rigidbody.drag = newDrag;
            }
            else
            {
                rigidbody.drag = baseDrag;
            }

            UpdateGroundContacts();
            AdjustVelocity();

            Jump();

            
            rigidbody.velocity = velocity;
            
            
            Rotate();
            ResetContactCounts();
        }
        
        

        public void FlipMovement()
        {
            reverseX = !reverseX;
            reverseY = !reverseY;
        }

        private void ManageAnimation()
        {
            zephAnimator.SetBool(jumpVariable, !OnGround);

            if (Input.GetKeyDown(KeyCode.M))
            {
                StopCoroutine(DanceRoutine());
                StartCoroutine(DanceRoutine());
            }

            zephAnimator.SetFloat(moveVariable, desiredVelocity.magnitude > 0.25f ? 1.0f : 0f);
        }

        private IEnumerator DanceRoutine()
        {
            zephAnimator.SetBool(isDancing, true);
            yield return danceWaitForSeconds;
            zephAnimator.SetBool(isDancing, false);
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

            var newZ = Mathf.MoveTowards(currentZ, ZGravity ? desiredVelocity.y : desiredVelocity.z, maxSpeedChange);

            velocity += xAxis * (newX - currentX) + zAxis * (newZ - currentZ);
        }

        private void Jump()
        {
            if (!hasScheduledJump) return;
            hasScheduledJump = false;

            Vector3 jumpDirection;
            
            if (OnGround || timeSinceLastJump < coyoteTime)
            {
                jumpDirection = groundContactNormal;
                timeSinceLastJump = 0;
            }
            else{
                return;
            }

            float jumpSpeed;
            if (currentGravity.z < 0)
                jumpSpeed = Mathf.Sqrt(-2f * currentGravity.z * jumpHeight);
            else if (currentGravity.z > 0)
                jumpSpeed = Mathf.Sqrt(-2f * -currentGravity.z * jumpHeight);
            else if (currentGravity.y > 0)
                jumpSpeed = Mathf.Sqrt(-2f * -currentGravity.y * jumpHeight);
            else
                jumpSpeed = Mathf.Sqrt(-2f * currentGravity.y * jumpHeight);


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
                    if (math.abs(normal.z) > minGroundDotProduct)
                    {
                        groundContactCount++;
                        groundContactNormal = normal;
                        //Debug.Log("Wall");
                        break;
                    }
                }
                else
                {
                    // if (math.abs(normal.y) > minGroundDotProduct)
                    // {
                    //     groundContactCount++;
                    //     groundContactNormal = normal;
                    //     break;
                    // }
                    
                    //TODO replace with math.abs
                    if (GravityRift.UseNewGravity)
                    {
                        if (normal.y <= minGroundDotProduct)
                        {
                            groundContactCount++;
                            groundContactNormal = normal;
                           //Debug.Log("Roof");
                            break;
                        }
                    }else if (normal.y >= minGroundDotProduct)
                    {
                        groundContactCount++;
                        groundContactNormal = normal;
                       //Debug.Log("Floor");
                        break;
                    }
                }
            }
        }

        private Vector3 ProjectOnContactPlane(Vector3 vector)
        {
            return vector - groundContactNormal * Vector3.Dot(vector, groundContactNormal);
        }

        public void ApplyKnockBackForce(Vector3 force, ForceMode forceMode)
        {
            rigidbody.AddForce(force, forceMode);
        }

        public void TeleportPlayer(Transform point)
        {
            rigidbody.MovePosition(point.position);
        }
    }
}
