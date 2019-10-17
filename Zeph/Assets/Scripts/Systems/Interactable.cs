using System;
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

    private List<AspectType> aspectTypesList;
    private List<Aspects> aspectComponentsList;
    private List<Type> componentsRequiredByAspects;

    private void Awake()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();
        
        if (componentsRequiredByAspects == null) componentsRequiredByAspects = new List<Type>();
        
        SetActiveAspects();
    }

    #region AspectComponents

    /// <summary>
    /// Adds or removes components depending on if they exist in the aspect list
    /// </summary>
    private void UpdateAspectComponents()
    {
        //Add components
        foreach (AspectType aspect in aspectTypesList)
        {
            if (aspectTypesList.Contains(aspect))
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

        foreach (AspectType aspect in aspectTypesList)
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
        
        //Adds all components added by aspects to a global list
        var tempAspects = GetComponents<Aspects>();
        for (int i = 0; i < tempAspects.Length; i++)
        {
            componentsRequiredByAspects.AddRange(tempAspects[i].RequiredComponents());
        }

        //Remove unused components
        if (componentList.Count > 0)
        {
            foreach (Aspects aspect in componentList)
            {
                RemoveComponentByType(aspect.GetType());
            }
        }
        
        aspectComponentsList = GetComponents<Aspects>().ToList();
        //Method call is delayed due to unity delay when destroying components
        Invoke("UpdateRequiredComponents", 0.5f);
    }

    private void RemoveComponentByType(Type type)
    {
        //                if (PrefabUtility.IsPartOfPrefabAsset(gameObject))
//                {
//                    PrefabUtility.ApplyRemovedComponent(gameObject, aspect.GetType(), )
//                }
#if UNITY_EDITOR_WIN
        if (Application.isEditor) //Removes component in editor
        {
            EditorApplication.delayCall += () => { DestroyImmediate(gameObject.GetComponent(type)); };

            Debug.Log("The following component " +
                      type.Name + " on " + gameObject.name + " was removed");
        }
        else //Removes component in play mode
            DestroyImmediate(gameObject.GetComponent(type));
#else
                DestroyImmediate(gameObject.GetComponent(type));
                Debug.Log("Standalone");
#endif
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

        aspectTypesList = tempAspects;
        UpdateAspectComponents();
        
    }

    private void OnValidate()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();
        if (componentsRequiredByAspects == null) componentsRequiredByAspects = new List<Type>();

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

    

    void UpdateRequiredComponents()
    {
        //Gets a list of all required assets
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

        //Adds components required by aspects
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
        }
        
        //Removes all curently used components from the global component list
        componentsRequiredByAspects.RemoveAll(type => typesUsedByAspects.Contains(type));


        //Removes unused components
        foreach (var type in componentsRequiredByAspects)
        {
            RemoveComponentByType(type);
        }
        componentsRequiredByAspects = new List<Type>();
    }
    #endregion
    
    /// <summary>
    /// makes gameobject interact with a defined element
    /// </summary>
    /// <param name="element"></param>
    public void ApplyElement(Element element, Transform source = null)
    {
        if (aspectComponentsList.Count <= 0)
        {
            return;
        }
        
        for (int i = 0; i < aspectComponentsList.Count; i++)
        {
            for (int p = 0; p < element.Promotes.Count; p++)
            {
                if (aspectComponentsList[i].AspectType == element.Promotes[p])
                {
                    aspectComponentsList[i].Promote(source);
                }
            }
            
            for (int n = 0; n < element.Negates.Count; n++)
            {
                if (aspectComponentsList[i].AspectType == element.Negates[n])
                {
                    aspectComponentsList[i].Negate(source);
                }
            }
        }
    }

}