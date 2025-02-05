﻿using UnityEngine;
using Cinemachine;
using Movement;

/// <summary>
/// Ensures the camera follows the player when walking on walls and the ceiling
/// </summary>
public class CameraFlip : MonoBehaviour
{
   public CinemachineVirtualCamera myCam;
   public static bool hasFlippedZ;
   public static bool hasFlippedX;
   public static bool hasChangedOrbitPoint;

   [SerializeField] private CameraConfinesController cameraCameraConfineses;
   

   private PlayerMoveRigidbody playerMoveRigidbody;

   void Start()
   {
      //Avoiding Errors
      if (!gameObject.CompareTag("Flip"))
      {
         cameraCameraConfineses = null;
      }
      
      playerMoveRigidbody = FindObjectOfType<PlayerMoveRigidbody>();
      

      //Resets the flip checks from scene to scene.
      hasFlippedX = false;
      hasFlippedZ = false;
      hasChangedOrbitPoint = false;
   }
   //Flips the Body offset X value. Can be accessed using a promote script
   public void FlipX()
   {
      Vector3 t = myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
      t.x = -t.x;
      myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = t;
      hasFlippedX = !hasFlippedX;
   }
   //Flips the Body offset Z value. Can be accessed using a promote script
   public void FlipZ()
   {
      Vector3 t = myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
      t.z = -t.z;
      myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = t;

      playerMoveRigidbody.SideCamera();
      hasFlippedZ = !hasFlippedZ;
   }

   //Changes the orbit transposer default orbit point to be opposite where it begins
   public void OrbitChangePoint()
   {
      if (hasChangedOrbitPoint)
      {
         ResetOrbit();
         playerMoveRigidbody.SideCamera();
      }
      else
      {
         myCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_HeadingBias = -90;
         playerMoveRigidbody.SideCamera();
         hasChangedOrbitPoint = !hasChangedOrbitPoint;
      }
   }

   public void ResetOrbit()
   {
      myCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_HeadingBias = 0;
      hasChangedOrbitPoint = !hasChangedOrbitPoint;
   }

   public void ChangeYOffset()
   {
      Vector3 followOffset = myCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_FollowOffset;
      myCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_FollowOffset = new Vector3(followOffset.x, 8, followOffset.z);
   }

   //Mainly for the Vert Slice scene/Cave Scene. Flips the camera if the player falls after the camera has previously been flipped.
   void OnTriggerEnter(Collider other) {

      if (gameObject.CompareTag("Flip"))
      {
         if (other.CompareTag("Player"))
         {
            if (hasFlippedZ)
            {
               FlipZ();
               playerMoveRigidbody.FlipMovement();
               cameraCameraConfineses.ChangeZOffset();
               hasFlippedZ = !hasFlippedZ;
            }
            if (hasFlippedX)
            {
               FlipX();
               playerMoveRigidbody.FlipMovement();
               cameraCameraConfineses.ChangeXOffset();
               hasFlippedX = false;
            }

            if (hasChangedOrbitPoint)
            {
               ResetOrbit();
               playerMoveRigidbody.SideCamera();
               cameraCameraConfineses.ChangeZOffset();
               hasChangedOrbitPoint = false;
            }
         }
      }
   }

   //Same as above but for actual collisions as opposed to a trigger.
   void OnCollisionEnter(Collision other)
   {
      if (!gameObject.CompareTag("Flip")) return;

      if (other.gameObject.CompareTag("Player"))
      {
         if (hasFlippedZ)
         {
            FlipZ();
            hasFlippedZ = false;
         }
      }
   }
}
