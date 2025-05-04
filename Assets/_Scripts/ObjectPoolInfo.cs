using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolInfo
{
    public string LookUpString;
    public List<GameObject> PooledObjects = new List<GameObject>();

    public ObjectPoolInfo(string poolLookUp)
    {
        LookUpString = poolLookUp;
    }
}
