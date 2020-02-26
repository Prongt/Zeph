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
      myCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
   }
   public void FlipX()
   {
      Vector3 t = myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
      t.x = -t.x;
      myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = t;
      hasFlippedX = true;
   }

   public void FlipZ()
   {
      Vector3 t = myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
      t.z = -t.z;
      myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = t;

      GameObject.Find("Zeph").GetComponent<PlayerMove>().flipMovement = true;
      hasFlippedZ = true;
   }
   
   void OnTriggerEnter(Collider other) {

      if (gameObject.CompareTag("Flip"))
      {
         if (other.CompareTag("Player"))
         {
            if (hasFlippedZ)
            {
               FlipZ();
               GameObject.Find("Zeph").GetComponent<PlayerMove>().flipMovement = false;
               hasFlippedZ = false;
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
