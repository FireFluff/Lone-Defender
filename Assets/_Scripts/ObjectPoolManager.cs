using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

namespace _Scripts
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static List<ObjectPoolInfo> ObjectPools = new List<ObjectPoolInfo>();
        
        [Foldout("Basic Enemy")]
        [SerializeField] private GameObject basicEnemy;
        [Foldout("Basic Enemy")]
        [SerializeField] private int amountToPoolBe;
        
        [Header("Inspector organizers")]
        [SerializeField] private GameObject emptyHolder;
        private static GameObject _enemiesEmpty;
        private static GameObject _projectilesEmpty;
        private static GameObject _particlesEmpty;
        private static GameObject _noneEmpty;
        
        public enum PoolType
        {
            Enemies,
            Projectiles,
            Particles,
            None
        }

        public static PoolType PoolingType;

        private void Awake()
        {
            SetupEmpties();
        }
        
        private void SetupEmpties()
        {
            _enemiesEmpty = new GameObject("Enemies");
            _enemiesEmpty.transform.SetParent(emptyHolder.transform);
            
            _projectilesEmpty = new GameObject("Projectiles");
            _projectilesEmpty.transform.SetParent(emptyHolder.transform);
            
            _particlesEmpty = new GameObject("Particles");
            _particlesEmpty.transform.SetParent(emptyHolder.transform);
            
            _noneEmpty = new GameObject("None");
            _noneEmpty.transform.SetParent(emptyHolder.transform);
        }

        public static GameObject SpawnFromPool(GameObject objectToSpawn, Vector3 spawnPosition,
            Quaternion spawnRotation, PoolType poolType = PoolType.None)
        {
            ObjectPoolInfo pool = ObjectPools.Find(p => p.LookUpString == objectToSpawn.name);

            if (pool == null)
            {
                pool = new ObjectPoolInfo(objectToSpawn.name);
                ObjectPools.Add(pool);
            }
            
            GameObject parentObject = SetParentObject(poolType);
            GameObject spawnableObject = null;
            if (pool.PooledObjects.Count <= 0)
            {
                spawnableObject = Instantiate(objectToSpawn, spawnPosition, spawnRotation);
                spawnableObject.transform.SetParent(parentObject.transform);
                pool.PooledObjects.Add(spawnableObject);
            }
            foreach (var obj in pool.PooledObjects.Where(obj => !obj.activeInHierarchy))
            {
                spawnableObject = obj;
                spawnableObject.SetActive(true);
                break;
            }
            return spawnableObject;
        }

        private static GameObject SetParentObject(PoolType poolType)
        {
            switch (poolType)
            {
                case PoolType.Enemies:
                    return _enemiesEmpty;
                case PoolType.Projectiles:
                    return _projectilesEmpty;
                case PoolType.Particles:
                    return _particlesEmpty;
                case PoolType.None:
                default:
                    return _noneEmpty;
            }
        }
        
        public static GameObject SpawnFromPool(GameObject objectToSpawn, PoolType poolType = PoolType.None)
        {
            ObjectPoolInfo pool = ObjectPools.Find(p => objectToSpawn.CompareTag(p.LookUpString));

            if (pool == null)
            {
                pool = new ObjectPoolInfo(objectToSpawn.name);
                ObjectPools.Add(pool);
            }
            
            GameObject parentObject = SetParentObject(poolType);
            
            GameObject spawnableObject = null;
            if (pool.PooledObjects.Count <= 0)
            {
                spawnableObject = Instantiate(objectToSpawn);
                spawnableObject.transform.SetParent(parentObject.transform);
                spawnableObject.SetActive(false);
                pool.PooledObjects.Add(spawnableObject);
            }
            foreach (var obj in pool.PooledObjects.Where(obj => !obj.activeInHierarchy))
            {
                spawnableObject = obj;
                break;
            }
            

            return spawnableObject;
        }
        
        public static void ReturnToPool(GameObject objectToReturn)
        {
            var usableName = objectToReturn.name.Substring(0, objectToReturn.name.Length - 7);
            
            var pool = ObjectPools.Find(p => p.LookUpString == usableName);

            if (pool != null)
            {
                objectToReturn.SetActive(false);
                pool.PooledObjects.Add(objectToReturn);
            }
            else
            {
                Debug.LogError("Trying to release an object that doesn't belong to any pool: " + objectToReturn.name);
            }
        }
        
        
        private void Start()
        {
            for (int i = 0; i < amountToPoolBe; i++)
            {
                var temp = SpawnFromPool(basicEnemy);
            }
        }
    
        /*public GameObject GetPooledObject(string wantedTag)
        {
            for (int i = 0; i < _pooledObjects.Count; i++)
            {
                if (!_pooledObjects[i].activeInHierarchy && _pooledObjects[i].CompareTag(wantedTag))
                {
                    return _pooledObjects[i];
                }
            }

            GameObject tmp = Instantiate(basicEnemy);
            tmp.SetActive(false);
            _pooledObjects.Add(tmp);
            return tmp;
        }*/
    }
}
