using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEditor;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    private Rigidbody myBody;
    private float verticalAxis;
    private float horAxis;

    private Vector3 forward;
    private Vector3 right;

    [Tooltip("Speed of Player")] 
    [SerializeField] private FloatReference speed;
    [SerializeField] private FloatReference turnSpeed;
    
    // Start is called before the first frame update
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        myBody = GetComponent<Rigidbody>();
        
        transform.forward = forward;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horAxis = Input.GetAxis("Horizontal");
        
        Vector3 movement = new Vector3(horAxis + verticalAxis,0,verticalAxis - horAxis);

       

        if (verticalAxis > 0 || verticalAxis < 0)
        {
            transform.forward = Vector3.Lerp(transform.forward, forward * verticalAxis, turnSpeed.Value * Time.deltaTime);
        } else if (horAxis > 0 || horAxis < 0)
        {
            transform.forward = Vector3.Lerp(transform.forward, right * horAxis, turnSpeed.Value * Time.deltaTime);
        } 
        
        myBody.AddForce(movement * speed.Value);
   
    }
}
