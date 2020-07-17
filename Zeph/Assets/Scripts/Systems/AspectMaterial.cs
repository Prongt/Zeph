using UnityEngine;

/// <summary>
/// Scriptable object that defines all the aspect types an object should contain
/// </summary>
[CreateAssetMenu(fileName = "AspectMaterial", menuName = "Aspects/Material", order = 2)]
public class AspectMaterial : ScriptableObject
{
    [SerializeField] private AspectType[] aspectTypes = default;

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