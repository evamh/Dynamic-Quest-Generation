using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/**
    Class to encapsulate NPC Behaviour
    Keeps a record of all speech objects for the NPC 
**/

public class NPCBehaviour : MonoBehaviour
{
    public Dictionary<int, CharacterSpeech> speechHistory;
    private int entryCounter;

    // Start is called before the first frame update
    void Start()
    {
        speechHistory = new Dictionary<int, CharacterSpeech>();
        entryCounter = 0;
    }

    // Add a CharacterSpeech object to the speech history
    public void AddSpeechEntry(CharacterSpeech speechEntry)
    {
        speechHistory.Add(entryCounter, speechEntry);
        entryCounter++;
    }

    // Return the corresponding CharacterSpeech object at the specified index 
    public CharacterSpeech GetSpeechEntryAt(int index)
    {
        if(speechHistory.TryGetValue(index, out CharacterSpeech entry))
        {
            return entry;
        } else
        {
            string npcTag = gameObject.tag;
            Debug.Log("No speech entry for NPC " + npcTag + " at counter " + index + "."
                        + "Returning null.");
            return null;
        }
    }

    // Get the first x CharacterSpeech entries 
    public List<CharacterSpeech> GetFirstEntries(int num)
    {
        List<CharacterSpeech> entries = speechHistory.Values.ToList();
        int end = Math.Min(num, entries.Count - 1);
        return entries.GetRange(0, end);
    }

    // Get the last x CharacterSpeech entries 
    public List<CharacterSpeech> GetLastEntries(int num)
    {
        List<CharacterSpeech> entries = speechHistory.Values.ToList();
        int start = Math.Max(0, entries.Count - num);
        int end = Math.Min(num, entries.Count - start);
        return entries.GetRange(0, end);
    }
}
