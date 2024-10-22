﻿using System.Collections;
using Unity.Mathematics;
using UnityEngine;

namespace Movement
{
    /// <summary>
    /// Player movement class that supports phyics and multiple directions of gravity
    /// </summary>
    public class PlayerMoveRigidbody : MonoBehaviour
    {
        private static readonly int isDancing = Animator.StringToHash("IsDancing");
        public static bool HaltMovement;
        public bool StopMovementAndPhysics = false;
        
        [Header("Movement")] [SerializeField] [Range(0f, 100f)]
        private float acceleration = 100f;

        [SerializeField] [Range(0f, 100f)] private float airAcceleration = 100f;

        private Vector3 currentGravity;
        private readonly WaitForSeconds danceWaitForSeconds = new WaitForSeconds(8f);
        private Vector3 desiredVelocity;
        [Range(0f, 100f)] [SerializeField] private float dragWhileMoving = 0.01f;
        [Range(0f, 200f)] [SerializeField] private float dragWhileStopped = 100f;
        [SerializeField] [Range(0, 90)] private float maxGroundAngle = 50f;
        private float minGroundDotProduct;

        [Header("Gravity")] [SerializeField] [Range(0f, 5f)]
        private float gravityFlipTime = 2.0f;

        private int groundContactCount;

        private Vector3 groundContactNormal;

        private bool hasScheduledJump;
        


        [Header("Jumping")] [SerializeField] [Range(0f, 10f)]
        private float jumpHeight = 1.3f;
        [SerializeField] [Range(0f, 1f)] private float coyoteTime = 0.125f;
        private float timeSinceGrounded;
        private bool isJumping = false;
        
        

        private Vector2 playerInput;
        [Header("Input")]
        [Range(0f, 1f)] [SerializeField] private float inputLimit = 0.125f;
        [SerializeField] private bool reverseX = false;
        [SerializeField] private bool reverseY = false;
        [SerializeField] private bool swapMovementAxis = false;
        
        private Vector3 upVector;
        private Vector3 velocity;
        private new Rigidbody rigidbody;

        [Header("Animation")] 
        [SerializeField] private Animator zephAnimator;
        [SerializeField] private string moveVariable = "moveSpeed";
        [SerializeField] private string jumpVariable = "IsJumping";

        [Header("Rotation")] 
        [SerializeField] private Transform zephModel = default;
        [SerializeField] [Range(0f, 5f)] private float rotationModifier = 1f;
        [SerializeField] [Range(0f, 100f)] private float speed = 5f;
        
        [Header("Particles")] 
        [SerializeField] private ParticleSystem jumpParticles;
        private ParticleSystem.EmissionModule jumpEmmision;
        private ParticleSystem.MinMaxCurve jumpEmmisionRate;
        
        [SerializeField] private ParticleSystem forwardParticles;
        private ParticleSystem.EmissionModule moveForwardEmmision;
        private ParticleSystem.MinMaxCurve moveForwardEmmisionRate;
        

        private bool OnGround => groundContactCount > 0;
        private bool ZGravity => currentGravity.z < 0 || currentGravity.z > 0;

        private bool reduceDrag;
        private readonly WaitForSeconds reduceDragWaitForSeconds = new WaitForSeconds(0.25f);

        public static Transform orbitPoint;
        private void Awake()
        {
            rigidbody = GetComponent<Rigidbody>();
            //zephAnimator = GetComponentInChildren<Animator>();
            HaltMovement = false;

            orbitPoint = zephModel;

            if (jumpParticles != null)
            {
                jumpEmmision = jumpParticles.emission;
                jumpEmmisionRate = jumpEmmision.rateOverTime;
            }

            if (forwardParticles != null)
            {
                moveForwardEmmision = forwardParticles.emission;
                moveForwardEmmisionRate = moveForwardEmmision.rateOverTime;
            }
        }

        private void Start()
        {
            HaltMovement = false;
            currentGravity = Physics.gravity;
            upVector = -currentGravity.normalized;
            minGroundDotProduct = Mathf.Cos(maxGroundAngle * Mathf.Deg2Rad);

            StartCoroutine(MovementDrag());
        }


        private void Update()
        {
            //TODO Remove hard coded values
            if (StopMovementAndPhysics)
            {
                HaltMovement = true;
                rigidbody.Sleep();
                
                //sets ground contacts to 2 so the falling/jump animations do not play
                groundContactCount = 2;
                hasScheduledJump = false;
            }
            else
            {
                HaltMovement = false;
            }
            
            StopMovementAndPhysics = HaltMovement;
            if (StopMovementAndPhysics || HaltMovement) return;
        
        
        
            GravitySwitching();
            ManageAnimation();
            HandlePlayerInput();

            if (ZGravity)
                desiredVelocity = new Vector3(playerInput.x, playerInput.y, 0f) * speed;
            else
                desiredVelocity = new Vector3(playerInput.x, 0f, playerInput.y) * speed;
            
            if (OnGround)
            {
                timeSinceGrounded = 0;
            }
            else
            {
                timeSinceGrounded += Time.deltaTime;   
            }

            if (OnGround) isJumping = false;
        }
        

