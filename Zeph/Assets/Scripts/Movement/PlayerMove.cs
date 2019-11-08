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

    private Vector3 oldGravity;

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
        oldGravity = gravityDirection;
    }

    private void Update()
    {
        gravityDirection = Physics.gravity;
        if (gravityDirection != oldGravity)
        {
            transform.up = -gravityDirection;
            oldGravity = gravityDirection;
        }

        Jump();
        if (!GravityDistortion.useNewGravity)
        {
            Move();
        }
        else
        {
            AltMove();
        }
    }


    private void Move()
    {
        var moveSpeed = Time.deltaTime * playerSpeed.Value;

        //Movement
        
        //if (Physics.)
        
        var movement = new Vector3();
        movement += right * (moveSpeed * Input.GetAxis("Horizontal"));
        movement += forward * (moveSpeed * Input.GetAxis("Vertical"));
        myBody.MovePosition(myBody.position + movement * playerSpeed.Value);

       
        //Rotation
        if (movement.magnitude > 0)
        {
            var quat = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, -gravityDirection),
                playerTurnSpeed * Time.deltaTime);
            myBody.MoveRotation(quat);
        }
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

        //Rotation
        if (movement.magnitude > 0)
        {
            var quat = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, -gravityDirection),
                playerTurnSpeed * Time.deltaTime);
            myBody.MoveRotation(quat);
        }

    }
}