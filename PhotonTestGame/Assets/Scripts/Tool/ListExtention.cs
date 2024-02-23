using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ListExtention
{
    public static List<Transform> RemoveNulls<Transform>(this List<Transform> e)
    {
        e.RemoveAll(x => x == null);
        return e;
    }
}
