using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conductor : MonoBehaviour
{
    private List<GameObject> _objectsInRadius;
    private List<SnowController> _snowControllersInRadius;

    private void Awake()
    {
        _objectsInRadius = new List<GameObject>();
        _snowControllersInRadius = new List<SnowController>();
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
