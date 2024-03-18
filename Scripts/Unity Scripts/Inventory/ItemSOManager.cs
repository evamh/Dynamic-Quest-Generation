using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal.Profiling.Memory.Experimental;
using UnityEngine;

/**
    Singleton class to manage the logic for InventoryItemSO objects
**/

public class ItemSOManager : MonoBehaviour
{
    public static ItemSOManager current;

    [SerializeField] public List<InventoryItemSO> Items;
    [SerializeField] InventoryItemSO Default;
    
    void Awake()
    {
        current = this; 
    }

    // Given a string tag, return the corresponding InventoryItemSO if it exists
    public InventoryItemSO GetItemFromTag(string tag)
    {
        InventoryItemSO item = Items.Find(x => x.Tag.ToUpper() == tag.ToUpper());
        if(item == null)
        {
            Debug.Log("[InventoryManager.GetItemFromTag] Item for " + tag + " doesn't exist. Returning default.");
            return Default;
        }

        return item;
    }

    // Given a string tag, return the corresponding Prefab from its InventoryItemSO object if it exists
    public GameObject GetGameObjFromTag(string tag)
    {
        return GetItemFromTag(tag).Prefab;
    }
}
