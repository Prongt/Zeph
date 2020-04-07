using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations;

public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cCam;
    private CinemachineComposer comp;
    private CinemachineOrbitalTransposer trans;
    private Vector3 startAim;
    
    [Header("Camera Values")]
    [SerializeField] private int camSpeed;
    [SerializeField] private int verticalResetSpeed;
    [SerializeField] private int minValue = -15;
    [SerializeField] private int maxValue = 15;

    private readonly string rightUp = "Right Analog Up";
    private readonly string right = "Right Analog";


    void Start()
    {
        cCam = GetComponent<CinemachineVirtualCamera>();
        comp = cCam.GetCinemachineComponent<CinemachineComposer>();
        trans = cCam.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        startAim = comp.m_TrackedObjectOffset;
        //This locks the orbit
        trans.m_XAxis.SetThresholds(minValue,maxValue,false);
    }

    void Update()
    {
        if (Input.GetAxis(rightUp) > 0)
        {
            //This is Down
            if (comp.m_TrackedObjectOffset.y > -1)
            {
                comp.m_TrackedObjectOffset = new Vector3(comp.m_TrackedObjectOffset.x,
                    comp.m_TrackedObjectOffset.y - (Input.GetAxis(rightUp) * camSpeed) * Time.deltaTime,
                    comp.m_TrackedObjectOffset.z);
            }
        } else if (Input.GetAxis(rightUp) < 0)
        {
            //This is Up
            if (comp.m_TrackedObjectOffset.y < 4)
            {
                comp.m_TrackedObjectOffset = new Vector3(comp.m_TrackedObjectOffset.x,
                    comp.m_TrackedObjectOffset.y - (Input.GetAxis(rightUp) * camSpeed) * Time.deltaTime,
                    comp.m_TrackedObjectOffset.z);
            }
        }
        else
        {
            //This resets
            comp.m_TrackedObjectOffset =
                Vector3.Lerp(comp.m_TrackedObjectOffset, startAim, verticalResetSpeed * Time.deltaTime);
        }
    }
    
    
    /*OLD CODE
     private Transform player;

    [Range(0.0f, 300.0f)] [SerializeField] private float smoothFactor = 300.0f;
    //[Range(0.0f, 30.0f)] [SerializeField] private float rotationFactor = 1f;

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
        if (GravityRift.UseNewGravity)
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
       
    }*/
    
}
