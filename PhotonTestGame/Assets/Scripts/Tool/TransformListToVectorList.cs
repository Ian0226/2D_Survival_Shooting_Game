using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class TransformListToVectorList
{
    public static List<Vector3> ListConvert(List<Transform> transformList)
    {
        List<Vector3> vector2List = new List<Vector3>();
        foreach(Transform transform in transformList)
        {
            if (transform != null)
                vector2List.Add(transform.position);
        }
        return vector2List;
    }
}
