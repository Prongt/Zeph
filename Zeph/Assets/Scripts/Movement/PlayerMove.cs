using System;
using Unity.Mathematics;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{
    private Rigidbody myBody;
    private Vector3 forward;

    private Vector3 right;

    private Vector3 gravityDirection;
    //private Vector3 movement;

    [Tooltip("Speed of Player")] [SerializeField]
    private FloatReference playerSpeed;

    [SerializeField] private FloatReference playerTurnSpeed;
    [SerializeField] private FloatReference jumpForce;

    public float sideMoveMultiplier;
    public float upMoveMultiplier;
    private float distanceToGround;

    private bool rot = false;

    private void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        myBody = GetComponent<Rigidbody>();

        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        transform.forward = forward;
        gravityDirection = Physics.gravity;
    }

    private void Update()
    {
        gravityDirection = Physics.gravity;
        if (!GravityDistortion.useNewGravity)
        {

            Move();
            Jump();
        }
        else
        {
            Jump();
            AltMove();

            if (!rot)
            {
                transform.up = -gravityDirection;
                rot = true;
            }
            

        }
    }


    private void Move()
    {
        var moveSpeed = Time.deltaTime * playerSpeed.Value;

        //Movement
        var movement = new Vector3();
        movement += right * (moveSpeed * Input.GetAxis("Horizontal"));
        movement += forward * (moveSpeed * Input.GetAxis("Vertical"));
        myBody.MovePosition(myBody.position + movement * playerSpeed.Value);


        
        //Rotation
        var heading = Vector3.Normalize(movement);
        Vector3 lerpForward = math.lerp((float3) transform.forward, (float3) heading,
            Time.deltaTime * playerTurnSpeed.Value);
        transform.forward = new Vector3(lerpForward.x, 0, lerpForward.z);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && CheckIfGrounded())
        {
            myBody.AddForce(-gravityDirection.normalized * jumpForce.Value, ForceMode.Impulse);
        }
    }

    private bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, gravityDirection, distanceToGround + 0.1f);
    }


    private void AltMove()
    {
        
        
        
        var moveSpeed = Time.deltaTime * playerSpeed.Value;

        //Movement
        var movement = new Vector3();
        movement.y = moveSpeed * Input.GetAxis("Vertical");
        movement.z = moveSpeed * -Input.GetAxis("Horizontal");

        myBody.MovePosition(myBody.position + movement * playerSpeed.Value);


        if (movement.magnitude > 0)
        {
            myBody.MoveRotation(Quaternion.LookRotation(movement, -gravityDirection));
        }
        //Quaternion.LookRotation(movement);
        

//        var rot = transform.rotation;
//        rot.x += movement.normalized.y;
//        myBody.MoveRotation(rot);

//        var heading = Vector3.Normalize(movement);
//        Vector3 lerpForward = math.lerp((float3) transform.up, (float3) heading,
//            Time.deltaTime * playerTurnSpeed.Value);
//        //transform.forward = new Vector3(lerpForward.y + lerpForward.z, 0, 0);
//        transform.forward = lerpForward;
    }
}