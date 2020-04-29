using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;


public class PlayerElementController : MonoBehaviour
{
    public bool BlockPowers = false;

    public PlayerElementData[] elementData;
    [SerializeField] private float height = 1;
    [SerializeField] private bool drawGizmos = false;
    [HideIf("drawGizmos", true)][SerializeField] private float gizmoHeight = 1.0f;
    private new Light light;

    [SerializeField]private Animator animator;

    [SerializeField] private VisualEffect fireEffect = default;
    [SerializeField] private VisualEffect leafEffect = default;

    [SerializeField] private LayerMask layerMask;

    private bool fire;
    public bool orbit;
    private bool lightP;

    //number of collisions detected for each element
    private const int MaxAffectableObjects = 40;

    private static readonly int usePower = Animator.StringToHash("usePower");


    private void Awake()
    {
        light = GetComponentInChildren<Light>();

        for (var i = 0; i < elementData.Length; i++)
        {
            elementData[i].element.colliders = new Collider[MaxAffectableObjects];
        }
    }

    private void Update()
    {
        if (BlockPowers) return;

        if (light)
        {
            if (light.intensity >= 95f)
            {
                light.intensity = Mathf.Lerp(light.intensity, 94, 0.5f * Time.deltaTime);
            }
        }

        UsePowers();
    }

    private void UsePowers()
    {
        for (var i = 0; i < elementData.Length; i++)
        {
            if (!Input.GetButtonDown(elementData[i].element.ButtonName)) continue;
            if (!elementData[i].element.PowerIsEnabled) continue;
            switch (elementData[i].element.ButtonName)
            {
                case "FirePower":
                    //Debug.Log("Fire power used");
                    fireEffect.SetInt("Spawn Rate", 1000);
                    fire = true;
                    break;
                case "OrbitPower":
                    //Debug.Log("Orbit power used");
                    leafEffect.SetInt("Spawn Rate", 30);
                    orbit = true;
                    break;
                case "LightPower":
                    //Debug.Log("Light power used");
                    light.intensity = 1200f;
                    lightP = true;
                    break;
            }

            //Trigger Audio Effect
            var audioEmitter = elementData[i].audioEmitter;
            if (audioEmitter)
            {
                if (!audioEmitter.IsPlaying())
                {
                    audioEmitter.Play();   
                }
            }


            StartCoroutine(UsePowerAnimation());
            elementData[i].element.colliders = new Collider[MaxAffectableObjects];
            Physics.OverlapSphereNonAlloc(transform.position, elementData[i].element.PlayerRange,
                elementData[i].element.colliders);
            
            for (var j = 0; j < elementData[i].element.colliders.Length; j++)
            {
                var collisionObj = elementData[i].element.colliders[j];

                if (!collisionObj) continue;
                    
                var obj = collisionObj.GetComponent<Interactable>();

                if (!obj) continue;
                    
                var position = transform.position;
                var nearestPoint = collisionObj.ClosestPoint(position);
                var dir = nearestPoint - position;
                //Debug.Log("Found");
                if (obj.requireRaycast)
                {
                    Physics.Raycast(position, dir, out RaycastHit hitInfo,
                        elementData[i].element.PlayerRange,
                        layerMask);

                    if (hitInfo.collider != collisionObj) continue;
                   
                    
                    var playerY = transform.position.y;
                    
                    if (Mathf.Abs(nearestPoint.y - playerY) < height)
                    {
                        obj.ApplyElement(elementData[i].element, gameObject.transform, true);
                    }
                }
                else
                {
                    obj.ApplyElement(elementData[i].element, gameObject.transform, true);
                }
            }
        }
    }


    private IEnumerator UsePowerAnimation()
    {
        if (fire)
        {
            animator.SetBool("Fire", true);
            yield return new WaitForSeconds(1f);
            animator.SetBool("Fire", false);
            fire = false;
        } else if (orbit)
        {
            animator.SetBool("Orbit", true);
            yield return new WaitForSeconds(1f);
            animator.SetBool("Orbit", false);
            orbit = false;
        } else if (lightP)
        {
            animator.SetBool("Light", true);
            yield return new WaitForSeconds(1f);
            animator.SetBool("Light", false);
            lightP = false;
        }
        //animator.SetBool(usePower, true);
        //animator.SetBool(usePower, false);
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
        var topGizmo = position;
        topGizmo.y += height;

        var bottomGizmo = position;
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
            var x = elementData[i].element.PlayerRange * Mathf.Cos(theta);
            var y = elementData[i].element.PlayerRange * Mathf.Sin(theta);
            var pos = position + new Vector3(x, gizmoHeight, y);
            var lastPos = pos;
            for (var thetaLoop = 0.1f; thetaLoop < Mathf.PI * 2; thetaLoop += 0.1f)
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





