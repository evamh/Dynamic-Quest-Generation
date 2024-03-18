using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

/**
    Functions to generate string descriptions of each parameter in game world:
        1. NPC names
        2. Inventory information (objects)
        3. Locations in game world 

    The final string description concatenates each singular description and gets appended to the 
    system instructions sent to the GPT model 
**/

public class DescriptionGenerator : MonoBehaviour
{
    public Dictionary<string, List<string>> Descriptions; // Dictionary of string descriptions for NPC, Inventory, and Location 

    // Start is called before the first frame update
    void Start()
    {
        Descriptions = new Dictionary<string, List<string>>();
        AddNPCInformation();
        AddInventoryInformation();
        AddLocationInformation();

        string res = GetWorldDescription();
    }

    // Add NPC information to the descriptions object 
    private void AddNPCInformation()
    {
        List<string> npcs = new List<string>();

        GameObject npcObject = GameObject.FindWithTag(Constants.Description.NPC_GAMEOBJECT_TAG);

        foreach(Transform NPC in npcObject.transform)
        {
            npcs.Add(NPC.tag);
        }

        Descriptions.Add(Constants.Description.NPC, npcs);
    }

    // Add inventory/object information to the description object 
    private void AddInventoryInformation()
    {
        List<string> items = new List<string>();
        foreach(InventoryItemSO item in ItemSOManager.current.Items)
        {
            if(!item.ExcludeForFindObjectQuest) items.Add(item.Tag);
        }

        Descriptions.Add(Constants.Description.INVENTORY_ITEMS, items);
    }

    // Add location information to the description object 
    private void AddLocationInformation()
    {
        //TODO: generate this from game object instead 
        string[] locations = { "BEACH", "FOREST", "VILLAGE" };
        Descriptions.Add(Constants.Description.LOCATIONS, locations.ToList());
    }

    // Concatenate each string into one world description with all the information
    public string GetWorldDescription()
    {
        string result = "";
        foreach(var description in Descriptions)
        {
            result += "\n" + description.Key + ": " + description.Value.ToList().ToCommaSeparatedString();
        }

        return result;
    }
}
