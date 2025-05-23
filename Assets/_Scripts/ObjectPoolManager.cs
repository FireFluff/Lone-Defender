using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;
using System.Linq;

namespace _Scripts
{
    public class ObjectPoolManager : MonoBehaviour
    {
        /// <summary>
        /// The list of all current object pools.
        /// </summary>
        private static readonly List<ObjectPoolInfo> ObjectPools = new List<ObjectPoolInfo>();
        
        // Object pool 1
        [Foldout("Basic Enemy")]
        [SerializeField] private GameObject basicEnemy;
        [FormerlySerializedAs("amountToPoolBe")]
        [Foldout("Basic Enemy"), Range(1, 100)]
        [SerializeField] private int preloadAmountBe;
        
        [Foldout("Inspector organizers")]
        [SerializeField] private GameObject emptyHolder;
        [Space]
        private static GameObject _enemiesEmpty;
        private static GameObject _projectilesEmpty;
        private static GameObject _particlesEmpty;
        private static GameObject _noneEmpty;
        
        
        /// <summary>
        /// The enum for separating the pooled objects to separate parent objects.
        /// </summary>
        public enum PoolType
        {
            Enemies,
            Projectiles,
            Particles,
            None
        }
        // Variables for the object pools.
        //==============================================================================================================
        private void Awake()
        {
            SetupEmpties();
        }
        //==============================================================================================================
        // The setup stage for better Hierarchy organization during.
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
        //==============================================================================================================
        
        /// <summary>
        /// Spawn an object from the pools with a specific position rotation.
        /// </summary>
        /// <param name="objectToSpawn">What object to get/create in the pools.</param>
        /// <param name="spawnPosition">Vector3 location.</param>
        /// <param name="spawnRotation">Quaternion Rotation.</param>
        /// <param name="poolType">Defaults to none, used to organize in the hierarchy.</param>
        /// <returns>
        /// Game object from the pools with the given location and Rotation.
        /// It's also a child of the given parent pool object.
        /// </returns>
        public static GameObject SpawnFromPool(GameObject objectToSpawn, Vector3 spawnPosition,
            Quaternion spawnRotation, PoolType poolType = PoolType.None)
        {
            // Look through the pools and find a pool with the correct objects.
            ObjectPoolInfo pool = ObjectPools.Find(p => p.LookUpString == objectToSpawn.name);

            // If no pools matched, make one for this object.
            if (pool == null)
            {
                pool = new ObjectPoolInfo(objectToSpawn.name);
                ObjectPools.Add(pool);
            }
            
            GameObject parentObject = SetParentObject(poolType); // Bind the object to appropriate empty.
            /*GameObject spawnableObject = null;
            // Look for an inactive object in the pool and return it.
            foreach (var obj in pool.InactiveObjects.Where(obj => !obj.activeInHierarchy))
            {
                spawnableObject = obj;
                spawnableObject.SetActive(true);
                pool.InactiveObjects.Remove(spawnableObject);
                break;
            }*/
            
            GameObject spawnableObject = pool.InactiveObjects.FirstOrDefault();
            // Check if the pool has any objects in it, if not make one.
            if (spawnableObject == null)
            {
                spawnableObject = Instantiate(objectToSpawn, parentObject.transform, true);
                pool.InactiveObjects.Add(spawnableObject);
            }
            
            spawnableObject.transform.position = spawnPosition;
            spawnableObject.transform.rotation = spawnRotation;
            spawnableObject.SetActive(true);
            pool.InactiveObjects.Remove(spawnableObject);
            
            return spawnableObject;
        }
        //==============================================================================================================
        
        /// <summary>
        /// Set the parent object to one of the empty objects for hierarchy organization. 
        /// </summary>
        /// <param name="poolType">What empty pool object to parent to.</param>
        /// <returns>The proper empty object to parent to.</returns>
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
        //==============================================================================================================
        
        /// <summary>
        /// The simpler overload of spawn from pool that you can set location/rotation externally.
        /// </summary>
        /// <param name="objectToSpawn">What object to get/create in the pools.</param>
        /// <param name="poolType">Defaults to none, used to organize in the hierarchy.</param>
        /// <returns>
        /// Game object from the pools with no set location/rotation.
        /// But it's a child of the given parent pool object.
        /// </returns>
        private static GameObject SpawnFromPool(GameObject objectToSpawn, PoolType poolType = PoolType.None)
        {
            // Look through the pools and find a pool with the correct objects.
            ObjectPoolInfo pool = ObjectPools.Find(p => objectToSpawn.CompareTag(p.LookUpString));

            // If no pools matched, make one for this object.
            if (pool == null)
            {
                pool = new ObjectPoolInfo(objectToSpawn.name);
                ObjectPools.Add(pool);
            }
            
            GameObject parentObject = SetParentObject(poolType); // Bind the object to appropriate empty.
            GameObject spawnableObject = null;
            // Check if the pool has any objects in it, if not make one.
            if (pool.InactiveObjects.Count <= 0)
            {
                spawnableObject = Instantiate(objectToSpawn, parentObject.transform, true);
                spawnableObject.SetActive(false);
                pool.InactiveObjects.Add(spawnableObject);
            }
            // Look for an inactive object in the pool and return it.
            foreach (var obj in pool.InactiveObjects.Where(obj => !obj.activeInHierarchy))
            {
                spawnableObject = obj;
                break;
            }
            
            return spawnableObject;
        }
        //==============================================================================================================
        
        public static void ReturnToPool(GameObject objectToReturn)
        {
            var usableName = objectToReturn.name.Substring(0, objectToReturn.name.Length - 7);
            
            var pool = ObjectPools.Find(p => p.LookUpString == usableName);

            if (pool != null)
            {
                objectToReturn.SetActive(false);
                pool.InactiveObjects.Add(objectToReturn);
            }
            else
            {
                Debug.LogError("Trying to release an object that doesn't belong to any pool: " + objectToReturn.name);
            }
        }
        //==============================================================================================================
        
        private void Start()
        {
            
            // Preload the object pools with the given amount.
            PreloadObjects(basicEnemy, preloadAmountBe, PoolType.Enemies);
        }
        //==============================================================================================================
        private static void PreloadObjects(GameObject prefab, int amount, PoolType poolType)
        {
            ObjectPoolInfo pool = new ObjectPoolInfo(prefab.name);
            ObjectPools.Add(pool);
            GameObject parentObject = SetParentObject(poolType);
            for (int i = 0; i < amount; i++)
            {
                GameObject obj = Instantiate(prefab, parentObject.transform , true);
                obj.SetActive(false);
                pool.InactiveObjects.Add(obj);
            }
        }
    }
}
