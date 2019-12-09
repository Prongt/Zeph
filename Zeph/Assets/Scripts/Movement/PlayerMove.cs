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
	private float playerYHeight;
	private float characterWidth;

	private bool debugGravity = false;
	
	public static bool PlayerIsGrounded;
	private float gravityPull;
	public static bool _PlayerMovementEnabled = true;
	public float gravityFlipTime = 2;
	private Vector3 newUp;
	private Vector3 originalUp;

	[Header("Water Knock Back")]
	public float knockBackDistance = 0.75f;
	public LayerMask waterLayerMask;
	public float knockBackForce = 1.0f;
	public float movePauseTime = 0.25f;
	public StudioEventEmitter splashEmitter;

	private Vector3 velocity;
	void Start ()
	{
		camera = Camera.main.transform;
		
		GrabComponents();
		
		gravityDirection = Physics.gravity;
		oldGravity = gravityDirection;

		gravityJump = gravityJump + playerJumpHeight;
		playerYHeight = GetComponent<Collider>().bounds.extents.y + 0.1f;
		characterWidth = GetComponent<Collider>().bounds.extents.x + 0.15f;
		gravityPull = playerGravity;
		
		zephModel = zephAnimator.transform;

		_PlayerMovementEnabled = true;
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

	void Update () {
		Vector2 input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));
		Vector2 inputDir = input.normalized;

		gravityDirection = Physics.gravity;
		PlayerIsGrounded = CheckIfGrounded(gravityDirection, playerYHeight);

		if (gravityDirection != oldGravity)
		{
			if (GravityRift.useNewGravity)
			{
				newUp = new Vector3(-Physics.gravity.x, -Physics.gravity.y , -Physics.gravity.z ).normalized;
			}
			
			StartCoroutine(PausePlayerMovement(gravityFlipTime));
			oldGravity = Physics.gravity;
		}
		
		KnockBack();
		
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
				transform.up = Vector3.Lerp(transform.up, newUp, (gravityFlipTime * 1.5f)  * Time.deltaTime);
			}
		}
	}

	IEnumerator PausePlayerMovement(float pauseTime)
	{
		_PlayerMovementEnabled = false;
		yield return new WaitForSeconds(pauseTime);
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
			animator.SetBool("IsJumping", false);
		}
	}

	void Move(Vector2 inputDir, Vector3 upAxis) {
		upAxis.Normalize();
		
		float targetSpeed = playerMoveSpeed * inputDir.magnitude; 

		currentSpeed = Mathf.SmoothDamp (currentSpeed, targetSpeed, ref smoothingVelocity, GetModifiedSmoothTime(velocitySmoothing));

		
		velocityY += Time.deltaTime * gravityPull;
		
		velocity = new Vector3(-inputDir.x, 0, -inputDir.y);
		velocity.Normalize();
		velocity *= currentSpeed;

		var speed = velocity;
		zephAnimator.SetFloat("moveSpeed", speed.magnitude);
		velocity.y = velocityY;

		characterController.Move (velocity * Time.deltaTime);
		//currentSpeed = new Vector2 (characterController.velocity.x, characterController.velocity.z).magnitude;
		
		if (characterController.isGrounded) {
			velocityY = 0;
			animator.SetBool("IsJumping", false);
		}
	}
	
	public IEnumerator UseDanceAnimation()
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
				float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + camera.eulerAngles.y;

				float angle = Mathf.SmoothDampAngle(zephModel.localEulerAngles.y, targetRotation, ref turnVelocity,
					GetModifiedSmoothTime(playerTurnSpeed));

				var rot = zephModel.localEulerAngles;
				rot.y = angle;
				zephModel.localEulerAngles = rot;
			}
		}
	}

	void Jump() {
		if (Input.GetButtonDown("Jump"))
		{
			if (CheckIfGrounded(gravityDirection, playerYHeight))
			{
				animator.SetBool("IsJumping", true);
				float jumpVelocity;
				if (GravityRift.useNewGravity)
				{
					jumpVelocity = Mathf.Sqrt(-2 * playerGravity * gravityJump);
				}
				else
				{
					jumpVelocity = Mathf.Sqrt(-2 * playerGravity * playerJumpHeight);
				}
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

	
	public void KnockBack()
	{
		//Forward
		//Debug.DrawRay(transform.position, zephModel.forward * dist);
		Ray forwardRay = new Ray(transform.position, zephModel.forward);
		
		if (Physics.Raycast(forwardRay, out RaycastHit hit, knockBackDistance, waterLayerMask))
		{
			StartCoroutine(PausePlayerMovement(movePauseTime));

		
			Vector3 knockBackVector = transform.position - hit.point;
			//Debug.Log(knockBackVector);
			knockBackVector.Normalize();
			characterController.Move(knockBackVector * knockBackForce);
			splashEmitter.Play();
			
		}
		
		
		Ray downRay = new Ray(transform.position, -transform.up);
		if (Physics.Raycast(downRay, out RaycastHit downHit, knockBackDistance, waterLayerMask))
		{
			Vector3 knockBackVector = transform.position + Vector3.right + Vector3.forward;
			knockBackVector.Normalize();
			characterController.Move(knockBackVector * knockBackForce);
			splashEmitter.Play();
		}
	}

	

	private bool CheckIfGrounded(Vector3 direction, float distance)
	{
		if (GravityRift.useNewGravity)
		{
			return Physics.Raycast(transform.position, direction, distance);
		}
		else
		{
			return characterController.isGrounded;
		}
	}
	
	
	
}