using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Singleton class to manage the NPC logic, specifically the dialogue
**/

public class NPCManager : MonoBehaviour, IDataSave
{
    public static NPCManager current;

    public List<List<CharacterSpeech>> Dialogues;

    void Awake()
    {
        current = this;
    } 

    void Start()
    {
        PostLoad();
    }

    // Given dialogue, store the speech entry for the character speaking
    private void StoreSpeechForNPC(List<CharacterSpeech> dialogue)
    {
        foreach(CharacterSpeech entry in dialogue)
        {
            try
            {
                GameObject NPC = GameObject.FindWithTag(entry.Tag.ToUpper());

                if(NPC != null)
                {
                    NPC.GetComponent<NPCBehaviour>().AddSpeechEntry(entry);
                } else {
                    Debug.Log("NPC with tag " + entry.Tag + " not found.");
                }
                
            } catch(System.Exception e)
            {
                Debug.Log("Exception storing speech for " + entry.ToString() + ",\n" + e);
            }
            
        }
    }

    // Wrapper function to store the speechEntries for each character 
    public void StoreSpeech(List<CharacterSpeech> speechEntries)
    {
        StoreSpeechForNPC(speechEntries);
        Dialogues.Add(speechEntries);
        
    }

    // Post load 
    private void PostLoad()
    {
        foreach(List<CharacterSpeech> dialogue in Dialogues)
        {
            StoreSpeechForNPC(dialogue);
        }
    }

    // Persisting data
    public void LoadData(GameData gameData)
    {
        Dialogues = gameData.Dialogues;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.Dialogues = Dialogues;
    }
}

