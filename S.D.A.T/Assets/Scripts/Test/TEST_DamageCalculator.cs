using System;
using UnityEngine;

public class TEST_DamageCalculator
{
    public static int CalculateDamage(int amount, float mitigationPercent)
    {
        float multiplier = 1f - mitigationPercent;
        return Convert.ToInt32(amount * multiplier);
    }

    public static GameObject InstantiateCubeAtPoint(Vector3 position)
    {
        GameObject obj = new GameObject();


        //obj.transform.position = position + new Vector3(1,1,1);
        obj.transform.position = position;
        return obj;
    }
}