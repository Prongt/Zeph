using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Interactable : MonoBehaviour
{
    [Header("Aspects")]
    [SerializeField] private AspectMaterial aspectMaterial;
    [SerializeField] private List<AspectTypes> additionalAspects;

    private List<AspectTypes> aspects;

    private void Awake()
    {
        if (additionalAspects == null)
        {
            additionalAspects = new List<AspectTypes>();
        }
        aspects = SetActiveAspects();
        UpdateAspectComponents();
    }

    private void UpdateAspectComponents()
    {
        foreach (AspectTypes aspect in aspects)
        {
            if (aspects.Contains(aspect))
            {
                var type = Type.GetType(aspect.ToString());
                if (type == null)
                {
                    Debug.LogWarning("The following component " + 
                                     aspect.ToString() + " is null and cannot be added to " + gameObject.name);
                }
                else
                {
                    if (!gameObject.GetComponent(type))
                    {
                        gameObject.AddComponent(type);
                    }
                }
            }
        }
        
//        if (aspects.Contains(AspectTypes.Flamable))
//        {
//            var type = Type.GetType(AspectTypes.Flamable.ToString());
//            if (type == null)
//            {
//                Debug.LogWarning(type + " is null and cannot be added to " + gameObject.name);
//            }
//            else
//            {
//                if (!gameObject.GetComponent(type))
//                {
//                    gameObject.AddComponent(type);
//                }
//            }
//        }
    }
    

    private List<AspectTypes> SetActiveAspects()
    {
        List<AspectTypes> tempAspects = new List<AspectTypes>();
        if (aspectMaterial)
        {
            foreach (AspectTypes aspect in aspectMaterial.AspectTypes)
            {
                if (!tempAspects.Contains(aspect))
                {
                    tempAspects.Add(aspect);
                }
            }
        }

        if (additionalAspects.Count >= 1)
        {
            foreach (AspectTypes aspect in additionalAspects)
            {
                if (!tempAspects.Contains(aspect))
                {
                    tempAspects.Add(aspect);
                }
            }
        }

        return tempAspects;
    }
    

    private void OnValidate()
    {
        if (additionalAspects == null)
        {
            additionalAspects = new List<AspectTypes>();
        }
        aspects = SetActiveAspects();
        UpdateAspectComponents();
    }
    
    
}
