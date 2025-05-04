using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Scripts
{
    public class ObjectPoolManager : MonoBehaviour
    {
        public static ObjectPoolManager SourceObjectPoolManager;
        private List<ObjectPoolInfo> _objectPools;
    
        [FormerlySerializedAs("object1")]
        [Foldout("Basic Enemy")]
        [SerializeField] private GameObject basicEnemy;
        [Foldout("Basic Enemy")]
        [SerializeField] private int amountToPoolBE;

        private GameObject _enemiesEmpty;
        private GameObject _projectilesEmpty;
        private GameObject _particlesEmpty;
        private GameObject _noneEmpty;
        
        public enum PoolType
        {
            Enemies,
            Projectiles,
            Particles,
            None
        }

        private void Awake()
        {
            SourceObjectPoolManager = this;

            SetupEmpties();
        }

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        private void Start()
        {
            _objectPools.Add(new ObjectPoolInfo());

            for (int i = 0; i < amountToPoolBE; i++)
            {
                var temp = Instantiate(basicEnemy);
                temp.SetActive(false);
                _pooledObjects.Add(temp);
            }
        }
    
        public GameObject GetPooledObject(string wantedTag)
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
        }
    }
}
