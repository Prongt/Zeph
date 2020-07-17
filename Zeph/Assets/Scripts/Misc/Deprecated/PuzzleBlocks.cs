using System;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Creates a scriptable object to instantiate gameobjects with the puzzle editor window
/// </summary>
[Serializable][CreateAssetMenu()]
public class PuzzleBlocks : ScriptableObject
{
    public List<GameObject> BlockPrefabs = new List<GameObject>();
}