        private IEnumerator MovementDrag()
        {
            while (gameObject.activeSelf)
            {
                if (hasScheduledJump || reduceDrag)
                {
                    rigidbody.drag = dragWhileMoving;
                    yield return reduceDragWaitForSeconds;
                    reduceDrag = false;
                }

                var noPlayerInput = math.abs(playerInput.x) < inputLimit && math.abs(playerInput.y) < inputLimit;

                if (noPlayerInput && groundContactCount > 0)
                    rigidbody.drag = dragWhileStopped;
                else
                    rigidbody.drag = dragWhileMoving;

                yield return null;
            }
        }

        private void FixedUpdate()
        {
            if (HaltMovement) return;
            if (StopMovementAndPhysics) return;
            velocity = rigidbody.velocity;

            UpdateGroundContacts();
            AdjustVelocity();

            Jump();

            rigidbody.velocity = velocity;

            RotatePlayerModel();
            ResetContactCounts();
        }

        private void HandlePlayerInput()
        {
            //if (StopMovementAndPhysics) return;
            playerInput.x = -Input.GetAxis("Horizontal");
            playerInput.y = -Input.GetAxis("Vertical");

            if (swapMovementAxis)
            {
                var tempInput = playerInput;
                playerInput.x = tempInput.y;
                playerInput.y = tempInput.x;
            }

            if (reverseX) playerInput.x = -playerInput.x;

            if (reverseY) playerInput.y = -playerInput.y;

            playerInput = Vector2.ClampMagnitude(playerInput, 1f);
            //if (Input.GetButtonDown("Jump")) hasScheduledJump = true;
            if (Input.GetButton("Jump")) hasScheduledJump = true;
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
            while (math.abs(transform.up.y - upVector.y) > 0.05f || math.abs(transform.up.x - upVector.x) > 0.05f ||
                   math.abs(transform.up.z - upVector.z) > 0.05f)
            {
                rigidbody.Sleep();
                transform.up = Vector3.Lerp(transform.up, upVector, gravityFlipTime * Time.deltaTime);
                yield return null;
            }

            //Snaps to new up vector just in case the lerp didnt finish
            transform.up = upVector;
            HaltMovement = false;
        }


        public void FlipMovement()
        {
            reverseX = !reverseX;
            reverseY = !reverseY;
        }

        public void SideCamera()
        {
            reverseX = !reverseX;
            swapMovementAxis = !swapMovementAxis;
        }

        private void ManageAnimation()
        {
            if (StopMovementAndPhysics)
            {
                zephAnimator.SetBool(jumpVariable, false);
                return;
            }
            zephAnimator.SetBool(jumpVariable, !OnGround);

            if (StopMovementAndPhysics) return;
            if (Input.GetKeyDown(KeyCode.M))
            {
                StopCoroutine(DanceRoutine());
                StartCoroutine(DanceRoutine());
            }
            
            //TODO Remove hardcoded values
            zephAnimator.SetFloat(moveVariable, desiredVelocity.magnitude > 0.25f ? 1.0f : 0f);

            //TODO avoid null checking during update
            if (jumpParticles == null || forwardParticles == null) return;
            
            if (!OnGround)
            {
                //play jumping particles
                jumpEmmision.rateOverTime = jumpEmmisionRate;
                //jumpParticles;
                moveForwardEmmision.rateOverTime = 0;
            }
            else
            {
                jumpEmmision.rateOverTime = 0;

                moveForwardEmmision.rateOverTime = desiredVelocity.magnitude > 0.25f ? moveForwardEmmisionRate : 0;
            }
        }

        private IEnumerator DanceRoutine()
        {
            zephAnimator.SetBool(isDancing, true);
            yield return danceWaitForSeconds;
            zephAnimator.SetBool(isDancing, false);
        }


        private void RotatePlayerModel()
        {
            if (playerInput.magnitude < float.Epsilon) return;

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

            if (isJumping) return;
 
            if (OnGround)
            {
                isJumping = true;
            }
            else
            {
                if (timeSinceGrounded < coyoteTime)
                {
                    isJumping = true;
                }
                else
                {
                    return;
                }
            }
            
            Vector3 jumpDirection = groundContactNormal;
            
            if (timeSinceGrounded < coyoteTime)
            {
                jumpDirection = upVector;
            }

            float jumpSpeed;
            float force;
            
            //TODO Clean here
            if (currentGravity.z < 0)
                force = currentGravity.z;
            else if (currentGravity.z > 0)
                force = -currentGravity.z;
            else if (currentGravity.y > 0)
                force = -currentGravity.y;
            else
                force = currentGravity.y;

            jumpSpeed = Mathf.Sqrt(-2f * force * jumpHeight);
            
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
                        break;
                    }
                }
                else
                {
                    if (math.abs(normal.y) > minGroundDotProduct)
                    {
                        groundContactCount++;
                        groundContactNormal = normal;
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
            reduceDrag = true;
        }

        public void TeleportPlayer(Transform point)
        {
            rigidbody.MovePosition(point.position);
        }
    }
}