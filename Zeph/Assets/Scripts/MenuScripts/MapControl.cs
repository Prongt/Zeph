using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapControl : MonoBehaviour
{
    [Header("Buttons/Levels")]
    [SerializeField] private List<Button> leftColumn = default;
    [SerializeField] private List<Button> rightColumn = default;

    private GameObject selector;
    
    // Start is called before the first frame update
    void Start()
    {
        selector = new GameObject("Selector");
        if (leftColumn != null)
        {
            selector.transform.position = leftColumn[0].transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        //I honestly have no idea why this works properly
        if (Input.GetAxis("Horizontal") > 0)
        {
            selector.transform.position = rightColumn[0].transform.position;
        }
        
        if (selector.transform.position == leftColumn[0].transform.position)
        {
            leftColumn[0].Select();
        }
    }
}
