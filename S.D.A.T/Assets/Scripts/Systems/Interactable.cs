using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    [Header("Aspects")] [SerializeField] private AspectMaterial aspectMaterial;
    [SerializeField] private List<AspectType> additionalAspects;

    private List<AspectType> aspects;

    private void Awake()
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();

        SetActiveAspects();
        UpdateAspectComponents();
        
    }

    #region AspectComponents
private void UpdateAspectComponents()
    {
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

    public void AddAspect(AspectType aspectType)
    {
        if (additionalAspects == null) additionalAspects = new List<AspectType>();
        additionalAspects.Add(aspectType);
        SetActiveAspects();
        UpdateAspectComponents();
    }
    

    #endregion


    public void TakeInElement(Element element)
    {
        //var components = GetComponents<Aspects>();
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

    public Type GetComponentByAspect(AspectType aspectToCall) //TODO cache components to increase performance
    {
        foreach (AspectType aspect in aspects)
        {
            if (aspect == aspectToCall)
            {
                Type aspectComponent = Type.GetType(aspectToCall.ToString());
                
                if (aspectComponent != null)
                {
                    //component type is valid
                    
                    Component component = GetComponent(aspectComponent);
                    
                    if (component != null)
                    {
                        //Component found on gameobject
                        //Returns component type
                        return aspectComponent;
                    }
                }
            }
        }
        
        //If aspect component is not found on the gameobject
        return null;
    }
    
    /*
     * Take in element
     * parameter a list of elements
     * loop through the elements apects and call promote or negate if there is a relevant component attached
     store list of aspect components
     */

}