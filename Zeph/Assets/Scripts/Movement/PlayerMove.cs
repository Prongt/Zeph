﻿using System;
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
    
    private float distanceToGround;
    public static bool IsGrounded;

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
//        print(gravityDirection);
        //Rotates players transform to be opposite of direction of gravity
        if (gravityDirection != oldGravity)
        {
            transform.up = new Vector3(0, -(gravityDirection.x + gravityDirection.z) / 2, 0);
            oldGravity = gravityDirection;
        }
        
        Jump();
        Move();
    }


    private void Move()
    {
        var moveSpeed = Time.deltaTime * playerSpeed.Value;

        //Movement
        var movement = new Vector3();

        if (GravityRift.useNewGravity)
        {
            //Movement in Y dir is constant
            movement.y = moveSpeed * Input.GetAxis("Vertical");
            
            //Gravity in X dir;
            if ((gravityDirection.x > 0 || gravityDirection.x < 0) && (gravityDirection.z > 0 || gravityDirection.z < 0))
            {
                print("both dir");
                movement.z = moveSpeed * -Input.GetAxis("Horizontal");
                movement.x = moveSpeed * -Input.GetAxis("Horizontal");
            }
            else if (gravityDirection.x > 0 || gravityDirection.x < 0)
            {
                print("x Dir");
                movement.z = moveSpeed * -Input.GetAxis("Horizontal");
            }
            //Gravity in Z dir
            else if (gravityDirection.z > 0 || gravityDirection.z < 0)
            {
                print("Y dir");
                movement.x = moveSpeed * Input.GetAxis("Horizontal");
            }
            
        }
        else
        {
            movement += right * (moveSpeed * Input.GetAxis("Horizontal"));
            movement += forward * (moveSpeed * Input.GetAxis("Vertical"));
        }

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
        if (CheckIfGrounded())
        {
            IsGrounded = true;
            if (Input.GetButtonDown("Jump"))
            {
                myBody.AddForce(-gravityDirection.normalized * jumpForce.Value, ForceMode.Impulse);
            }
        }
        else
        {
            IsGrounded = false;
        }
        
    }

    private bool CheckIfGrounded()
    {
        return Physics.Raycast(transform.position, gravityDirection, distanceToGround + 0.1f);
    }
}