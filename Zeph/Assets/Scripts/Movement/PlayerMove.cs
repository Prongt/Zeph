using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody myBody;
    

    private Vector3 forward;
    private Vector3 right;

    [Tooltip("Speed of Player")] 
    [SerializeField] private FloatReference playerSpeed;
    [SerializeField] private FloatReference playerTurnSpeed;
    [SerializeField] private FloatReference jumpForce;

    public float speed = 90f;
    public float turnSpeed = 5f;
    public float hoverForce = 65f;
    public float hoverHeight = 3.5f;
    
    public float sideMoveMultiplier;
    public float upMoveMultiplier;
    private float distanceToGround;
    
    
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        myBody = GetComponent<Rigidbody>();
        
        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        transform.forward = forward;
    }
    
    void FixedUpdate()
    {
//        if (!GravityDistortion.useNewGravity)
//        {
//            Move();
//            Jump();
//        }
//        else
//        {
//            AltMove();
//            AltJump();
//        }
        //AltMove();
        
        Move();
        Jump();
    }

    private void AltJump()
    {
        
    }

    private void AltMove()
    {
//        Vector3 newRight = Vector3.Cross(Physics.gravity, transform.forward);
//        Vector3 newForward = Vector3.Cross(newRight, Physics.gravity);
//        var moveSpeed = Time.deltaTime * playerSpeed.Value;
//        
//        //Movement
//        Vector3 movement = new Vector3();
//        
//        movement += newRight* (moveSpeed * -Input.GetAxis("Horizontal")) * sideMoveMultiplier; 
//        movement += newForward * (moveSpeed * Input.GetAxis("Vertical")) * upMoveMultiplier;
//        myBody.MovePosition(myBody.position + (movement * playerSpeed.Value));
//        
//        
//        
//        Vector3 heading = Vector3.Normalize(movement);
//        Vector3 lerpForward = math.lerp((float3) transform.forward, (float3) heading,
//            Time.deltaTime * playerTurnSpeed.Value);
//        transform.forward = new Vector3(lerpForward.x, 0, lerpForward.z);

        Ray ray = new Ray (transform.position, -transform.up);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, hoverHeight))
        {
            float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
            Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
            myBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
        }
        
        Vector3 movement = new Vector3();
        movement += right * (playerSpeed.Value * Input.GetAxis("Horizontal")); 
        movement += forward * (playerSpeed.Value * Input.GetAxis("Vertical"));
        var test = movement * playerSpeed.Value;
        //myBody.MovePosition(myBody.position + (movement * playerSpeed.Value));
        myBody.AddRelativeForce(test.x, 0, test.z);
        
                Vector3 heading = Vector3.Normalize(movement);
        Vector3 lerpForward = math.lerp((float3) transform.forward, (float3) heading,
            Time.deltaTime * playerTurnSpeed.Value);
        transform.forward = new Vector3(lerpForward.x, 0, lerpForward.z);
        //myBody.AddRelativeTorque(0f, Input.GetAxis ("Horizontal") * turnSpeed, 0f);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && CheckIfGrounded())
        {
            myBody.AddForce(transform.up * jumpForce.Value, ForceMode.Impulse);
        }
    }

    private bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, -transform.up, distanceToGround + 0.1f);
    }

    void Move()
    {
        var moveSpeed = Time.deltaTime * playerSpeed.Value;
        
        //Movement
        Vector3 movement = new Vector3();
        movement += right * (moveSpeed * Input.GetAxis("Horizontal")); 
        movement += forward * (moveSpeed * Input.GetAxis("Vertical"));
        myBody.MovePosition(myBody.position + (movement * playerSpeed.Value));

        //Rotation
        Vector3 heading = Vector3.Normalize(movement);
        Vector3 lerpForward = math.lerp((float3) transform.forward, (float3) heading,
            Time.deltaTime * playerTurnSpeed.Value);
        transform.forward = new Vector3(lerpForward.x, 0, lerpForward.z);
    }  
}
