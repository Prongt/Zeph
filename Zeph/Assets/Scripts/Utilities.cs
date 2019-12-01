using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities
{
    public static bool IsLayerInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }
}
