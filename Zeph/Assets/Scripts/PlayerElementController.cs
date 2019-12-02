using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Experimental.VFX;
using UnityEngine.Jobs;

public class PlayerElementController : MonoBehaviour
{
    public Element[] elementData;
    [SerializeField] private float height = 1;
    [SerializeField] private bool drawGizmos = false;
    [HideIf("drawGizmos", true)][SerializeField] private float gizmoHeight;
    private Light light;

    private Animator animator;
    private bool usedPower = false;

    [SerializeField] private VisualEffect fireEffect;
    [SerializeField] private VisualEffect leafEffect;


    //number of collisions detected for each element
    private const int maxAffectableObjects = 25;

   

    private void Awake()
    {
        light = GetComponentInChildren<Light>();
        animator = GetComponentInChildren<Animator>();
        
        for (int i = 0; i < elementData.Length; i++)
        {
            elementData[i].colliders = new Collider[25];
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
                if (Input.GetButtonDown(elementData[i].ButtonName))
                {
                    if (elementData[i].ButtonName == "FirePower")
                    {
                        fireEffect.SetInt("Spawn Rate", 1000);
                    } else if (elementData[i].ButtonName == "OrbitPower")
                    {
                        leafEffect.SetInt("Spawn Rate", 30);
                    }
                    
                    StartCoroutine(UsePowerAnimation());
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
                                Vector3 dir = nearestPoint - transform.position;
                                ;
                                RaycastHit hitInfo;


                                Physics.Raycast(transform.position, dir, out hitInfo, elementData[i].PlayerRange,
                                    LayerMask.NameToLayer("Player"));

                                if (hitInfo.collider == collisionObj)
                                {
                                    float playerY = transform.position.y;

                                    if (Mathf.Abs(nearestPoint.y - playerY) < height)
                                    {
                                        obj.ApplyElement(elementData[i], gameObject.transform, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }
    }
    
    private void UsePowersWithJobs()
    {
        if (PlayerMove.PlayerIsGrounded)
        {
            for (int i = 0; i < elementData.Length; i++)
            {
                if (Input.GetButtonDown(elementData[i].ButtonName))
                {
                    StartCoroutine(UsePowerAnimation());
                    elementData[i].colliders = new Collider[maxAffectableObjects];
                    Physics.OverlapSphereNonAlloc(transform.position, elementData[i].PlayerRange,
                        elementData[i].colliders);
                    NativeHashMap<int, float3> objectPositions = new NativeHashMap<int, float3>(maxAffectableObjects, Allocator.TempJob);
                    
                    for (int j = 0; j < elementData[i].colliders.Length; j++)
                    {
                        if (elementData[i].colliders[j] != null)
                        {
                            objectPositions.TryAdd(j, elementData[i].colliders[j].ClosestPoint(transform.position));
                        }
                    }

                    var job = new CheckIfInAreaJob()
                    {
                        objectPositions = objectPositions,
                        maxDist = height,
                        playerY = transform.position.y
                    };
                    
                    var handle = job.Schedule(objectPositions.Length, 32);
                    
                    handle.Complete();

                    var list = new List<Collider>();
                    NativeArray<int> indexes = job.objectPositions.GetKeyArray(Allocator.TempJob);
                    for (int j = 0; j < indexes.Length; j++)
                    {
                        list.Add(elementData[i].colliders[indexes[i]]);
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
        }
    }
}

public struct CheckIfInAreaJob : IJobParallelFor
{
    public NativeHashMap<int, float3> objectPositions;
    public float playerY;
    public float maxDist;
    public void Execute(int index)
    {
        if (math.abs(objectPositions[index].y - playerY) < maxDist)
        {
            //Objects[ObjectPositions[index].y] = true;
            
        }
        else
        {
            objectPositions.Remove(index);
            //Objects[ObjectPositions[index].y] = false;
        }
    }
}



