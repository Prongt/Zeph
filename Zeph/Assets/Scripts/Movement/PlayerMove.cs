using Unity.Mathematics;
using UnityEngine;


public class PlayerMove : MonoBehaviour
{

    private Vector3 forwardVector;
    private Vector3 rightVector;
    
    [SerializeField] private FloatReference playerSpeed;
    [SerializeField] private FloatReference playerTurnSpeed;
    [SerializeField] private FloatReference playerGravityScale;
    [SerializeField] private FloatReference playerJumpForce;
    
    
    private float desiredAngle;

    private Quaternion targetRot;
    private CharacterController characterController;

    private Vector3 movement;


    private float distanceToGround;
    
    private Vector3 oldGravity;
    private Vector3 gravityDirection;

    public static bool IsGrounded = false;
    void Start()
    {
        forwardVector = Camera.main.transform.forward;
        forwardVector.y = 0;
        forwardVector = Vector3.Normalize(forwardVector);
        rightVector = Quaternion.Euler(new Vector3(0, 90, 0)) * forwardVector;

        distanceToGround = GetComponent<Collider>().bounds.extents.y;
        transform.forward = forwardVector;


        characterController = GetComponent<CharacterController>();
        
        gravityDirection = Physics.gravity;
        oldGravity = gravityDirection;
        
    }


    private void Update()
    {
        characterController.Move(Vector3.forward * 0.00f);
        IsGrounded = CheckIfGrounded();

        gravityDirection = Physics.gravity;

        if (gravityDirection != oldGravity)
        {
            var newUp = new Vector3(-Physics.gravity.x, -Physics.gravity.y , -Physics.gravity.z );
            transform.up = newUp;
            oldGravity = Physics.gravity;
        }

        
        
        
        
        if (CheckIfGrounded())
        {
            SetMove();
            if (Input.GetButtonDown("Jump"))
            {
                movement.y = playerJumpForce.Value;
            }
        }
        
        if (movement.magnitude > 0.01f)
        {
            var quat = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movement, -gravityDirection),
                playerTurnSpeed.Value * Time.deltaTime);
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
            
                transform.RotateAround(transform.position, transform.up, Time.deltaTime * -angle1 * playerTurnSpeed.Value);

            
            
            
        }
        movement += Time.deltaTime * playerGravityScale.Value * gravityDirection;
        
        

       

        
        //movement += Time.deltaTime * _gravity* gravityDirection;

        characterController.Move(movement * Time.deltaTime);
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
                tempMove.y = Input.GetAxis("Vertical") * playerSpeed.Value;

                Debug.Log("Gravity");
                //tempMove.y *= 2;
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
           // Debug.Log("Norm");
             tempMove = (rightVector * Input.GetAxis("Horizontal")) + (forwardVector * Input.GetAxis("Vertical"));
             
        }
        
        
        movement = tempMove;
        movement *= playerSpeed.Value;
        
    }


    void ApplyGravity()
    {
        movement.y -= playerGravityScale.Value * Time.deltaTime;
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