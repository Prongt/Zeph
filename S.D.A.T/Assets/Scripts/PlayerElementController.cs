using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using UnityEngine;

public class PlayerElementController : MonoBehaviour
{
    public PlayerElement[] elementData;
    [SerializeField] private bool drawGizmos = false;
    [SerializeField] private float gizmoHeight;
    
    [SerializeField] private KeyCode powerKey;
    [SerializeField] private ParticleSystem wind;
    
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
            if (!GameObject.Find("Wind(Clone)"))
            {
                Instantiate(wind.gameObject, gameObject.transform);
                
            }
            else if(GameObject.Find("Wind(Clone)"))
            {
                Destroy(GameObject.Find("Wind(Clone)"));
                Instantiate(wind.gameObject, gameObject.transform);
            }
            
            for (int i = 0; i < elementData.Length; i++)
            {
                var size = Physics.OverlapSphereNonAlloc(transform.position, elementData[i].Range, elementData[i].colliders);
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
            Gizmos.color = elementData[i].DebugColor;
            Transform t = GetComponent<Transform>();
            float theta = 0;
            float x = elementData[i].Range * Mathf.Cos(theta);
            float y = elementData[i].Range * Mathf.Sin(theta);
            Vector3 pos= t.position + new Vector3(x,gizmoHeight,y);
            Vector3 newPos= pos;
            Vector3 lastPos= pos;
            for(float thetaLoop = 0.1f; thetaLoop < Mathf.PI*2; thetaLoop += 0.1f){
                x = elementData[i].Range * Mathf.Cos(thetaLoop);
                y = elementData[i].Range * Mathf.Sin(thetaLoop);
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
    public float Range;
    public Color DebugColor;
    [HideInInspector] public Collider[] colliders;
}