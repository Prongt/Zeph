using Unity.Mathematics;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{

    private Vector3 forwardVector;
    private Vector3 rightVector;

    private Transform cam;
    [SerializeField] private float _speed = 5f;
    [SerializeField] private float _turnSpeed = 10f;
    [SerializeField] private float _gravity = 20f;
    [SerializeField] private float _jumpSpeed = 10f;

    [SerializeField] private bool _jump;


    private Vector2 _input;
    private float _angle;

    private Quaternion _targetRotation;
    private CharacterController _controller;

    private Vector3 movement;

    public bool grounded;
    

    private float distanceToGround;
    
    private Vector3 oldGravity;
    private Vector3 gravityDirection;
    void Start()
    {
        cam = Camera.main.transform;
        forwardVector = Camera.main.transform.forward;
        forwardVector.y = 0;
        forwardVector = Vector3.Normalize(forwardVector);
        rightVector = Quaternion.Euler(new Vector3(0, 90, 0)) * forwardVector;

        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        transform.forward = forwardVector;


        _controller = GetComponent<CharacterController>();
        
        gravityDirection = Physics.gravity;
        oldGravity = gravityDirection;
        
    }

    public float rot;
 

    private void Update()
    {
        _controller.Move(Vector3.forward * 0.00f);
        grounded = CheckIfGrounded();
        _input.x = Input.GetAxis("Horizontal");
        _input.y = Input.GetAxis("Vertical");

        gravityDirection = Physics.gravity;

        if (gravityDirection != oldGravity)
        {
            //transform.up = new Vector3(0, -(gravityDirection.x + gravityDirection.z) / 2, 0);
            //transform.up = -Physics.gravity;
            //transform.up = new Vector3(0, -(Physics.gravity.x + Physics.gravity.z) / 2, 0);
            var newUp = new Vector3(-Physics.gravity.x, -Physics.gravity.y , -Physics.gravity.z );
            transform.up = newUp;
            oldGravity = Physics.gravity;
        }

        
        
        
        
        if (CheckIfGrounded())
        {
            SetMove();
            if (Input.GetButtonDown("Jump"))
            {
                movement.y = _jumpSpeed;
            }
        }
        
        if (_input.magnitude > 0.01f)
        {
            var quat = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, -gravityDirection),
                _turnSpeed * Time.deltaTime);
            quat.x = 0;
            quat.z = 0;
            //transform.rotation = quat;
            var angle1 = Quaternion.Angle(transform.rotation, Quaternion.LookRotation(movement, -gravityDirection));
            var angle2 = Quaternion.Angle(Quaternion.LookRotation(movement, -gravityDirection), transform.rotation);

            //if (angle1 > 0.1f)
            
//                if (angle1 < angle2)
//                {
//                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * angle1 * _turnSpeed);
//                }
//                else
//                {
//                    transform.RotateAround(transform.position, transform.up, Time.deltaTime * -angle1 * _turnSpeed);
//                }
            
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * -angle1 * _turnSpeed);

            
            
            
        }
        movement += Time.deltaTime * _gravity* gravityDirection;
        
        

       

        
        //movement += Time.deltaTime * _gravity* gravityDirection;

        _controller.Move(movement * Time.deltaTime);
    }

    private Vector3 tempMove;
    void SetMove()
    {
        //Vector3 tempMove = new Vector3();
        if (GravityRift.useNewGravity)
        {
            //Movement in Y dir is constant
            tempMove.y = Input.GetAxis("Vertical");
            
            //Gravity in X dir;
            if ((gravityDirection.x > 0 || gravityDirection.x < 0) && (gravityDirection.z > 0 || gravityDirection.z < 0))
            {
               // print("both dir");
                tempMove.z = -Input.GetAxis("Horizontal");
                tempMove.x = -Input.GetAxis("Horizontal");
            }
            else if (gravityDirection.x > 0 || gravityDirection.x < 0)
            {
//                print("x Dir");
                tempMove.z = -Input.GetAxis("Horizontal");
            }
            //Gravity in Z dir
            else if (gravityDirection.z > 0 || gravityDirection.z < 0)
            {
               // print("Y dir");
                tempMove.x = Input.GetAxis("Horizontal");
            }
        }
        else
        {
             tempMove = (rightVector * _input.x) + (forwardVector * _input.y);
             
        }
        
        
        movement = tempMove;
        movement *= _speed;
        
    }


    void ApplyGravity()
    {
        movement.y -= _gravity * Time.deltaTime;
    }



    void Jump()
    {
        
    }
    
    private bool CheckIfGrounded()
    {
        //return Physics.Raycast(transform.position, Physics.gravity, distanceToGround + 0.1f);
        return Physics.Raycast(transform.position, gravityDirection, distanceToGround + 0.1f);
    }
    

}