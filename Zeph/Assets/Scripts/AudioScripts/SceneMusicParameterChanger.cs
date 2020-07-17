
using FMODUnity;
using UnityEngine;

/// <summary>
/// Tells fmod which scene is currently active
/// </summary>
public class SceneMusicParameterChanger : MonoBehaviour
{
    [SerializeField] [ParamRef] [Tooltip("Level parameter name in Fmod")]
    private string levelParameterName = default;
    
    [SerializeField] [Tooltip("Index of the active level")]
    private int levelIndex = 0;

    private void Start()
    {
        RuntimeManager.StudioSystem.setParameterByName(levelParameterName, levelIndex);
    }
}
