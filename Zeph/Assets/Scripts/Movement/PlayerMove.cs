using System.Collections;
using FMODUnity;
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
	private float turnVelocity;

	public float velocitySmoothing = 0.1f;

	private float smoothingVelocity;
	private float currentSpeed;
	private float upVelocity;

	private CharacterController characterController;
	private new Transform camera;
	private Animator animator;
	

	private float gravityJump = 0.5f;
	private Vector3 oldGravity;
	private Vector3 gravityDirection;
	private float playerYHeight;
	private float characterWidth;


	
	public static bool PlayerIsGrounded;
	private float gravityPull;
	public static bool PlayerMovementEnabled = true;
	public float gravityFlipTime = 2;
	private Vector3 newUp;
	private Vector3 originalUp;
	[SerializeField] private float coyoteTime = 0.15f;
	private float coyoteTimeRemaining;
	
	[Header("Tick if movement is janky")]
	public bool reverseMovementDirections;
	public bool flipMovement;

	[Header("Water Knock Back")]
	public float knockBackDistance = 0.75f;
	public LayerMask waterLayerMask;
	public float knockBackForce = 1.0f;
	public float movePauseTime = 0.25f;
	public StudioEventEmitter splashEmitter;

	private Vector3 velocity;
	private static readonly int moveSpeed = Animator.StringToHash("moveSpeed");
	private static readonly int isJumping = Animator.StringToHash("IsJumping");
	private static readonly int isDancing = Animator.StringToHash("IsDancing");

	void Start ()
	{
		if (Camera.main != null) camera = Camera.main.transform;

		GrabComponents();
		
		gravityDirection = Physics.gravity;
		oldGravity = gravityDirection;

		gravityJump = gravityJump + playerJumpHeight;
		playerYHeight = GetComponent<Collider>().bounds.extents.y + 0.1f;
		characterWidth = GetComponent<Collider>().bounds.extents.x + 0.15f;
		gravityPull = playerGravity;
		
		zephModel = zephAnimator.transform;

		PlayerMovementEnabled = true;
		originalUp = transform.up;
		gravityPull = playerGravity;
	}

	private void OnValidate()
	{
		GrabComponents();
	}

	private void GrabComponents()
	{
		if (splashEmitter == null)
		{
			splashEmitter = GetComponent<StudioEventEmitter>();
		}
		
		characterController = GetComponent<CharacterController> ();
		animator = GetComponentInChildren<Animator>();
		zephAnimator = GetComponentInChildren<Animator>();
	}

	void Update ()
	{
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		Vector2 inputDir = input.normalized;

		gravityDirection = Physics.gravity;
		PlayerIsGrounded = CheckIfGrounded(gravityDirection, playerYHeight);

		if (gravityDirection != oldGravity)
		{
			newUp = new Vector3(-Physics.gravity.x, -Physics.gravity.y , -Physics.gravity.z ).normalized;

			StartCoroutine(PausePlayerMovement(gravityFlipTime, inputDir));
			oldGravity = Physics.gravity;
		}
		
		if (PlayerIsGrounded)
		{
			coyoteTimeRemaining = coyoteTime;
		}
		else
		{
			coyoteTimeRemaining -= Time.deltaTime;
			
		}
		
		KnockBack();
		
		if (PlayerMovementEnabled)
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
			
			Jump();
		}
		else
		{
			if (GravityRift.useNewGravity == false)
			{
				transform.up = Vector3.Lerp(transform.up, originalUp, (gravityFlipTime * 1.5f) * Time.deltaTime);
				//transform.rotation = originalrot;
			}
			else
			{
				if (gravityDirection.y > 9f)
				{
					newUp = new Vector3(0,-1, 0);
				}
				transform.up = Vector3.Lerp(transform.up, newUp, (gravityFlipTime * 1.5f)  * Time.deltaTime);
			}
		}
	}


	IEnumerator PausePlayerMovement(float pauseTime, Vector2 inputDir = new Vector2())
	{
		if (inputDir != new Vector2())
		{
			float jumpVelocity;
		
			if (GravityRift.useNewGravity)
			{
				jumpVelocity = Mathf.Sqrt(-2 * playerGravity * gravityJump);
				upVelocity = jumpVelocity;
				AltMove(inputDir, -Physics.gravity);
			}
			else
			{
				jumpVelocity = Mathf.Sqrt(-2 * playerGravity * playerJumpHeight);
				upVelocity = jumpVelocity;
				Move(inputDir, -Physics.gravity);
			}
		}
		
		PlayerMovementEnabled = false;
		yield return new WaitForSeconds(pauseTime);
		PlayerMovementEnabled = true;
	}
	

	private void AltMove(Vector2 inputDir, Vector3 upAxis)
	{
		upAxis.Normalize();

		float targetSpeed = playerMoveSpeed * inputDir.magnitude;
		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref smoothingVelocity, GetModifiedSmoothTime(velocitySmoothing));
		
		Vector3 velocity = new Vector3();
		if (gravityDirection.x > 0 || gravityDirection.x < 0)
		{
			Debug.Log("Cheese");
			upVelocity += Time.deltaTime * gravityPull;
			//movement
			velocity.y = inputDir.y * playerMoveSpeed;

			//Stops player from getting stuck in the ground/wall when the player is in the air and is movinging in the direction of the ground/wall
			if (!PlayerIsGrounded)
			{
				if (CheckIfGrounded(Vector3.down, characterWidth))
				{
					inputDir.y = -inputDir.y;
				}
				if (CheckIfGrounded(Vector3.up, characterWidth))
				{
					inputDir.y = -inputDir.y;
				}
				if (CheckIfGrounded(Vector3.left, characterWidth))
				{
					inputDir.x = -inputDir.x;
				}
				if (CheckIfGrounded(Vector3.right, characterWidth))
				{
					inputDir.x = -inputDir.x;
				}
			}
			
			velocity.y = inputDir.y * playerMoveSpeed;
			velocity.z = -inputDir.x * playerMoveSpeed;
			
			zephAnimator.SetFloat(moveSpeed, velocity.magnitude);

			//gravity
			if (gravityDirection.x > 0)
			{
				velocity.x -= upVelocity;
			}else if (gravityDirection.x < 0)
			{
				velocity.x += upVelocity;
			}
		}
		else if (gravityDirection.z > 0 || gravityDirection.z < 0)
		{
			upVelocity += Time.deltaTime * gravityPull;
			//Movement
			if (reverseMovementDirections)
			{
				if (gravityDirection.z > 0)
				{
					velocity.y = -inputDir.x * playerMoveSpeed;
				}
				else
				{
					velocity.y = inputDir.x * playerMoveSpeed;
				}
				
				velocity.x = inputDir.y * playerMoveSpeed;
			}
			else
			{
				velocity.y = inputDir.y * playerMoveSpeed;
				velocity.x = -inputDir.x * playerMoveSpeed;
			}
			
			
			zephAnimator.SetFloat(moveSpeed, velocity.magnitude);

			//Gravity
			if (gravityDirection.z > 0)
			{
				velocity.z -= upVelocity;
			}else if (gravityDirection.z < 0)
			{
				velocity.z += upVelocity;
			}
		}else if (gravityDirection.y > 9f)
		{
//			Debug.Log("Up Move");
			upVelocity += Time.deltaTime * gravityPull;
			// //velocity = transform.forward * currentSpeed + upAxis * velocityY;
			// velocity = transform.forward * currentSpeed + upAxis;
			// velocity.y = velocityY;
			//velocity = transform.forward;
			var tempInput = inputDir;
			if (flipMovement)
			{
				tempInput.x = -tempInput.x;
				tempInput.y = -tempInput.y;
			}
			
			if (reverseMovementDirections)
			{
				velocity.x = tempInput.y * playerMoveSpeed;
				velocity.z = tempInput.x * playerMoveSpeed;
			}
			else
			{
				velocity.x = -tempInput.x * playerMoveSpeed;
				velocity.z = -tempInput.y * playerMoveSpeed;
			}
			
			velocity.y -= upVelocity;
			
			zephAnimator.SetFloat(moveSpeed, velocity.magnitude);
		}
		else
		{
			upVelocity += Time.deltaTime * playerGravity;
			velocity = transform.forward * currentSpeed + upAxis * upVelocity;
		}
		//
		// if (reverseMovementDirections)
		// {
		// 	velocity = new Vector3(inputDir.y, 0, -inputDir.x);
		// }else
		// {
		// 	velocity = new Vector3(-inputDir.x, 0, -inputDir.y);
		// }
		//
		// if (flipMovement)
		// {
		// 	velocity = new Vector3(-velocity.x, 0, -velocity.z);
		// }

		characterController.Move (velocity * Time.deltaTime);
		currentSpeed = new Vector2 (characterController.velocity.x, characterController.velocity.y).magnitude;

		//if (CheckIfGrounded(Physics.gravity, 5f))
		if (CheckIfGrounded(Physics.gravity, 5f)) {
			upVelocity = 0;
			animator.SetBool(isJumping, false);
		}
	}

	void Move(Vector2 inputDir, Vector3 upAxis) {
		upAxis.Normalize();
		
		float targetSpeed = playerMoveSpeed * inputDir.magnitude; 

		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref smoothingVelocity, GetModifiedSmoothTime(velocitySmoothing));

		
		upVelocity += Time.deltaTime * gravityPull;

		if (reverseMovementDirections)
		{
			velocity = new Vector3(inputDir.y, 0, -inputDir.x);
		}else
		{
			velocity = new Vector3(-inputDir.x, 0, -inputDir.y);
		}

		if (flipMovement)
		{
			velocity = new Vector3(-velocity.x, 0, -velocity.z);
		}
		
		
		velocity.Normalize();
		velocity *= currentSpeed;

		var speed = velocity;
		zephAnimator.SetFloat(moveSpeed, speed.magnitude);
		velocity.y = upVelocity;

		characterController.Move (velocity * Time.deltaTime);
		//currentSpeed = new Vector2 (characterController.velocity.x, characterController.velocity.z).magnitude;
		
		if (characterController.isGrounded) {
			upVelocity = 0;
			animator.SetBool(isJumping, false);
		}
	}
	
	public IEnumerator UseDanceAnimation()
	{
		animator.SetBool(isDancing, true);
		yield return new WaitForSeconds(8f);
		animator.SetBool(isDancing, false);
	}

	public void FlipMovement()
	{
		flipMovement = !flipMovement;
	}

	private void Rotate(Vector2 inputDir)
	{
		if (inputDir != Vector2.zero)
		{
			if (GravityRift.useNewGravity)
			{
				float targetRotation = Mathf.Atan2(-inputDir.x, -inputDir.y) * Mathf.Rad2Deg;
				if (gravityDirection.z > 0 || gravityDirection.z < 0)
				{
					
				}

				if (flipMovement)
				{
					inputDir.x = -inputDir.x;
					inputDir.y = -inputDir.y;
				}
				
				if (reverseMovementDirections)
				{
					targetRotation = Mathf.Atan2(inputDir.y, -inputDir.x) * Mathf.Rad2Deg;
				}


				float angle = Mathf.SmoothDampAngle(zephModel.localEulerAngles.y, targetRotation, ref turnVelocity,
						GetModifiedSmoothTime(playerTurnSpeed));

					var rot = zephModel.localEulerAngles;
					rot.y = angle;
					zephModel.localEulerAngles = rot;
			}
			else
			{
				float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camera.eulerAngles.y;

				float angle = Mathf.SmoothDampAngle(zephModel.localEulerAngles.y, targetRotation, ref turnVelocity,
					GetModifiedSmoothTime(playerTurnSpeed));

				var rot = zephModel.localEulerAngles;
				rot.y = angle;
				zephModel.localEulerAngles = rot;
			}
		}
	}

	void Jump()
	{
		if (!Input.GetButtonDown("Jump")) return;

		if (!CheckIfGrounded(gravityDirection, playerYHeight) && !(coyoteTimeRemaining > 0)) return;
		
		animator.SetBool(isJumping, true);
		float jumpVelocity;
		
		if (GravityRift.useNewGravity)
		{
			jumpVelocity = Mathf.Sqrt(-2 * playerGravity * gravityJump);
		}
		else
		{
			jumpVelocity = Mathf.Sqrt(-2 * playerGravity * playerJumpHeight);
		}
		upVelocity = jumpVelocity;
	}

	private float GetModifiedSmoothTime(float smoothTime) {
		if (characterController.isGrounded) {
			return smoothTime;
		}

		if (airControlPercent <= 0.01f) {
			return float.MaxValue;
		}
		return smoothTime / airControlPercent;
	}


	private void KnockBack()
	{
		var forwardRay = new Ray(transform.position, zephModel.forward);
		
		if (Physics.Raycast(forwardRay, out var hit, knockBackDistance, waterLayerMask))
		{
			StartCoroutine(PausePlayerMovement(movePauseTime));

			var knockBackVector = transform.position - hit.point;
			knockBackVector.Normalize();
			characterController.Move(knockBackVector * knockBackForce);
			splashEmitter.Play();
			
		}

		var groundRay = new Ray(transform.position, -transform.up);
		if (Physics.Raycast(groundRay, out var downHit, knockBackDistance, waterLayerMask))
		{
			StartCoroutine(PausePlayerMovement(movePauseTime));
			var knockBackVector = transform.position + Vector3.right + Vector3.forward;
			if (downHit.collider.CompareTag("Water"))
			{
				knockBackVector = transform.position + Vector3.right;
			}
			
			knockBackVector.Normalize();
			characterController.Move(knockBackVector * (knockBackForce *2));
			splashEmitter.Play();
		}
	}

	private bool CheckIfGrounded(Vector3 direction, float distance)
	{
		return GravityRift.useNewGravity ? Physics.Raycast(transform.position, direction, distance) : characterController.isGrounded;
	}
}