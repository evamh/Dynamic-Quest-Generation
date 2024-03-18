using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/**
    Singleton class to manage all instances of object pools 

    ChatGPT helped quite a bit with this
**/

public class ObjectPoolManager : MonoBehaviour
{
    public static ObjectPoolManager current;
    private Dictionary<string, InventoryObjectPool> pools;

    // Start is called before the first frame update
    void Start()
    {
        pools = new Dictionary<string, InventoryObjectPool>();

        // Loop through each item in InventorySO and create a pool
        foreach(InventoryItemSO item in ItemSOManager.current.Items)
        {
            pools.Add(item.Tag, new InventoryObjectPool(item));
        }

        Debug.Log("[ObjectPoolManager] Pool count is: " + pools.Count);
    }

    // Given an InventoryItemSO, get the corresponding pool 
    public InventoryObjectPool GetPoolForItem(InventoryItemSO item)
    {
        if(pools.TryGetValue(item.Tag, out InventoryObjectPool objectPool))
        {
            return objectPool;
        } else {
            Debug.Log("No pool for: " + item.Tag + ". Returning null.");
            return null;
        }
    }

    // Given a tag, get the pool for the corresponding InventoryItemSO object 
    public InventoryObjectPool GetPoolForItem(string tag)
    {
        if(pools.TryGetValue(tag, out InventoryObjectPool objectPool))
        {
            return objectPool;
        } else {
            Debug.Log("No pool for: " + tag + ". Returning null.");
            return null;
        }
    }

}
