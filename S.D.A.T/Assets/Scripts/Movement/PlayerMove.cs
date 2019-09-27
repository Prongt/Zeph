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
    [SerializeField]
    public int speed;
    
    
    // Start is called before the first frame update
    void Start()
    {
        forward = Camera.main.transform.forward;
        forward.y = 0;
        forward = Vector3.Normalize(forward);
        right = Quaternion.Euler(new Vector3(0, 90, 0)) * forward;
        myBody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        verticalAxis = Input.GetAxis("Vertical");
        horAxis = Input.GetAxis("Horizontal");
        
        Vector3 movement = new Vector3(horAxis + verticalAxis,0,verticalAxis - horAxis);

        transform.forward = forward;

        if (verticalAxis > 0 || verticalAxis < 0)
        {
            transform.forward = forward * verticalAxis;
        } else if (horAxis > 0 || horAxis < 0)
        {
            transform.forward = right * horAxis;
        } 
        
        myBody.AddForce(movement * speed);
   
    }
}
