using System;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using Unity.Mathematics;
using UnityEngine;

public class PlayerElementController : MonoBehaviour
{
    public Element[] elementData;
    [SerializeField] private float height = 1;
    [SerializeField] private bool drawGizmos = false;
    [HideIf("drawGizmos", true)][SerializeField] private float gizmoHeight;
    [SerializeField] private KeyCode powerKey;
    private Light light;
    public bool powerUsed;
    private bool routineRunning;


    //number of collisions detected for each element
    private const int maxAffectableObjects = 25;

   

    private void Awake()
    {
        light = GetComponentInChildren<Light>();
        
        for (int i = 0; i < elementData.Length; i++)
        {
            elementData[i].colliders = new Collider[25];
        }

        powerUsed = false;
        routineRunning = false;
    }

    private void Update()
    {
        if (light.intensity >= 6)
        {
            light.intensity = Mathf.Lerp(light.intensity, 5, 0.5f * Time.deltaTime);
        }
        
        if (PlayerMove.PlayerIsGrounded)
        {
            for (int i = 0; i < elementData.Length; i++)
            {
                if (Input.GetButtonDown(elementData[i].ButtonName))
                {
                    //Debug.Log(elementData[i].name);
                    elementData[i].colliders = new Collider[maxAffectableObjects];
                    Physics.OverlapSphereNonAlloc(transform.position, elementData[i].PlayerRange,
                        elementData[i].colliders);
                    for (int j = 0; j < elementData[i].colliders.Length; j++)
                    {
                        var collisionObj = elementData[i].colliders[j];

                        if (collisionObj)
                        {
                            var obj = collisionObj.GetComponent<Interactable>();
                            if (obj)
                            {
                                var nearestPoint = collisionObj.ClosestPoint(transform.position);
                                //Debug.Log(nearestPoint);
                                Vector3 dir = nearestPoint - transform.position;;
                                RaycastHit hitInfo;
                                
                                 
                                Physics.Raycast(transform.position, dir, out hitInfo, elementData[i].PlayerRange, LayerMask.NameToLayer("Player"));

                                if (hitInfo.collider == collisionObj)
                                {
                                    float playerY = transform.position.y;

                                    if (Mathf.Abs(nearestPoint.y - playerY) < height)
                                    {
                                        obj.ApplyElement(elementData[i], gameObject.transform);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
        
        //CombinedPowers();
    }

    private void CombinedPowers()
    {
//if (Input.GetKeyDown(powerKey) && !powerUsed && PlayerMove.IsGrounded)
        //if (Input.GetKeyDown(powerKey) )
        if (Input.GetButtonDown("FirePower") && PlayerMove.PlayerIsGrounded)
        {
            powerUsed = true;
            StartCoroutine(Delay());
            light.intensity = 100;

            if (!routineRunning)
            {
                //StartCoroutine(LightFade());
            }

            for (int i = 0; i < elementData.Length; i++)
            {
                elementData[i].colliders = new Collider[maxAffectableObjects];
                var size = Physics.OverlapSphereNonAlloc(transform.position, elementData[i].PlayerRange,
                    elementData[i].colliders);
                for (int j = 0; j < elementData[i].colliders.Length; j++)
                {
                    var collisionObj = elementData[i].colliders[j];

                    if (collisionObj)
                    {
                        var obj = collisionObj.GetComponent<Interactable>();
                        if (obj)
                        {
                            float objY = elementData[i].colliders[j].ClosestPoint(transform.position).y;
                            //float objY = obj.transform.position.y;
                            float playerY = transform.position.y;

                            if (Mathf.Abs(objY - playerY) < height)
                            {
                                obj.ApplyElement(elementData[i], gameObject.transform);
                            }
                        }
                    }
                }
            }
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(3f);
        powerUsed = false;
    }

    private void OnDrawGizmos()
    {
        if (drawGizmos == false)
        {
            return;
        }

        Vector3 topGizmo = transform.position;
        topGizmo.y += height;

        Vector3 bottomGizmo = transform.position;
        bottomGizmo.y -= height;

        DrawGizmosAtHeight(topGizmo);
        DrawGizmosAtHeight(bottomGizmo);
    }

    private void DrawGizmosAtHeight(Vector3 position)
    {
        for (int i = 0; i < elementData.Length; i++)
        {
            Gizmos.color = elementData[i].DebugColor;
            float theta = 0;
            float x = elementData[i].PlayerRange * Mathf.Cos(theta);
            float y = elementData[i].PlayerRange * Mathf.Sin(theta);
            Vector3 pos = position + new Vector3(x, gizmoHeight, y);
            Vector3 newPos = pos;
            Vector3 lastPos = pos;
            for (float thetaLoop = 0.1f; thetaLoop < Mathf.PI * 2; thetaLoop += 0.1f)
            {
                x = elementData[i].PlayerRange * Mathf.Cos(thetaLoop);
                y = elementData[i].PlayerRange * Mathf.Sin(thetaLoop);
                newPos = position + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }

            Gizmos.DrawLine(pos, lastPos);
            //Gizmos.DrawWireSphere(transform.position, elementData[i].Range);
        }
    }
}



