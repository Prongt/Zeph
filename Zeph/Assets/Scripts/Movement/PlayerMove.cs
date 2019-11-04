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
        Move();
        Jump();
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
