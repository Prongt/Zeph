using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Transform player;

    [Range(0.0f, 60.0f)] [SerializeField] private float smoothFactor = 1f;
    [Range(0.0f, 30.0f)] [SerializeField] private float rotationFactor = 1f;

    private Transform camMain;
    private Transform camAlt;

    void Start()
    {
        player = FindObjectOfType<PlayerMove>().transform;
        for (int i = 0; i < player.childCount; i++)
        {
            if (player.GetChild(i).CompareTag("Cam/Main"))
            {
                camMain = player.GetChild(i).transform;
            }
            
            if (player.GetChild(i).CompareTag("Cam/Alt"))
            {
                camAlt = player.GetChild(i).transform;
            }
        }

        if (camMain == null || camAlt == null)
        {
            Debug.LogWarning("Camera transforms are null!");
        }
    }
    
    
    void LateUpdate ()
    {
        Vector3 desiredPosition;
        if (GravityRift.useNewGravity)
        { 
            desiredPosition = Vector3.Slerp(transform.position, camAlt.position, smoothFactor * Time.deltaTime);
        }
        else
        {
            desiredPosition =  Vector3.Slerp(transform.position, camMain.position, smoothFactor * Time.deltaTime);
           
        }

        if (desiredPosition.magnitude > 0.1f)
        {
            transform.position = desiredPosition;
        }

        transform.LookAt(player.position);
       
    }
    
}
