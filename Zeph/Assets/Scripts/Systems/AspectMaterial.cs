using System;
using UnityEngine;

[CreateAssetMenu(fileName = "AspectMaterial", menuName = "Aspects/Material", order = 2)]
public class AspectMaterial : ScriptableObject
{
    [SerializeField] private AspectType[] aspectTypes;

    public AspectType[] AspectTypes => aspectTypes;

    private void OnValidate()
    {
        var objects = FindObjectsOfType<Interactable>();
        for (int i = 0; i < objects.Length; i++)
        {
            objects[i].Invoke("SetActiveAspects", 0f);
        }
    }
}