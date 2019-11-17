using System;
using Unity.Mathematics;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{

    
	public float playerMoveSpeed = 6f;
	public float playerGravity = -12f;
	public float playerJumpHeight = 1f;
	[Range(0,1)]
	public float airControlPercent;

	public float playerTurnSpeed = 0.2f;
	float turnVelocity;

	public float velocitySmoothing = 0.1f; 
	float smoothingVelocity;
	float currentSpeed;
	float velocityY;
	
	Transform cam;
	CharacterController characterController;

	private float gravityJump = 0.5f;
	private Vector3 oldGravity;
	private Vector3 gravityDirection;
	private float distanceToGround;

	private bool debugGravity = false;
	
	public static bool IsGrounded;

	void Start () {
		cam = Camera.main.transform;
		characterController = GetComponent<CharacterController> ();
		
		gravityDirection = Physics.gravity;
		oldGravity = gravityDirection;

		gravityJump = gravityJump + playerJumpHeight;
		distanceToGround = GetComponent<Collider>().bounds.extents.y;
	}

	void Update () {
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;

		gravityDirection = Physics.gravity;
		IsGrounded = CheckIfGrounded();

		if (gravityDirection != oldGravity)
		{
			var newUp = new Vector3(-Physics.gravity.x, -Physics.gravity.y , -Physics.gravity.z ).normalized;
			transform.up = newUp;
			oldGravity = Physics.gravity;
		}
		
		
		if (GravityRift.useNewGravity)
		{
			AltMove(inputDir, -Physics.gravity);
		}
		else
		{
			Move(inputDir, -Physics.gravity);
		}

		
		//Debug.Log(gravityDirection.magnitude);

		if (Input.GetButtonDown("Jump")) {
			Jump ();
		}
	}
	

	private void AltMove(Vector2 inputDir, Vector3 upAxis)
	{
		upAxis.Normalize();
		

			
		float targetSpeed = playerMoveSpeed * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref smoothingVelocity, GetModifiedSmoothTime(velocitySmoothing));

		Vector3 velocity = new Vector3();
		if (gravityDirection.x > 0 || gravityDirection.x < 0)
		{
              //print("x Dir");

			velocityY += Time.deltaTime * playerGravity;
			//movement
			velocity.y = inputDir.y * playerMoveSpeed;
			velocity.z = -inputDir.x * playerMoveSpeed;
			//velocity.x -= velocityY;
			
			//gravity
			if (gravityDirection.x > 0)
			{
				velocity.x -= velocityY;
			}else if (gravityDirection.x < 0)
			{
				velocity.x += velocityY;
			}
			
			//Rotation 1
//			if (inputDir != Vector2.zero) 
//			{
//			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
//			transform.eulerAngles = transform.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
//			}
			
			//Rotation 2
//			var vel = velocity;
//			vel.x = 0;
//			var quat = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(vel, -gravityDirection),
//				turnSmoothTime * Time.deltaTime);
////			quat.x = 0;
//			quat.z = 0;
//			quat.y = 0;
			//transform.rotation = transform.rotation * quat;
			//transform.rotation *= Quaternion.Euler(0,quat.x * rotMul,0);

			if (debugGravity)
			{
				Debug.Log("Up = " + velocity.y + "     Side = " + velocity.z);
				Debug.Log("x Dir");
			}
		}
		else if (gravityDirection.z > 0 || gravityDirection.z < 0)
		{
			velocityY += Time.deltaTime * playerGravity;
			//Movement
			velocity.y = inputDir.y * playerMoveSpeed;
			velocity.x = -inputDir.x * playerMoveSpeed;

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
		if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
			transform.eulerAngles = upAxis * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnVelocity, GetModifiedSmoothTime(playerTurnSpeed));
		}
			
		float targetSpeed = playerMoveSpeed * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref smoothingVelocity, GetModifiedSmoothTime(velocitySmoothing));

		velocityY += Time.deltaTime * playerGravity;
		Vector3 velocity = transform.forward * currentSpeed + upAxis * velocityY;

		characterController.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (characterController.velocity.x, characterController.velocity.z).magnitude;
		
		
		if (characterController.isGrounded) {
			velocityY = 0;
		}

	}

	void Jump() {
		if (CheckIfGrounded())
		{
			float jumpVelocity;
			if (GravityRift.useNewGravity)
			{
				jumpVelocity = Mathf.Sqrt (-2 * playerGravity * gravityJump);
			}
			else
			{
				jumpVelocity = Mathf.Sqrt (-2 * playerGravity * playerJumpHeight);
			}
			//float jumpVelocity = Mathf.Sqrt (-2 * playerGravity * playerJumpHeight);
			velocityY = jumpVelocity;
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
	
	private bool CheckIfGrounded()
	{
		//return Physics.Raycast(transform.position, Physics.gravity, distanceToGround + 0.1f);
		return Physics.Raycast(transform.position, gravityDirection, distanceToGround + 0.1f);
	}
}