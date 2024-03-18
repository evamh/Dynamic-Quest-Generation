using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using static QuestInfoSO;
    
/**
    Base class for each Quest Object 
    Encapsulates the core logic for each Quest Type 
**/

[System.Serializable]
public class QuestObject
{
    public string ID;
    public string QuestType;
    public string QuestTitle;
    public string QuestDesc;
    public int CoinReward;
    public QuestState State; 

    public QuestObject() {}

    public QuestObject(string name, 
        string desc, 
        int reward, 
        string questID = null, 
        string type = Constants.Quest_Types.UNDEFINED 
    )
    {
        ID = (ID == null) ? Guid.NewGuid().ToString() : ID;
        QuestType = type;
        QuestTitle = name;
        QuestDesc = desc;
        CoinReward = reward;
        State = QuestState.NOT_STARTED; // TODO: figure out if there's already an active quest
    }

    // ToString function 
    public override string ToString()
    {
        string result = "ID: " + ID; 
        result += ", type: " + QuestType;
        result += ", title: " + QuestTitle;
        result += ", description: " + QuestDesc;
        result += ", reward: " + CoinReward;
        result += ", state: " + State;

        return result;
    }

    // Determines if the quest is completed 
    public virtual bool QuestCompleted()
    {
        return false;
    }

    // Called when the quest has finished initialising 
    public virtual QuestPostInitStates PostInitialise() 
    {
        return QuestPostInitStates.CONTINUE;
    }

    // Called post quest completion 
    public virtual void PostComplete() {}

    public virtual void OnDestroy() {}

    // Retrieve the coin reward from the JSON, returning 0 if any exceptions are thrown 
    protected static int GetCoinRewardFromJson(Dictionary<string, object> json)
    {
        try
        {
            return Convert.ToInt32(json[Constants.JSON_Fields.COIN_REWARD]);
        } catch(Exception)
        {
            return 0;
        }
    }

    // Function that initialises the quest object from a JSON object 
    public static QuestObject FromJSONStructure(Dictionary<string, object> json)
    {
        return null;
    }

}
