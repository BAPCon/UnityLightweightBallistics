using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lwdata
{

}


public class _data_9mm_glock
{
    public _data_9mm_glock()
    {

    }

    public static float DropFormula(float position)
    {
        float result = (.00181818f * Mathf.Pow(position, 2)) - (0.0194545f * position) + .0636f;
        return result * -1f;
    }
    public static float VelocityFormula(float position)
    {
        float result = (.00181818f * Mathf.Pow(position, 2)) - (0.0194545f * position) + .0636f;
        return result * -1f;
    }
}
[System.Serializable]
public class DataPoints
{
    //employees is case sensitive and must match the string "employees" in the JSON.
    public float[] datarows;
}


