
using FMODUnity;
using UnityEngine;

public class SceneMusicParameterChanger : MonoBehaviour
{
    [SerializeField] [ParamRef] private string levelParameterName;
    [SerializeField] private int levelIndex;

    private void Start()
    {
        RuntimeManager.StudioSystem.setParameterByName(levelParameterName, levelIndex);
    }
}
