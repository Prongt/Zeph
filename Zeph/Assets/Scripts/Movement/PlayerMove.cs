using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
	public float playerMoveSpeed = 6f;
	public float playerGravity = -12f;
	public float playerJumpHeight = 1f;
	[Range(0,1)]
	public float airControlPercent;

	private Transform zephModel;
	private Animator zephAnimator;

	public float playerTurnSpeed = 0.2f;
	float turnVelocity;

	public float velocitySmoothing = 0.1f;
	
	float smoothingVelocity;
	float currentSpeed;
	float velocityY;
	
	CharacterController characterController;
	private Transform camera;
	private Animator animator;
	

	private float gravityJump = 0.5f;
	private Vector3 oldGravity;
	private Vector3 gravityDirection;
	private float distanceToGround;
	private float distanceToGroundWidth;

	private bool debugGravity = false;
	
	public static bool PlayerIsGrounded;
	public static bool PlayerUsesGravity = true;
	private float gravityPull;
	public static bool _PlayerMovementEnabled = true;
	public float gravityFlipTime = 2;
	private Vector3 newUp;
	private Vector3 originalUp;
	private Quaternion originalrot;
	
	
	void Start ()
	{
		camera = Camera.main.transform;
		characterController = GetComponent<CharacterController> ();
		animator = GetComponentInChildren<Animator>();
		
		gravityDirection = Physics.gravity;
		oldGravity = gravityDirection;

		gravityJump = gravityJump + playerJumpHeight;
		distanceToGround = GetComponent<Collider>().bounds.extents.y + 0.1f;
		distanceToGroundWidth = GetComponent<Collider>().bounds.extents.x + 0.15f;
		gravityPull = playerGravity;

		zephAnimator = GetComponentInChildren<Animator>();
		zephModel = GetComponentInChildren<Animator>().transform;

		_PlayerMovementEnabled = true;
		originalUp = transform.up;
		originalrot = transform.rotation;
	}

	void Update () {
		if (PlayerUsesGravity)
		{
			gravityPull = playerGravity;
		}
		else
		{
			gravityPull = 0;
		}
		
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;

		gravityDirection = Physics.gravity;
		PlayerIsGrounded = CheckIfGrounded(gravityDirection, distanceToGround);

		if (gravityDirection != oldGravity)
		{
			if (GravityRift.useNewGravity)
			{
				newUp = new Vector3(-Physics.gravity.x, -Physics.gravity.y , -Physics.gravity.z ).normalized;
			}
			
			StartCoroutine(LerpTransformUp());
			oldGravity = Physics.gravity;
		}


		if (_PlayerMovementEnabled)
		{
			if (GravityRift.useNewGravity)
			{
				AltMove(inputDir, -Physics.gravity);
			}
			else
			{
				Move(inputDir, -Physics.gravity);
			}
		
			Rotate(inputDir);
			
			if (Input.GetButtonDown("Jump")) {
				Jump ();
			}

			if (Input.GetKeyDown(KeyCode.E))
			{
				StartCoroutine(UseDanceAnimation());
			}
		}
		else
		{
			if (GravityRift.useNewGravity == false)
			{
				transform.up = Vector3.Lerp(transform.up, originalUp, gravityFlipTime * Time.deltaTime);
				transform.rotation = originalrot;
			}
			else
			{
				transform.up = Vector3.Lerp(transform.up, newUp, gravityFlipTime * Time.deltaTime);
			}
		}
	}

	IEnumerator LerpTransformUp()
	{
		_PlayerMovementEnabled = false;
		yield return new WaitForSeconds(gravityFlipTime);
		_PlayerMovementEnabled = true;
	}
	

	private void AltMove(Vector2 inputDir, Vector3 upAxis)
	{
		upAxis.Normalize();

		float targetSpeed = playerMoveSpeed * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref smoothingVelocity, GetModifiedSmoothTime(velocitySmoothing));

		Vector3 velocity = new Vector3();
		if (gravityDirection.x > 0 || gravityDirection.x < 0)
		{
			velocityY += Time.deltaTime * gravityPull;
			//movement
			velocity.y = inputDir.y * playerMoveSpeed;
			Debug.Log(velocity.y);

			
			//Stops player from getting stuck in the ground/wall when the player is in the air and is movinging in the direction of the ground/wall
			if (!PlayerIsGrounded)
			{
				if (CheckIfGrounded(Vector3.down, distanceToGroundWidth))
				{
					inputDir.y = -inputDir.y;
				}
				if (CheckIfGrounded(Vector3.up, distanceToGroundWidth))
				{
					inputDir.y = -inputDir.y;
				}
				if (CheckIfGrounded(Vector3.left, distanceToGroundWidth))
				{
					inputDir.x = -inputDir.x;
				}
				if (CheckIfGrounded(Vector3.right, distanceToGroundWidth))
				{
					inputDir.x = -inputDir.x;
				}
			}
			
			velocity.y = inputDir.y * playerMoveSpeed;
			velocity.z = -inputDir.x * playerMoveSpeed;
			
			zephAnimator.SetFloat("moveSpeed", velocity.magnitude);

			//gravity
			if (gravityDirection.x > 0)
			{
				velocity.x -= velocityY;
			}else if (gravityDirection.x < 0)
			{
				velocity.x += velocityY;
			}


			if (debugGravity)
			{
				Debug.Log("Up = " + velocity.y + "     Side = " + velocity.z);
				Debug.Log("x Dir");
			}
		}
		else if (gravityDirection.z > 0 || gravityDirection.z < 0)
		{
			velocityY += Time.deltaTime * gravityPull;
			//Movement
			velocity.y = inputDir.y * playerMoveSpeed;
			velocity.x = -inputDir.x * playerMoveSpeed;
			
			var speed = velocity;
			zephAnimator.SetFloat("moveSpeed", velocity.magnitude);

			//Gravity
			if (gravityDirection.z > 0)
			{
				velocity.z -= velocityY;
			}else if (gravityDirection.z < 0)
			{
				velocity.z += velocityY;
			}
		}
		else
		{
			velocityY += Time.deltaTime * playerGravity;
			velocity = transform.forward * currentSpeed + upAxis * velocityY;
		}

		characterController.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (characterController.velocity.x, characterController.velocity.y).magnitude;

		if (characterController.isGrounded) {
			velocityY = 0;
		}
	}

	void Move(Vector2 inputDir, Vector3 upAxis) {
		upAxis.Normalize();
		
		float targetSpeed = playerMoveSpeed * inputDir.magnitude; 

		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref smoothingVelocity, GetModifiedSmoothTime(velocitySmoothing));

		velocityY += Time.deltaTime * gravityPull;
		
		Vector3 velocity = new Vector3(-inputDir.x, 0, -inputDir.y);
		velocity.Normalize();
		velocity *= currentSpeed;

		var speed = velocity;
		zephAnimator.SetFloat("moveSpeed", speed.magnitude);

		velocity.y = velocityY;

		characterController.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (characterController.velocity.x, characterController.velocity.z).magnitude;
		
		if (characterController.isGrounded) {
			velocityY = 0;
		}
	}
	
	IEnumerator UseDanceAnimation()
	{
		animator.SetBool("IsDancing", true);
		yield return new WaitForSeconds(8f);
		animator.SetBool("IsDancing", false);
	}

	private void Rotate(Vector2 inputDir)
	{
		if (inputDir != Vector2.zero)
		{
			if (GravityRift.useNewGravity)
			{
				float targetRotation = Mathf.Atan2(-inputDir.x, -inputDir.y) * Mathf.Rad2Deg;

				float angle = Mathf.SmoothDampAngle(zephModel.localEulerAngles.y, targetRotation, ref turnVelocity,
					GetModifiedSmoothTime(playerTurnSpeed));

				var rot = zephModel.localEulerAngles;
				rot.y = angle;
				zephModel.localEulerAngles = rot;
			}
			else
			{
				float targetRotation = Mathf.Atan2(-inputDir.x, -inputDir.y) * Mathf.Rad2Deg + camera.eulerAngles.y;

				float angle = Mathf.SmoothDampAngle(zephModel.localEulerAngles.y, targetRotation, ref turnVelocity,
					GetModifiedSmoothTime(playerTurnSpeed));

				var rot = zephModel.localEulerAngles;
				rot.y = angle;
				zephModel.localEulerAngles = rot;
			}
		}
	}

	void Jump() {
		if (GravityRift.useNewGravity)
		{
			if (CheckIfGrounded(gravityDirection, distanceToGround))
			{
				float jumpVelocity;
				jumpVelocity = Mathf.Sqrt (-2 * playerGravity * gravityJump);
				velocityY = jumpVelocity;
			}
		}
		else
		{
			if (characterController.isGrounded)
			{
				float jumpVelocity;
				jumpVelocity = Mathf.Sqrt (-2 * playerGravity * playerJumpHeight);
				velocityY = jumpVelocity;
			}
		}
	}

	float GetModifiedSmoothTime(float smoothTime) {
		if (characterController.isGrounded) {
			return smoothTime;
		}

		if (airControlPercent == 0) {
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}
	
	private bool CheckIfGrounded(Vector3 direction, float distance)
	{
		return Physics.Raycast(transform.position, direction, distance);
	}
}