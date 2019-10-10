﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Tooltip("Aspect material used by this object")] [Header("Aspects")] [SerializeField]
    private AspectMaterial aspectMaterial;

    [Tooltip("Any additional aspects this object requires")] [SerializeField]
    private List<AspectType> additionalAspects;

    private List<AspectType> aspects;
    private List<Aspects> aspectComponents;
    private List<Type> componentsToRemove;

    private void Awake()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();
        
        if (componentsToRemove == null) componentsToRemove = new List<Type>();
        
        SetActiveAspects();
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
//                    if (aspectComponent.Name == "Pushable")
//                    {
//                        gameObject.AddComponent<Rigidbody>();
//                    }

                    
                }
            }
        }

        //List<Type> used = new List<Type>();
        //Checks if components are still being used
        List<Aspects> componentList = gameObject.GetComponents<Aspects>().ToList(); 
        List<string> safeList = new List<string>();
        foreach (AspectType aspect in aspects)
        {
            for (int i = 0; i < componentList.Count; i++)
            {
                if (Type.GetType(aspect.ToString()) == componentList[i].GetType())
                {
                    //good 
                    safeList.Add(componentList[i].name);
                    componentList.Remove(componentList[i]);
                }
            }
        }

        //Remove unused components
        if (componentList.Count > 0)
        {
            foreach (Aspects aspect in componentList)
            {
                
                if (!safeList.Contains(aspect.name))
                {
                    var comps = aspect.RequiredComponents();
                    for (int i = 0; i < comps.Length; i++)
                    {
                        componentsToRemove.Add(comps[i]);
                    }
                }
//                bool shouldRemove = true;
//                for (int i = 0; i < used.Count; i++)
//                {
//                    if (aspect.GetType() == used[i])
//                    {
//                        shouldRemove = false;
//                    }
//                }
//
//                if (shouldRemove)
//                {
//                    var comps = aspect.RequiredComponents();
//                    for (int i = 0; i < comps.Length; i++)
//                    {
//                        componentsToRemove.Add(comps[i]);
//                    }
//                }
                
                
//                if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
//                {
//                    PrefabUtility.ApplyRemovedComponent(gameObject, aspect.GetType(), )
//                }
#if UNITY_EDITOR_WIN
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
                #else
                DestroyImmediate(gameObject.GetComponent(aspect.GetType()));
                Debug.Log("Standalone");
#endif

            }
        }
        
        aspectComponents = GetComponents<Aspects>().ToList();
        UpdateRequiredComponents();
    }
    
    

    /// <summary>
    /// Adds all aspects to the aspects list
    /// </summary>
    public void SetActiveAspects()
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
        UpdateAspectComponents();
        
    }

    private void OnValidate()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();
        if (componentsToRemove == null) componentsToRemove = new List<Type>();

        SetActiveAspects();
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

    void UpdateRequiredComponents()
    {
        var aspectList = GetComponents<Aspects>();
        List<Type> typesUsedByAspects = new List<Type>();
        for (int i = 0; i < aspectList.Length; i++)
        {
            Type[] x = aspectList[i].RequiredComponents();
            for (int j = 0; j < x.Length; j++)
            {
                typesUsedByAspects.Add(x[j]);
            }
        }

        List<Type> tempComponents = new List<Type>();
        for (int i = 0; i < typesUsedByAspects.Count; i++)
        {
            if (GetComponent(typesUsedByAspects[i]))
            {
                //good component previously added
            }
            else
            {
                //add component
                gameObject.AddComponent(typesUsedByAspects[i]);
            }
            tempComponents.Add(typesUsedByAspects[i]);
        }

        foreach (var comp in componentsToRemove)
        {
            if (typesUsedByAspects.Contains(comp))
            {
                //keep component
                //Debug.Log("Hello");
            }
            else
            {
                componentsToRemove.Remove(comp);
            }
        }
        //List<Type> components = new List<Type>();
        


        //Removes un used components
        foreach (var comp in componentsToRemove)
        {
            //Destroy(gameObject.GetComponent(componentsToRemove[i]));
           // Debug.Log("Loop");
            
#if UNITY_EDITOR_WIN
            if (Application.isEditor) //Removes component in editor
            {
                EditorApplication.delayCall += () =>
                {
                    DestroyImmediate(gameObject.GetComponent(comp));
                };

                Debug.Log("The following component " +
                          comp.Name + " on " + gameObject.name + " was removed");
            }
            else //Removes component in play mode
                DestroyImmediate(gameObject.GetComponent(comp));
#else
                DestroyImmediate(gameObject.GetComponent(comp));
                //Debug.Log("Standalone");
#endif
        }

        //components = tempComponents;
    }
    /// <summary>
    /// makes gameobject interact with a defined element
    /// </summary>
    /// <param name="element"></param>
    public void ApplyElement(Element element, Transform source = null)
    {
        if (aspectComponents.Count <= 0)
        {
            return;
        }
        
        for (int i = 0; i < aspectComponents.Count; i++)
        {
            for (int p = 0; p < element.Promotes.Count; p++)
            {
                if (aspectComponents[i].AspectType == element.Promotes[p])
                {
                    aspectComponents[i].Promote(source);
                }
            }
            
            for (int n = 0; n < element.Negates.Count; n++)
            {
                if (aspectComponents[i].AspectType == element.Negates[n])
                {
                    aspectComponents[i].Negate(source);
                }
            }
        }
    }

}