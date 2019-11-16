using System;
using Unity.Mathematics;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{

    
	public float walkSpeed = 6;
	public float gravity = -12;
	public float jumpHeight = 1;
	[Range(0,1)]
	public float airControlPercent;

	public float turnSmoothTime = 0.2f;
	float turnSmoothVelocity;

	public float speedSmoothTime = 0.1f;
	float speedSmoothVelocity;
	float currentSpeed;
	float velocityY;
	
	Transform cameraT;
	CharacterController controller;
	
	private Vector3 oldGravity;
	private Vector3 gravityDirection;
	private float distanceToGround;

	private bool debugGravity = false;

	public float rotMul;

	void Start () {
		cameraT = Camera.main.transform;
		controller = GetComponent<CharacterController> ();
		
		gravityDirection = Physics.gravity;
		oldGravity = gravityDirection;
		
		distanceToGround = GetComponent<Collider>().bounds.extents.y;
	}

	void Update () {
		// input
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;

		gravityDirection = Physics.gravity;

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

		
		Debug.Log(gravityDirection.magnitude);

		if (Input.GetButtonDown("Jump")) {
			Jump ();
		}
	}
	

	private void AltMove(Vector2 inputDir, Vector3 upAxis)
	{
		upAxis.Normalize();
		

			
		float targetSpeed = walkSpeed * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		Vector3 velocity = new Vector3();
		if (gravityDirection.x > 0 || gravityDirection.x < 0)
		{
              //print("x Dir");

			velocityY += Time.deltaTime * gravity;
			//movement
			velocity.y = inputDir.y * walkSpeed;
			velocity.z = -inputDir.x * walkSpeed;
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
			velocityY += Time.deltaTime * gravity;
			//Movement
			velocity.y = inputDir.y * walkSpeed;
			velocity.x = -inputDir.x * walkSpeed;

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
			velocityY += Time.deltaTime * gravity;
			velocity = transform.forward * currentSpeed + upAxis * velocityY;
		}

		

		controller.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (controller.velocity.x, controller.velocity.y).magnitude;

		if (controller.isGrounded) {
			velocityY = 0;
		}
	}

	void Move(Vector2 inputDir, Vector3 upAxis) {
		upAxis.Normalize();
		if (inputDir != Vector2.zero) {
			float targetRotation = Mathf.Atan2 (inputDir.x, inputDir.y) * Mathf.Rad2Deg + cameraT.eulerAngles.y;
			transform.eulerAngles = upAxis * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref turnSmoothVelocity, GetModifiedSmoothTime(turnSmoothTime));
		}
			
		float targetSpeed = walkSpeed * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref speedSmoothVelocity, GetModifiedSmoothTime(speedSmoothTime));

		velocityY += Time.deltaTime * gravity;
		Vector3 velocity = transform.forward * currentSpeed + upAxis * velocityY;

		controller.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (controller.velocity.x, controller.velocity.z).magnitude;
		
		
		if (controller.isGrounded) {
			velocityY = 0;
		}

	}

	void Jump() {
		if (CheckIfGrounded()) {
			float jumpVelocity = Mathf.Sqrt (-2 * gravity * jumpHeight);
			velocityY = jumpVelocity;
		}
	}

	float GetModifiedSmoothTime(float smoothTime) {
		if (controller.isGrounded) {
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