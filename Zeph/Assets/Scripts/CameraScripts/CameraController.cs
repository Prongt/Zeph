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
    [Tooltip("Speed the camera moves")][SerializeField] private int camSpeed;
    [Tooltip("Speed at which the camera resets from vertical movement")][SerializeField] private int verticalResetSpeed;
    [Tooltip("Minimum angle the camera can orbit to")][SerializeField] private int minValue = -15;
    [Tooltip("Maximum angle the camera can orbit to")][SerializeField] private int maxValue = 15;

    private readonly string rightUp = "Right Analog Up";


    void Start()
    {
        //Getting all the relevant Cinemachine components
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
            //This is Down, Changes the aim of the cinemachine composer to analog input
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
}
