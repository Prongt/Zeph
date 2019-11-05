using System;
using Unity.Mathematics;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody myBody;


    private Vector3 forward;
    private Vector3 right;
    private Vector3 movement;

    [Tooltip("Speed of Player")] [SerializeField]
    private FloatReference playerSpeed;

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
        if (!GravityDistortion.useNewGravity)
        {
            Move();

        }
        else
        {
            AltMove();
        }
    }

        private void AltJump()
        {

        }

        private void Move()
        {
            Ray ray = new Ray(transform.position, -transform.up);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, hoverHeight))
            {
                float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                Vector3 appliedHoverForce = Vector3.up * proportionalHeight * hoverForce;
                myBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
            }



            movement = new Vector3();
            movement += right * (Input.GetAxis("Horizontal"));
            movement += forward * (Input.GetAxis("Vertical"));
            movement = Vector3.Normalize(Time.deltaTime * movement) * playerSpeed.Value;

            myBody.AddForce(movement);

            Vector3 heading = Vector3.Normalize(movement);
            Vector3 lerpForward = math.lerp((float3) transform.forward, (float3) heading,
                Time.deltaTime * playerTurnSpeed.Value);
            transform.forward = new Vector3(lerpForward.x, 0, lerpForward.z);
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




        private void AltMove()
        {
            Ray ray = new Ray(transform.position, Physics.gravity);
            Debug.DrawRay(transform.position, Physics.gravity);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, hoverHeight))
            {
                float proportionalHeight = (hoverHeight - hit.distance) / hoverHeight;
                Vector3 appliedHoverForce = hoverForce * proportionalHeight * -Physics.gravity;
                myBody.AddForce(appliedHoverForce, ForceMode.Acceleration);
            }



            movement = new Vector3(0,Input.GetAxis("Vertical"),0);
            movement += right * (Input.GetAxis("Horizontal"));
            //movement += forward * (Input.GetAxis("Vertical"));
            movement = Vector3.Normalize(Time.deltaTime * movement) * playerSpeed.Value;

            myBody.AddForce(movement);

            
            Vector3 heading = Vector3.Normalize(movement);
            Vector3 lerpForward = math.lerp((float3) transform.forward, (float3) heading,
                Time.deltaTime * playerTurnSpeed.Value);
            transform.forward = new Vector3(lerpForward.x, 0, lerpForward.z);
        }
    
}
