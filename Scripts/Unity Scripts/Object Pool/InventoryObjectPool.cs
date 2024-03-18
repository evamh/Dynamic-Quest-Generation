using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Pool;

/**
    Class to encapsulate the logic for each object pool 
**/

public class InventoryObjectPool
{
    private InventoryItemSO item;
    private int minPoolSize = 2;
    private int maxPoolSize = 5;

    public ObjectPool<GameObject> Pool;

    public InventoryObjectPool(InventoryItemSO so)
    {
        item = so;

        // Larger pool required for coin objects  
        if(item.Tag == Constants.Items.COIN)
        {
            minPoolSize = 10;
            maxPoolSize = 20;
        }

        Pool = new ObjectPool<GameObject>(CreatePrefab, OnGetFromPool, OnReleaseFromPool, OnDestroyFromPool, true, minPoolSize, maxPoolSize);

        Prewarm();
    }

    // Create the instances of the objects using their assigned prefabs
    private GameObject CreatePrefab()
    {
        GameObject itemPrefab = MonoBehaviour.Instantiate(item.Prefab);
        item.Pool = Pool;
        return itemPrefab;
    }

    private void Prewarm()
    {
        for(int i = 0; i < minPoolSize; i++)
        {
            GameObject obj = Pool.Get();
            Pool.Release(obj);
        }
    }

    private void OnReleaseFromPool(GameObject poolObj)
    {
        poolObj.SetActive(false);
    }

    private void OnGetFromPool(GameObject poolObj)
    {
        poolObj.SetActive(true);
    }

    private void OnDestroyFromPool(GameObject poolObj)
    {
        MonoBehaviour.Destroy(poolObj);
    }

    private void OnDestroy()
    {
        Pool.Dispose();
    }

}
