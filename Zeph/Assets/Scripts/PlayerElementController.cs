using System;
using System.Collections;
using FMODUnity;
using UnityEngine;
using UnityEngine.VFX;


public class PlayerElementController : MonoBehaviour
{
    public PlayerElementData[] elementData;
    [SerializeField] private float height = 1;
    [SerializeField] private bool drawGizmos;
    [HideIf("drawGizmos", true)][SerializeField] private float gizmoHeight = 1.0f;
    private new Light light;

    private Animator animator;

    [SerializeField] private VisualEffect fireEffect;
    [SerializeField] private VisualEffect leafEffect;

    [SerializeField] private LayerMask layerMask;

    //number of collisions detected for each element
    private const int MaxAffectableObjects = 25;

    private static readonly int usePower = Animator.StringToHash("usePower");


    private void Awake()
    {
        light = GetComponentInChildren<Light>();
        animator = GetComponentInChildren<Animator>();

        for (var i = 0; i < elementData.Length; i++)
        {
            elementData[i].element.colliders = new Collider[25];
        }

    }

    private void Update()
    {
        if (light.intensity >= 95f)
        {
//            print("Intensity = " + light.intensity);
            light.intensity = Mathf.Lerp(light.intensity, 94, 0.5f * Time.deltaTime);
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
                    break;
                case "OrbitPower":
                    //Debug.Log("Orbit power used");
                    leafEffect.SetInt("Spawn Rate", 30);
                    break;
                case "LightPower":
                    //Debug.Log("Light power used");
                    light.intensity = 1200f;
                    break;
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
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var j = 0; j < elementData[i].element.colliders.Length; j++)
            {
                var collisionObj = elementData[i].element.colliders[j];

                if (!collisionObj) continue;
                    
                var obj = collisionObj.GetComponent<Interactable>();

                if (!obj) continue;
                    
                var position = transform.position;
                var nearestPoint = collisionObj.ClosestPoint(position);
                var dir = nearestPoint - position;

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
        }
    }
    
    
    IEnumerator UsePowerAnimation()
    {
        animator.SetBool(usePower, true);
        yield return new WaitForSeconds(1f);
        animator.SetBool(usePower, false);
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





