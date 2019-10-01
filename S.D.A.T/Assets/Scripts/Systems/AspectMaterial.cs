using UnityEngine;

[CreateAssetMenu(fileName = "AspectMaterial", menuName = "Aspects/Material", order = 2)]
public class AspectMaterial : ScriptableObject
{
    [SerializeField] private AspectType[] aspectTypes;

    public AspectType[] AspectTypes => aspectTypes;
}