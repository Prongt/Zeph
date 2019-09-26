using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    [Header("ElementalProperties")]
    [SerializeField] private ElementalMaterial elementalMaterial;
    [SerializeField] private MaterialProperties[] additionalProperties;

    private List<MaterialProperties> exposedProperties;

    private void Awake()
    {
        exposedProperties = new List<MaterialProperties>();
        InitalizeProperties();
    }

    private void InitalizeProperties()
    {
        foreach (MaterialProperties props in elementalMaterial.MaterialProperties)
        {
            AddProperties(props);
        }

        foreach (MaterialProperties props in additionalProperties)
        {
            AddProperties(props);
        }
    }

    private void OnValidate()
    {
        InitalizeProperties();
    }

    void AddProperties(MaterialProperties propertyToAdd)
    {
        if (!exposedProperties.Contains(propertyToAdd))
        {
            exposedProperties.Add(propertyToAdd);
        }
    }
}
