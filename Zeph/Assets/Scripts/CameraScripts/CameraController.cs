using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Animations;
/// <summary>
/// Confines cinemachine orbit anngles and movement speed
/// </summary>
public class CameraController : MonoBehaviour
{
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    private CinemachineComposer cinemachineComposer;
    private CinemachineOrbitalTransposer cinemachineOrbitalTransposer;
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
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
        cinemachineComposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineComposer>();
        cinemachineOrbitalTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        startAim = cinemachineComposer.m_TrackedObjectOffset;
        
        //This locks the orbit
        cinemachineOrbitalTransposer.m_XAxis.SetThresholds(minValue,maxValue,false);
    }

    void Update()
    {
        if (Input.GetAxis(rightUp) > 0)
        {
            //This is Down, Changes the aim of the cinemachine composer to analog input
            if (cinemachineComposer.m_TrackedObjectOffset.y > -1)
            {
                cinemachineComposer.m_TrackedObjectOffset = new Vector3(cinemachineComposer.m_TrackedObjectOffset.x,
                    cinemachineComposer.m_TrackedObjectOffset.y - (Input.GetAxis(rightUp) * camSpeed) * Time.deltaTime,
                    cinemachineComposer.m_TrackedObjectOffset.z);
            }
        } else if (Input.GetAxis(rightUp) < 0)
        {
            //This is Up
            if (cinemachineComposer.m_TrackedObjectOffset.y < 4)
            {
                cinemachineComposer.m_TrackedObjectOffset = new Vector3(cinemachineComposer.m_TrackedObjectOffset.x,
                    cinemachineComposer.m_TrackedObjectOffset.y - (Input.GetAxis(rightUp) * camSpeed) * Time.deltaTime,
                    cinemachineComposer.m_TrackedObjectOffset.z);
            }
        }
        else
        {
            //This resets
            cinemachineComposer.m_TrackedObjectOffset =
                Vector3.Lerp(cinemachineComposer.m_TrackedObjectOffset, startAim, verticalResetSpeed * Time.deltaTime);
        }
    }
}
