using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerElementController : MonoBehaviour
{
    public float[] elementRadius;
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Transform t = GetComponent<Transform>();
        float theta = 0;
        float x = elementRadius[0] * Mathf.Cos(theta);
        float y = elementRadius[0] * Mathf.Sin(theta);
        Vector3 pos= t.position + new Vector3(x,0,y);
        Vector3 newPos= pos;
        Vector3 lastPos= pos;
        for(float thetaLoop = 0.1f; thetaLoop < Mathf.PI*2; thetaLoop += 0.1f){
            x = elementRadius[0] * Mathf.Cos(thetaLoop);
            y = elementRadius[0] * Mathf.Sin(thetaLoop);
            newPos = t.position + new Vector3(x,0,y);
            Gizmos.DrawLine(pos,newPos);
            pos = newPos;
        }
        Gizmos.DrawLine(pos,lastPos);
    }
}
