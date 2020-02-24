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
    [SerializeField] private float waitTime;

    private void Awake()
    {
        objectsInRadius = new List<GameObject>();
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
            interactable.IsEnabled = true;
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
        if (interactable) interactable.IsEnabled = false;
    }
}