using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFlip : MonoBehaviour
{
   public CinemachineVirtualCamera myCam;
   public static bool hasFlippedZ;
   public static bool hasFlippedX;


   void Start()
   {
      //Finds the main Virtual Camera in the scene
      myCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();

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

      GameObject.Find("Zeph").GetComponent<PlayerMove>().flipMovement = true;
      hasFlippedZ = !hasFlippedZ;
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
               GameObject.Find("Zeph").GetComponent<PlayerMove>().flipMovement = false;
               hasFlippedZ = !hasFlippedZ;
            }
            if (hasFlippedX)
            {
               FlipX();
               GameObject.Find("Zeph").GetComponent<PlayerMove>().flipMovement = false;
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
