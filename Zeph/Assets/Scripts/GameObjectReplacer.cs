using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameObjectReplacer : ScriptableWizard
{
    public bool copyValues = true;
    public GameObject useGameObject;
    public GameObject[] Replace;

    [MenuItem ("Custom/Replace GameObjects")]


    static void CreateWizard ()
    {
        ScriptableWizard.DisplayWizard("Replace GameObjects", typeof(GameObjectReplacer), "Replace");
    }

    void OnWizardCreate ()
    {
        List<Transform> ObjectsToReplace = new List<Transform>();
        foreach (var obj in Replace)
        {
            ObjectsToReplace.Add(obj.transform);
        }
        //Transform[] ObjectsToReplace;
        //ObjectsToReplace = Replace.GetComponentsInChildren<Transform>();

        foreach (Transform t in ObjectsToReplace)
        {
            GameObject newObject;
            newObject = (GameObject) PrefabUtility.InstantiatePrefab(useGameObject);
            //newObject = (GameObject)EditorUtility.InstantiatePrefab(useGameObject);
            newObject.transform.position = t.position;
            newObject.transform.rotation = t.rotation;
            newObject.transform.parent = t.parent;
            newObject.name = t.name;

            DestroyImmediate(t.gameObject);

        }
// Transform[] Replaces;
//         Replaces = Replace.GetComponentsInChildren<Transform>();
//
//         foreach (Transform t in Replaces)
//         {
//             GameObject newObject;
//             newObject = (GameObject) PrefabUtility.InstantiatePrefab(useGameObject);
//             //newObject = (GameObject)EditorUtility.InstantiatePrefab(useGameObject);
//             newObject.transform.position = t.position;
//             newObject.transform.rotation = t.rotation;
//             newObject.transform.parent = t.parent;
//             newObject.name = t.name;
//
//             DestroyImmediate(t.gameObject);
//
//         }

    }
}
