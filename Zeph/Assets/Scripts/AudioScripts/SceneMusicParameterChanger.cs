
using FMODUnity;
using UnityEngine;

public class SceneMusicParameterChanger : MonoBehaviour
{
    [SerializeField] [ParamRef] private string levelParameterName = default;
    [SerializeField] private int levelIndex = 0;

    private void Start()
    {
        RuntimeManager.StudioSystem.setParameterByName(levelParameterName, levelIndex);
    }
}
