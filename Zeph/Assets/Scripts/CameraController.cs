using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public GameObject player;
    
    public FloatReference speed;
    public FloatReference minDistance;
    
    
    void Update () {
        float interpolation = speed * Time.deltaTime;

        if (Vector3.Distance(transform.position, player.transform.position) > minDistance)
        {
            Vector3 position = this.transform.position;
            position.y = Mathf.Lerp(this.transform.position.y, player.transform.position.y, interpolation);
            position.x = Mathf.Lerp(this.transform.position.x, player.transform.position.x, interpolation);
            position.z = Mathf.Lerp(this.transform.position.z, player.transform.position.z, interpolation);
        
            this.transform.position = position;
        }
    }
}
