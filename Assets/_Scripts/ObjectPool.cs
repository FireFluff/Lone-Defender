using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ObjectPool : MonoBehaviour
{
    public static ObjectPool ShearedInstance;
    private List<GameObject> _pooledObjects;
    [SerializeField] private GameObject objectToPool;
    [SerializeField] private int amountToPool;

    private void Awake()
    {
        ShearedInstance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _pooledObjects = new List<GameObject>();
        GameObject temp;

        for (int i = 0; i < amountToPool; i++)
        {
            temp = Instantiate(objectToPool);
            temp.SetActive(false);
            _pooledObjects.Add(temp);
        }
    }
    
    public GameObject GetPooledObject()
    {
        for (int i = 0; i < _pooledObjects.Count; i++)
        {
            if (!_pooledObjects[i].activeInHierarchy)
            {
                return _pooledObjects[i];
            }
        }

        GameObject tmp = Instantiate(objectToPool);
        tmp.SetActive(false);
        _pooledObjects.Add(tmp);
        return tmp;
    }
}
