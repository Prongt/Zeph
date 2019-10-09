using System;
using System.Collections;
using System.Collections.Generic;

using UnityEditor;
using UnityEngine;

public class PlayerElementController : MonoBehaviour
{
    public PlayerElement[] elementData;
    [SerializeField] private bool drawGizmos = false;
    [HideIf("drawGizmos", true)][SerializeField] private float gizmoHeight;
    
    [SerializeField] private KeyCode powerKey;
    

    //number of collisions detected for each element
    private const int maxAffectableObjects = 25;


    private void Awake()
    {
        
        
        for (int i = 0; i < elementData.Length; i++)
        {
            elementData[i].colliders = new Collider[25];
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(powerKey))
        {
            for (int i = 0; i < elementData.Length; i++)
            {
                var size = Physics.OverlapSphereNonAlloc(transform.position, elementData[i].Element.PlayerRange, elementData[i].colliders);
                for (int j = 0; j < elementData[i].colliders.Length; j++)
                {
                    var objec = elementData[i].colliders[j];
                    if (objec)
                    {
                        var obj = objec.GetComponent<Interactable>();
                        if (obj)
                        {
                            obj.ApplyElement(elementData[i].Element);
                        }   
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false)
        {
            return;
        }

        
        for (int i = 0; i < elementData.Length; i++)
        {
            Gizmos.color = elementData[i].Element.DebugColor;
            Transform t = GetComponent<Transform>();
            float theta = 0;
            float x = elementData[i].Element.PlayerRange * Mathf.Cos(theta);
            float y = elementData[i].Element.PlayerRange * Mathf.Sin(theta);
            Vector3 pos= t.position + new Vector3(x,gizmoHeight,y);
            Vector3 newPos= pos;
            Vector3 lastPos= pos;
            for(float thetaLoop = 0.1f; thetaLoop < Mathf.PI*2; thetaLoop += 0.1f){
                x = elementData[i].Element.PlayerRange * Mathf.Cos(thetaLoop);
                y = elementData[i].Element.PlayerRange * Mathf.Sin(thetaLoop);
                newPos = t.position + new Vector3(x,0,y);
                Gizmos.DrawLine(pos,newPos);
                pos = newPos;
            }
            Gizmos.DrawLine(pos,lastPos);
            //Gizmos.DrawWireSphere(transform.position, elementData[i].Range);
        }
    }

}

[Serializable]
public struct PlayerElement
{
    public Element Element;
    [HideInInspector] public Collider[] colliders;
}