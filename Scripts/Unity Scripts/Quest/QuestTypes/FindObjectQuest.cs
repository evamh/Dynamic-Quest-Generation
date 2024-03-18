using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System;

/**
    Class for the FIND OBJECT quest type
    Extends the QuestObject class
**/

public class FindObjectQuest : QuestObject
{
    public string ObjToFindTag;
    public string Location;
    private InventoryItemSO inventoryItem;
    private GameObject objToFind; // actual prefab

    public FindObjectQuest() : base(String.Empty, String.Empty, 0, null, Constants.Quest_Types.FIND_OBJECT) 
    {}

    public FindObjectQuest(string id, string name, string desc, int coinReward, string obj, string loc) :
        base(name, desc, coinReward, id, Constants.Quest_Types.FIND_OBJECT)
    {
        ObjToFindTag = obj.ToUpper();
        Location = loc.ToUpper();
        QuestType = Constants.Quest_Types.FIND_OBJECT;
    }

    public override QuestPostInitStates PostInitialise()
    {
        // Spawn prefab instance in random part of the world and store coordinates
        inventoryItem = ItemSOManager.current.GetItemFromTag(ObjToFindTag);

        objToFind = SpawnGameObjectInWorld();
        if(objToFind == null) return QuestPostInitStates.QUIT;

        return QuestPostInitStates.CONTINUE;
    }

    public override bool QuestCompleted()
    {
        if(Input.GetKeyDown(KeyCode.Space) && HelperFunctions.GameObjectIsNearby(objToFind, 5))
        {
            return true;
        }

        return false;
    }

    public override void PostComplete()
    {
        // Add object to inventory
        InventoryManager.current.AddItem(ObjToFindTag, 1);
        // Remove gameobject 
        inventoryItem.Pool.Release(objToFind);
    }

    public static new FindObjectQuest FromJSONStructure(Dictionary<string, object> json)
    {
        try
        {
            string loc = Constants.Locations.DEFAULT;
            if(json.TryGetValue(Constants.JSON_Fields.LOCATION, out object location))
            {
                loc = location.ToString();
            }
            
            FindObjectQuest quest = new FindObjectQuest(
                null,
                json[Constants.JSON_Fields.QUEST_TITLE].ToString(),
                json[Constants.JSON_Fields.QUEST_DESC].ToString(),
                GetCoinRewardFromJson(json),
                json[Constants.JSON_Fields.OBJECT_TYPE].ToString(),
                loc
            );

            return quest;

        } catch(System.Exception e)
        {
            string message = "[FindObjectQuest - FromJSONStructure()] Exception thrown for JSON : " + json;
            message += "\nException: " + e;
            Debug.Log(message);

            return null;
        }
    }

    // Spawn the object in the game world 
    private GameObject SpawnGameObjectInWorld()
    {
        Vector3 position = EnvLocationGenerator.current.GenerateCoordinateInLocation(Location);
        if(position == Vector3.zero) position = HelperFunctions.GenerateRandomPositionInWorld();

        Debug.Log("GENERATING OBJECT IN: " + position.ToString());

        GameObject pooledObject = inventoryItem.Pool.Get(); // or get it from ObjectPoolManager.current.GetPoolForItem?
        pooledObject.transform.position = position;
        return pooledObject;
    }

    public override void OnDestroy()
    {
        MonoBehaviour.Destroy(objToFind);
    }

}
