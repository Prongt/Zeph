using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeRift : Activator
{
    [SerializeField] private List<GameObject> fadeGameobjects = new List<GameObject>();
    public override void Activate()
    {
        base.Activate();
        for (int i = 0; i < fadeGameobjects.Count; i++)
        {
            if (fadeGameobjects[i])
            {
                var isActive = fadeGameobjects[i].activeSelf;
                
                fadeGameobjects[i].SetActive(!isActive);
            }
        }
    }
}
