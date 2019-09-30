using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Aspects")] [SerializeField] private AspectMaterial aspectMaterial;
    [SerializeField] private List<AspectTypes> additionalAspects;

    private List<AspectTypes> aspects;

    private void Awake()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectTypes>();

        SetActiveAspects();
        UpdateAspectComponents();
    }

    private void UpdateAspectComponents()
    {
        foreach (AspectTypes aspect in aspects)
        {
            if (aspects.Contains(aspect))
            {
                Type aspectComponent = Type.GetType(aspect.ToString());
                if (aspectComponent == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("The following component " +
                                   aspect + " is null and cannot be added to " + gameObject.name);
#endif
                }
                else
                {
                    if (!gameObject.GetComponent(aspectComponent)) gameObject.AddComponent(aspectComponent);
                }
            }
        }

        List<Aspects> componentList = gameObject.GetComponents<Aspects>().ToList();
        foreach (AspectTypes aspect in aspects)
        {
            for (int i = 0; i < componentList.Count; i++)
            {
                if (Type.GetType(aspect.ToString()) == componentList[i].GetType())
                {
                    //good 
                    componentList.Remove(componentList[i]);
                }
            }
        }

        if (componentList.Count > 0)
        {
            foreach (Aspects aspect in componentList)
            {
                if (Application.isEditor) //Removes component in editor
                {
                    EditorApplication.delayCall += () =>
                    {
                        DestroyImmediate(gameObject.GetComponent(aspect.GetType()));
                    };

                    Debug.Log("The following component " +
                              aspect + " on " + gameObject.name + " was removed");
                }
                else //Removes component in play mode
                    DestroyImmediate(gameObject.GetComponent(aspect.GetType()));
            }
        }
    }


    private void SetActiveAspects()
    {
        List<AspectTypes> tempAspects = new List<AspectTypes>();
        if (aspectMaterial)
        {
            foreach (AspectTypes aspect in aspectMaterial.AspectTypes)
            {
                if (!tempAspects.Contains(aspect))
                    tempAspects.Add(aspect);
            }
        }

        if (additionalAspects.Count >= 1)
        {
            foreach (AspectTypes aspect in additionalAspects)
            {
                if (!tempAspects.Contains(aspect))
                    tempAspects.Add(aspect);
            }
        }

        aspects = tempAspects;
    }


    private void OnValidate()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectTypes>();

        SetActiveAspects();
        UpdateAspectComponents();
    }

    public void AddAspect(AspectTypes aspectType)
    {
        if (additionalAspects == null) additionalAspects = new List<AspectTypes>();
        additionalAspects.Add(aspectType);
        SetActiveAspects();
        UpdateAspectComponents();
    }
}