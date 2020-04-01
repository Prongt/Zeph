using System.Collections;
using System.Collections.Generic;
using FMODUnity;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(StudioEventEmitter))]
public class Conductor : MonoBehaviour
{
    public static Transform GlobalConductor;

    private float currentTarget;
    [SerializeField] private float growSpeed = 0.25f;
    [SerializeField] private float maxRadius = 25f;
    [SerializeField] private float minRadius = 1;
    private List<GameObject> objectsInRadius;
    [SerializeField] private float shrinkSpeed = 0.1f;
    [SerializeField] private float waitTime = 3;
    private WaitForSeconds waitForSeconds;
    [Header("Fmod")]
    [SerializeField] private StudioEventEmitter conductorEvent;
    [SerializeField] [ParamRef] private string conductorExtentsParam;

    private void Awake()
    {
        objectsInRadius = new List<GameObject>();
        waitForSeconds = new WaitForSeconds(waitTime);
        conductorEvent = GetComponent<StudioEventEmitter>();
    }

    [ContextMenu("Grow")]
    public void Grow()
    {
        GlobalConductor = this.transform;
        currentTarget = maxRadius;
        StopAllCoroutines();
        StartCoroutine(GrowWaitShrinkRoutine(growSpeed, shrinkSpeed));
    }

    private void Update()
    {
        conductorEvent.SetParameter(conductorExtentsParam, transform.localScale.x);
    }

    private IEnumerator GrowWaitShrinkRoutine(float growingSpeed, float shrinkingSpeed)
    {
        if (!conductorEvent.IsPlaying()) conductorEvent.Play();

        while (math.abs(transform.localScale.x - currentTarget) > 0.25f)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one * currentTarget,
                growingSpeed * Time.deltaTime);
            yield return null;
        }

        yield return waitForSeconds;

        while (math.abs(transform.localScale.x - minRadius) > 0.25f)
        {
            transform.localScale =
                Vector3.Slerp(transform.localScale, Vector3.one * minRadius, shrinkingSpeed * Time.deltaTime);
            yield return null;
        }
        conductorEvent.Stop();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (objectsInRadius.Contains(other.gameObject)) return;

        var snowController = other.GetComponent<SnowController>();
        if (snowController)
        {
            snowController.Melt();
        }

        var iceController = other.GetComponent<IceController>();
        if (iceController)
        {
            iceController.Melt();
        }
        
        objectsInRadius.Add(other.gameObject);
            

        var interactable = other.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.isFrozen = false;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!objectsInRadius.Contains(other.gameObject)) return;

        var snowController = other.GetComponent<SnowController>();
        if (snowController)
        {
            snowController.Freeze();
        }

        var iceController = other.GetComponent<IceController>();
        if (iceController)
        {
            iceController.Freeze();
        }
        objectsInRadius.Remove(other.gameObject);

        var interactable = other.GetComponent<Interactable>();
        if (interactable) interactable.isFrozen = true;
    }
}