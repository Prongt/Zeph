using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
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
                var aspectComponent = Type.GetType(aspect.ToString());
                if (aspectComponent == null)
                {
#if UNITY_EDITOR
                    Debug.LogError("The following component " + 
                                     aspect.ToString() + " is null and cannot be added to " + gameObject.name);
#endif
                }
                else
                {
                    if (!gameObject.GetComponent(aspectComponent))
                    {
                        gameObject.AddComponent(aspectComponent);
                    }
                }
            }
        }

        
        List<Aspects> componentList = gameObject.GetComponents<Aspects>().ToList();
        foreach (var aspect in aspects)
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
//            for (int i = 0; i < componentList.Count; i++)
//            {
//                if (Application.isEditor)
//                    //Removes component when playing
//                    UnityEditor.EditorApplication.delayCall+=()=>
//                    {
//                        DestroyImmediate(gameObject.GetComponent(componentList[i].GetType()));
//                    };
//                else
//                    Destroy(gameObject.GetComponent(componentList[i].GetType()));
//                
//                
//            }

            foreach (Aspects aspect in componentList)
            {
//                if (aspects.Contains(aspect.name))
//                var type = Type.GetType(aspect.ToString());
                    
                if (Application.isEditor)
                    //Removes component when playing
                    UnityEditor.EditorApplication.delayCall+=()=>
                    {
                        DestroyImmediate(gameObject.GetComponent(aspect.GetType()));
                    };
//                else
//                    Destroy(gameObject.GetComponent(Type.GetType(aspect.ToString())));



//                if (Application.isPlaying)
//                {
//                    //Destroy(gameObject.GetComponent(Type.GetType(aspect.ToString())));
//                    Destroy(gameObject.GetComponent(aspect.GetType()));
//                    Debug.Log("The following component " + 
//                                     aspect.ToString() + " on " + gameObject.name + " was removed on play");
//                    continue;
//                }
#if UNITY_EDITOR
                
                Debug.LogWarning("The following component " + 
                               aspect.ToString() + " on " + gameObject.name + " will be removed on play as it is no longer required");

                
#endif
            }
            
        }

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
