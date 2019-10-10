using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class print : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        var list1 = new List<int> { 1, 2, 3, 4, 5};
        var list2 = new List<int> { 3, 4, 5, 6, 7 };

        HashSet<int> list1Set = new HashSet<int>(list1); //.net framework 4.7.2 and .net core 2.0 and above otherwise new HashSet(list1)
        list1Set.SymmetricExceptWith(list2);
        var resultList = list1Set.ToList(); //resultList contains 1, 2, 6, 7
        for (int i = 0; i < resultList.Count; i++)
        {
            Debug.Log(resultList[i] + " ");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
