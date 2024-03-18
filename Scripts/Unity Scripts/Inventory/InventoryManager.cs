using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Singleton class to manage the inventory in game
**/

public class InventoryManager : MonoBehaviour, IDataSave
{
    public static InventoryManager current;
    public Dictionary<string, int> Inventory; // Dictionary of inventory items 

    void Awake()
    {
        current = this;
    }

    // Add item to the inventory
    public void AddItem(string tag, int amount = 1)
    {
        if(Inventory.ContainsKey(tag))
        {
            Inventory[tag] += amount;
        } else {
            Inventory.Add(tag, amount);
        }
    }

    // Decrease the amount for a specified item
    public void DecreaseAmountForItem(string tag, int amount = 1)
    {
        if(Inventory.TryGetValue(tag, out int num) && num >= amount)
        {
            Inventory[tag] -= amount;

            if(Inventory[tag] == 0)
            {
                Debug.Log("No more " + tag + " in inventory. Removing entry.");
                Inventory.Remove(tag);
            }
        }
    }

    // Check if the item exists in the inventory 
    public bool ExistsInInventory(string tag)
    {
        return Inventory.ContainsKey(tag);
    }

    // Given a tag, get the corresponding InventoryItemSO object 
    public InventoryItemSO GetItemSO(string tag)
    {
        return ItemSOManager.current.GetItemFromTag(tag);
    }

    // Data Persistence
    public void LoadData(GameData gameData)
    {
        Inventory = gameData.Inventory;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.Inventory = Inventory;
    } 
}
