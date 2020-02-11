// using System.Collections;
// using System.Collections.Generic;
// using Cinemachine;
// using UnityEngine;
//
// public class Flythrough : MonoBehaviour
// {
//     [SerializeField] private CinemachineVirtualCamera dollyController;
//     private float pos;
//     
//
//     // Start is called before the first frame update
//     void Start()
//     {
//         
//     }
//
//     // Update is called once per frame
//     void Update()
//     {
//         pos = dollyController.GetCinemachineComponent<CinemachineTrackedDolly>().m_PathPosition;
//         if (pos == 12)
//         {
//             StartCoroutine(Delay());
//         }
//     }
//
//     IEnumerator Delay()
//     {
//         yield return new WaitForSeconds(1.5f);
//         gameObject.GetComponent<CameraController>().enabled = true;
//         gameObject.GetComponent<CinemachineBrain>().enabled = false;
//     }
// }
