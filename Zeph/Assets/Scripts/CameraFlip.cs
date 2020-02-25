using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFlip : MonoBehaviour
{
   public CinemachineVirtualCamera myCam;
   public static bool hasFlipped;
   

   void Start()
   {
      myCam = GameObject.Find("CM vcam1").GetComponent<CinemachineVirtualCamera>();
   }
   public void FlipX()
   {
      Vector3 t = myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
      t.x = -t.x;
      myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = t;
   }

   public void FlipZ()
   {
      Vector3 t = myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
      t.z = -t.z;
      myCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = t;

      GameObject.Find("Zeph").GetComponent<PlayerMove>().flipMovement = true;
      hasFlipped = true;
   }
   
   void OnTriggerEnter(Collider other) {
      if (gameObject.CompareTag("Flip"))
      {
         if (other.CompareTag("Player"))
         {
            if (hasFlipped)
            {
               FlipZ();
               GameObject.Find("Zeph").GetComponent<PlayerMove>().flipMovement = false;
               hasFlipped = false;
            }
         }
      }
   }

   void OnCollisionEnter(Collision other)
   {
      if (gameObject.CompareTag("Flip"))
         if (other.gameObject.CompareTag("Player"))
         {
            if (hasFlipped)
            {
               FlipZ();
               hasFlipped = false;
            }
         }
   }
}
