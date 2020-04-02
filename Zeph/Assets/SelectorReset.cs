using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorReset : MonoBehaviour
{
    public EventSystem eve;
    
    public void ResetSelectedButton()
    {
        eve.SetSelectedGameObject(null);
    } 
}
