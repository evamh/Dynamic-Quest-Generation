using System;
using System.Collections;
using System.Collections.Generic;
using Mono.Cecil.Cil;
using Newtonsoft.Json;
using Unity.Burst.CompilerServices;
using UnityEngine;

/**
    Singleton class that manages the parsing of JSON objects
    This class is responsible for taking in JSON objects returned by the GPT model, and instantiating
    both quest series objects and within the series, each individual quest step 

    This class acts as the translation layer between GPT and the game's code 
**/

public class JSONParserManager : MonoBehaviour
{
    public static JSONParserManager current;

    void Awake()
    {
        current = this;
    }

    // Given a questObject, create an instance of the appropriate Quest Type class 
    private QuestObject CreateSingularQuest(Dictionary<string, object> questObj)
    {
        string questType = questObj[Constants.JSON_Fields.QUEST_TYPE].ToString();
        return questType switch
        {
            Constants.Quest_Types.TALK_NPC => TalkNPCQuest.FromJSONStructure(questObj),
            Constants.Quest_Types.COLLECT_COINS => CollectCoinsQuest.FromJSONStructure(questObj),
            Constants.Quest_Types.FIND_OBJECT => FindObjectQuest.FromJSONStructure(questObj),
            _ => QuestObject.FromJSONStructure(questObj),
        };
    }

    // Given a JSON object, deserialise the string into a dictionary of string/object pairs
    // Then create the corresponding QuestObject instance 
    // Return null if any exceptions are thrown 
    public QuestObject CreateQuestFromJSON(string json)
    {
        try
        {
            Dictionary<string, object> jsonStruct = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            QuestObject quest = CreateSingularQuest(jsonStruct);
            return quest;

        } catch(Exception e)
        {
            string message = "[JSONParserManager.ParseJSON] Unable to parse JSON: " + json;
            message += "\nException thrown: " + e;

            Debug.Log(message);

            return null;
        }
    } 

    // Given a JSON object, deserialise the string into an instance of a QuestSeries object
    // This also deserialises the individual quest objects stored in SERIES_STEPS as corresponding QuestObject instances
    // Return null if any exceptions are thrown 
    public QuestSeriesObject CreateQuestSeriesFromJSON(string json)
    {
        try
        {
            string oldJSON = json;
            json = json.Replace("\\\"", "\"").Trim('"');

            Debug.Log(json);

            Dictionary<string, object> jsonStruct = JsonConvert.DeserializeObject<Dictionary<string, object>>(json);
            List<Dictionary<string, object>> questObjs = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(jsonStruct[Constants.JSON_Fields.SERIES_STEPS].ToString());

            List<QuestObject> seriesQuests = new List<QuestObject>();
            foreach(Dictionary<string, object> obj in questObjs)
            {
                QuestObject newQuest = CreateSingularQuest(obj);
                if(newQuest != null) seriesQuests.Add(newQuest);
            }

            QuestSeriesObject series = new QuestSeriesObject(
                jsonStruct[Constants.JSON_Fields.SERIES_TITLE].ToString(),
                jsonStruct[Constants.JSON_Fields.SERIES_DESC].ToString(),
                seriesQuests
            );

            return series;


        } catch(Exception e)
        {
            string message = "[JSONParserManager.CreateQuestSeriesFromJSON]";
            message += "Unable to parse JSON: " + json + "\nException thrown: " + e;

            Debug.Log(message);
            return null;
        }
    }

}
