using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestElement : MonoBehaviour
{
    [SerializeField] private Element element;

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.GetComponent<Interactable>())
        {
            Interactable thing = other.collider.GetComponent<Interactable>();
            thing.ApplyElement(element);
        }
    }
}