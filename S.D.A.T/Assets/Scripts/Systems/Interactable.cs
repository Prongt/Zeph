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

    private List<AspectType> aspects;
    private List<Aspects> aspectComponents;

    private void Awake()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();

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
                    if (aspectComponent.Name == "Pushable")
                    {
                        gameObject.AddComponent<Rigidbody>();
                    }
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