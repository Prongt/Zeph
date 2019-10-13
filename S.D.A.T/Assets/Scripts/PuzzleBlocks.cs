using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable][CreateAssetMenu()]
public class PuzzleBlocks : ScriptableObject
{
    public List<GameObject> BlockPrefabs = new List<GameObject>();
}
