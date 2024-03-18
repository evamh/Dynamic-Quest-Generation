using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OpenAI;
using OpenAI.Chat;

/**
    Class for all data values persisted between sessions 

    The Save/Load code was modified from the one in the video "How to make a Save & Load System in Unity | 2022" by Shaped by Rain Studios. 
**/

[System.Serializable]
public class GameData
{
    // Journal entries
    public List<JournalEntry> JournalEntries;

    // Quests
    public List<QuestSeriesObject> QuestSeries;

    // NPC Speech
    public List<List<CharacterSpeech>> Dialogues;

    // GPT Messages
    public List<Message> GPTMessages;

    // Coins
    public int CoinCount;

    // Inventory
    public Dictionary<string, int> Inventory;

    public GameData()
    {
        JournalEntries = new List<JournalEntry>(); 
        QuestSeries = new List<QuestSeriesObject>();
        Dialogues = new List<List<CharacterSpeech>>();
        GPTMessages = new List<Message>();
        CoinCount = 10; // TODO - don't hardcode this

        Inventory = new Dictionary<string, int>();
    }


}
