using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using System.Linq;
using System;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

/**
    Class for the TALK NPC quest type
    Extends the QuestObject class
**/

[System.Serializable]
public class TalkNPCQuest : QuestObject
{
    public string NPCTag;
    public List<CharacterSpeech> Dialogue;

    public TalkNPCQuest() : base(String.Empty, String.Empty, 0, null, Constants.Quest_Types.TALK_NPC) 
    {}

    public TalkNPCQuest(string id, string name, string desc, int coinReward, string tag, List<CharacterSpeech> speeches) :
        base(name, desc, coinReward, id, Constants.Quest_Types.TALK_NPC)
    {
        NPCTag = tag.ToUpper();
        Dialogue = speeches;
    }

    public override bool QuestCompleted()
    {
        // Use key "t" for talk
        // Check which items are in the peripheral 
        if(Input.GetKeyDown(KeyCode.T) && HelperFunctions.GameObjectIsNearby(NPCTag, 5)) return true;

        return false;
    }

    public override void PostComplete()
    {
        EventsManager.current.InvokeCommunicationEvent(Dialogue);

        // store speech
        NPCManager.current.StoreSpeech(Dialogue);

        // destroy game object 
        //if(HelperFunctions.IsNPCRandomlyGenerated(NPCTag)) HelperFunctions.DestroyGameObjectByTag(NPCTag);

    }

    public override QuestPostInitStates PostInitialise()
    {
        if(HelperFunctions.IsNPCRandomlyGenerated(NPCTag)) 
        {
            bool result = CreateRandomNPC();
            if(!result) return QuestPostInitStates.QUIT; // Error generating NPC, quit the quest 
        }
        
        return QuestPostInitStates.CONTINUE;
    }

    public static new TalkNPCQuest FromJSONStructure(Dictionary<string, object> json)
    {
        try
        {

            List<CharacterSpeech> dialogue = JsonConvert.DeserializeObject<List<CharacterSpeech>>(json[Constants.JSON_Fields.DIALOGUE].ToString());
            
            TalkNPCQuest quest = new TalkNPCQuest(
                null, // generate new GUID
                json[Constants.JSON_Fields.QUEST_TITLE].ToString(),
                json[Constants.JSON_Fields.QUEST_DESC].ToString(),
                GetCoinRewardFromJson(json),
                json[Constants.JSON_Fields.NPC_NAME].ToString(), 
                dialogue
            );

            return quest;

        } catch(System.Exception e)
        {
            string message = "[TalkNPCQuest - FromJSONStructure()] Exception thrown for JSON : " + json;
            message += "\nException: " + e;
            Debug.Log(message);

            return null;
        }
    }

    private bool CreateRandomNPC()
    {
        try
        {
            // Create NPC
            GameObject NPC = DefaultNPCGenerator.current.GenerateNPC();
            NPCTag = NPC.tag.ToUpper();

            return true;

        } catch(System.Exception e)
        {
            Debug.Log("[TalkNPCQuest.CreateRandomNPC] Exception thrown: \n" + e);
            return false;
        }
       
    }

    public override string ToString()
    {
        string result = base.ToString();
        result += ", NPC name: " + NPCTag;
        result += ", dialogue: " + Dialogue.ToString();

        return result;
    }
}
