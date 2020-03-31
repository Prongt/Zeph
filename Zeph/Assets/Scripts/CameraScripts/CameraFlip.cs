using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Movement;

public class CameraFlip : MonoBehaviour
{
   public CinemachineVirtualCamera myCam;
   public static bool hasFlippedZ;
   public static bool hasFlippedX;

   [SerializeField] private ConfineController cameraConfines;
   

   private PlayerMoveRigidbody playerMoveRigidbody;

   void Start()
   {
      //Avoiding Errors
      if (!gameObject.CompareTag("Flip"))
      {
         cameraConfines = null;
      }
      
      playerMoveRigidbody = FindObjectOfType<PlayerMoveRigidbody>();
      

      //Resets the flip checks from scene to scene.
      hasFlippedX = false;
      hasFlippedZ = false;
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

      playerMoveRigidbody.FlipMovement();
      hasFlippedZ = !hasFlippedZ;
   }

   public void OrbitChangePoint()
   {
      myCam.GetCinemachineComponent<CinemachineOrbitalTransposer>().m_Heading.m_HeadingBias = 180;
      playerMoveRigidbody.FlipMovement();
      hasFlippedZ = !hasFlippedZ;
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
               cameraConfines.ChangeZOffset();
               hasFlippedZ = !hasFlippedZ;
            }
            if (hasFlippedX)
            {
               FlipX();
               playerMoveRigidbody.FlipMovement();
               cameraConfines.ChangeXOffset();
               hasFlippedX = false;
               
            }
         }
      }
   }

   //Same as above but for actual collisions as opposed to a trigger.
   void OnCollisionEnter(Collision other)
   {
      if (gameObject.CompareTag("Flip"))
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
