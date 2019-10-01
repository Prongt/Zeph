using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Aspect material used by this object")] [Header("Aspects")] [SerializeField]
    private AspectMaterial aspectMaterial;

    [Tooltip("Any additional aspects this object requires")] [SerializeField]
    private List<AspectType> additionalAspects;

    private List<AspectType> aspects;

    private void Awake()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();

        SetActiveAspects();
        UpdateAspectComponents();
    }

    #region AspectComponents

    /// <summary>
    /// Adds or removes components depending on if they exist in the aspect list
    /// </summary>
    private void UpdateAspectComponents()
    {
        //Add components
        foreach (AspectType aspect in aspects)
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

        //Checks if components are still being used
        List<Aspects> componentList = gameObject.GetComponents<Aspects>().ToList();
        foreach (AspectType aspect in aspects)
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

        //Remove unused components
        if (componentList.Count > 0)
        {
            foreach (Aspects aspect in componentList)
            {
//                if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
//                {
//                    PrefabUtility.ApplyRemovedComponent(gameObject, aspect.GetType(), )
//                }
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

    /// <summary>
    /// Adds all aspects to the aspects list
    /// </summary>
    private void SetActiveAspects()
    {
        List<AspectType> tempAspects = new List<AspectType>();
        if (aspectMaterial)
        {
            foreach (AspectType aspect in aspectMaterial.AspectTypes)
            {
                if (!tempAspects.Contains(aspect))
                    tempAspects.Add(aspect);
            }
        }

        if (additionalAspects.Count >= 1)
        {
            foreach (AspectType aspect in additionalAspects)
            {
                if (!tempAspects.Contains(aspect))
                    tempAspects.Add(aspect);
            }
        }

        aspects = tempAspects;
    }


    private void OnValidate()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();

        SetActiveAspects();
        UpdateAspectComponents();
    }

    /// <summary>
    /// Externally add aspect to this gameobject
    /// </summary>
    /// <param name="aspectType"></param>
    public void AddAspect(AspectType aspectType)
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();
        additionalAspects.Add(aspectType);
        SetActiveAspects();
        UpdateAspectComponents();
    }

    #endregion


    /// <summary>
    /// makes gameobject interact with a defined element
    /// </summary>
    /// <param name="element"></param>
    public void ApplyElement(Element element)
    {
        List<Aspects> components = GetComponents<Aspects>().ToList();

        foreach (Aspects component in components)
        {
            //promotes list
            foreach (AspectType aspect in element.Promotes)
            {
                if (component.AspectType == aspect)
                {
                    component.Promote();
                    //components.Remove(component);
                }
            }

            //negates list
            foreach (AspectType aspect in element.Negates)
            {
                if (component.AspectType == aspect)
                {
                    component.Negate();
                    //components.Remove(component);
                }
            }
        }
    }
}