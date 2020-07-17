using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Editor window that replaces a list of gameobjects with a chosen prefab
/// </summary>
public class GameObjectReplacer : ScriptableWizard
{
    public bool copyName = true;
    public List<GameObject> objectsToReplace;
    public GameObject prefab;

    [MenuItem("Custom/Replace GameObjects %q")]
    private static void CreateWizard()
    {
        DisplayWizard("Replace GameObjects", typeof(GameObjectReplacer), "Replace");
    }

    private void OnWizardCreate()
    {
        var transformsToReplace = new List<Transform>();
        // objectsToReplace.AddRange(Selection.transforms);

        foreach (var obj in objectsToReplace) transformsToReplace.Add(obj.transform);

        foreach (var t in transformsToReplace)
        {
            var instantiatePrefab = (GameObject) PrefabUtility.InstantiatePrefab(prefab);

            instantiatePrefab.transform.position = t.position;
            instantiatePrefab.transform.rotation = t.rotation;
            instantiatePrefab.transform.parent = t.parent;

            if (copyName) instantiatePrefab.name = t.name;


            DestroyImmediate(t.gameObject);
        }
    }
}