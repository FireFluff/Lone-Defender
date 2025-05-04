using System.Collections.Generic;
using UnityEngine;

namespace _Scripts
{
    public class ObjectPoolInfo
    {
        public string LookUpString;
        public List<GameObject> PooledObjects = new List<GameObject>();

        public ObjectPoolInfo(string poolLookUp)
        {
            LookUpString = poolLookUp;
        }
    }
}
