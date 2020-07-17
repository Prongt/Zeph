using UnityEngine;

/// <summary>
/// Utilities class with static helper functions
/// </summary>
public class Utilities
{
    public static bool IsLayerInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}
