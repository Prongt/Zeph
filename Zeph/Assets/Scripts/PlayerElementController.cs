using System;
using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Jobs;

public class PlayerElementController : MonoBehaviour
{
    public PlayerElementData[] elementData;
    [SerializeField] private float height = 1;
    [SerializeField] private bool drawGizmos = false;
    [HideIf("drawGizmos", true)][SerializeField] private float gizmoHeight = 1.0f;
    private new Light light;

    private Animator animator;
    //private bool usedPower = false;

    [SerializeField] private VisualEffect fireEffect;
    [SerializeField] private VisualEffect leafEffect;
    //private static readonly int usePower = Animator.StringToHash("usePower");

    //number of collisions detected for each element
    private const int MaxAffectableObjects = 25;

   

    private void Awake()
    {
        light = GetComponentInChildren<Light>();
        animator = GetComponentInChildren<Animator>();
        
        for (int i = 0; i < elementData.Length; i++)
        {
            elementData[i].element.colliders = new Collider[25];
        }

    }

    private void Update()
    {
        if (light.intensity >= 6)
        {
            light.intensity = Mathf.Lerp(light.intensity, 5, 0.5f * Time.deltaTime);
        }

        UsePowers();
    }

    private void UsePowers()
    {
        if (PlayerMove.PlayerIsGrounded)
        {
            for (int i = 0; i < elementData.Length; i++)
            {
                if (Input.GetButtonDown(elementData[i].element.ButtonName))
                {
                    if (elementData[i].element.ButtonName == "FirePower")
                    {
                        fireEffect.SetInt("Spawn Rate", 1000);
                    } else if (elementData[i].element.ButtonName == "OrbitPower")
                    {
                        leafEffect.SetInt("Spawn Rate", 30);
                    } else if (elementData[i].element.ButtonName == "LightPower")
                    {
                        light.intensity = 100f;
                    }
                    //Trigger Audio Effect
                    var audioEmitter = elementData[i].audioEmitter;
                    if (audioEmitter)
                    {
                        if (audioEmitter.IsPlaying())
                        {
                            audioEmitter.Stop();
                        }
                        audioEmitter.Play();
                    }
                    
                    
                    StartCoroutine(UsePowerAnimation());
                    elementData[i].element.colliders = new Collider[MaxAffectableObjects];
                    Physics.OverlapSphereNonAlloc(transform.position, elementData[i].element.PlayerRange,
                        elementData[i].element.colliders);
                    for (int j = 0; j < elementData[i].element.colliders.Length; j++)
                    {
                        var collisionObj = elementData[i].element.colliders[j];

                        if (collisionObj)
                        {
                            var obj = collisionObj.GetComponent<Interactable>();

                            if (obj)
                            {
                                var position = transform.position;
                                var nearestPoint = collisionObj.ClosestPoint(position);
                                Vector3 dir = nearestPoint - position;

                                Physics.Raycast(position, dir, out RaycastHit hitInfo, elementData[i].element.PlayerRange,
                                    LayerMask.NameToLayer("Player"));

                                if (hitInfo.collider == collisionObj)
                                {
                                    float playerY = transform.position.y;

                                    if (Mathf.Abs(nearestPoint.y - playerY) < height)
                                    {
                                        obj.ApplyElement(elementData[i].element, gameObject.transform, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    
    IEnumerator UsePowerAnimation()
    {
        animator.SetBool("usePower", true);
        yield return new WaitForSeconds(1f);
        animator.SetBool("usePower", false);
        fireEffect.SetInt("Spawn Rate", 0);
        leafEffect.SetInt("Spawn Rate",0);
    }
    
    

    private void OnDrawGizmos()
    {
        if (drawGizmos == false)
        {
            return;
        }

        var position = transform.position;
        Vector3 topGizmo = position;
        topGizmo.y += height;

        Vector3 bottomGizmo = position;
        bottomGizmo.y -= height;

        DrawGizmosAtHeight(topGizmo);
        DrawGizmosAtHeight(bottomGizmo);
    }

    private void DrawGizmosAtHeight(Vector3 position)
    {
        for (int i = 0; i < elementData.Length; i++)
        {
            Gizmos.color = elementData[i].element.DebugColor;
            const float theta = 0;
            float x = elementData[i].element.PlayerRange * Mathf.Cos(theta);
            float y = elementData[i].element.PlayerRange * Mathf.Sin(theta);
            Vector3 pos = position + new Vector3(x, gizmoHeight, y);
            Vector3 lastPos = pos;
            for (float thetaLoop = 0.1f; thetaLoop < Mathf.PI * 2; thetaLoop += 0.1f)
            {
                x = elementData[i].element.PlayerRange * Mathf.Cos(thetaLoop);
                y = elementData[i].element.PlayerRange * Mathf.Sin(thetaLoop);
                var newPos = position + new Vector3(x, 0, y);
                Gizmos.DrawLine(pos, newPos);
                pos = newPos;
            }

            Gizmos.DrawLine(pos, lastPos);
        }
    }
    
    [Serializable]
    public struct PlayerElementData
    {
        public Element element;
        public StudioEventEmitter audioEmitter;
    }
}





