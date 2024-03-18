using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Manager for the Inventory UI
**/

public class InventoryUIManager : MonoBehaviour
{
    [SerializeField] private GameObject inventoryUI;
    //[SerializeField] private GameObject inventoryButton;

    // Displays inventory button accordingly 
    public void InventoryButtonOnClick()
    {
        Debug.Log("InventoryButton on click");
        UIManager.current.ToggleUIElement(inventoryUI);     
    }
}
