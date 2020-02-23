using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    private float currentTarget;
    [SerializeField] private float growSpeed;
    [SerializeField] private float maxRadius;
    [SerializeField] private float minRadius;
    private List<GameObject> objectsInRadius;
    [SerializeField] private float shrinkSpeed;
    private List<SnowController> snowControllersInRadius;
    private List<IceController> iceControllersInRadius;
    [SerializeField] private float waitTime;

    private void Awake()
    {
        objectsInRadius = new List<GameObject>();
        snowControllersInRadius = new List<SnowController>();
        iceControllersInRadius = new List<IceController>();
    }

    [ContextMenu("Grow")]
    public void Grow()
    {
        currentTarget = maxRadius;
        StopAllCoroutines();
        StartCoroutine(GrowWaitShrinkRoutine(growSpeed, waitTime, shrinkSpeed));
    }


    private IEnumerator GrowWaitShrinkRoutine(float growSpeed, float waitTime, float shrinkSpeed)
    {
        while (math.abs(transform.localScale.x - currentTarget) > 0.25f)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one * currentTarget,
                growSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(waitTime);

        while (math.abs(transform.localScale.x - minRadius) > 0.25f)
        {
            transform.localScale =
                Vector3.Slerp(transform.localScale, Vector3.one * minRadius, shrinkSpeed * Time.deltaTime);
            yield return null;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (objectsInRadius.Contains(other.gameObject)) return;

        var snowController = other.GetComponent<SnowController>();
        if (snowController)
        {
            snowControllersInRadius.Add(snowController);
            snowController.Melt();
        }

        var iceController = other.GetComponent<IceController>();
        if (iceController)
        {
            iceControllersInRadius.Add(iceController);
            iceController.Melt();
        }
        
        objectsInRadius.Add(other.gameObject);
        

        var interactable = other.GetComponent<Interactable>();
        if (interactable)
        {
            Debug.Log(other.name);
            interactable.IsEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!objectsInRadius.Contains(other.gameObject)) return;
        
        var index = objectsInRadius.IndexOf(other.gameObject);
        snowControllersInRadius[index].Freeze();
        snowControllersInRadius.RemoveAt(index);
        iceControllersInRadius[index].Freeze();
        iceControllersInRadius.RemoveAt(index);
        objectsInRadius.Remove(other.gameObject);

        var interactable = other.GetComponent<Interactable>();
        if (interactable) interactable.IsEnabled = false;
    }
}