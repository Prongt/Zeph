using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraFlip : MonoBehaviour
{
   public CinemachineVirtualCamera myCam;
   

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
   }
}
