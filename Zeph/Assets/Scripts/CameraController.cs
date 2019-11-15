using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform player;

    private Vector3 cameraOffset;

    [Range(0.01f, 1.0f)] public float smoothFactor = 1f;
    [SerializeField] private bool lookAtPlayer;
    
    //public FloatReference speed;
    //public FloatReference minDistance;

    void Start()
    {
        cameraOffset = transform.position - player.position;
    }
    
    
    void LateUpdate ()
    {
        Vector3 newPos = player.position + cameraOffset;

        transform.position = Vector3.Slerp(transform.position, newPos, smoothFactor);

        if (lookAtPlayer)
        {
            transform.LookAt(player);
        }
        /*float interpolation = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) > minDistance)
        {
            Vector3 position = this.transform.position;
            position.y = Mathf.Lerp(this.transform.position.y, player.transform.position.y, interpolation);
            position.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x, interpolation);
            position.z = Mathf.Lerp(this.transform.position.z, player.transform.position.z, interpolation);
        
            this.transform.position = position;
        }*/
    }
}
