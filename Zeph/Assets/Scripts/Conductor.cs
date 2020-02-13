using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    [SerializeField] private float minRadius;
    [SerializeField] private float maxRadius;
    [SerializeField] private float growSpeed;
    [SerializeField] private float shrinkSpeed;
    [SerializeField] private float waitTime;
    
    
    private float currentTarget;
    private List<GameObject> _objectsInRadius;
    private List<SnowController> _snowControllersInRadius;

    private void Awake()
    {
        _objectsInRadius = new List<GameObject>();
        _snowControllersInRadius = new List<SnowController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Grow();
        }
    }

    [ContextMenu("Grow")]
    public void Grow()
    {
        currentTarget = maxRadius;
        StopAllCoroutines();
        StartCoroutine(GrowRoutine(growSpeed, waitTime, shrinkSpeed));
    }
    

    private IEnumerator GrowRoutine(float growSpeed, float waitTime, float shrinkSpeed)
    {
        while (Math.Abs(transform.localScale.x - currentTarget) > 0.25f)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one * currentTarget, growSpeed * Time.deltaTime);
            yield return null;
        }

        //Debug.Log("Waiting");
        yield return new WaitForSeconds(waitTime);
        
        while (Math.Abs(transform.localScale.x - minRadius) > 0.25f)
        {
            transform.localScale = Vector3.Slerp(transform.localScale, Vector3.one * minRadius, shrinkSpeed * Time.deltaTime);
            yield return null;
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (_objectsInRadius.Contains(other.gameObject)) return;

        var snowController = other.GetComponent<SnowController>();
        

        if (!snowController) return;
        
        
        _objectsInRadius.Add(other.gameObject);
        _snowControllersInRadius.Add(snowController);
        snowController.Melt();
        
        var interactable = other.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.IsEnabled = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!_objectsInRadius.Contains(other.gameObject)) return;
        
        
        var index = _objectsInRadius.IndexOf(other.gameObject);
        _snowControllersInRadius[index].Freeze();
        _snowControllersInRadius.RemoveAt(index);
        _objectsInRadius.Remove(other.gameObject);
        
        var interactable = other.GetComponent<Interactable>();
        if (interactable)
        {
            interactable.IsEnabled = false;
        }
    }
}
