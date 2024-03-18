using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
    Singleton class to manage the journal entries
    Keeps a list of JournalEntry objects, essentially constructing a journal 
**/

public class JournalManager : MonoBehaviour, IDataSave
{
    public static JournalManager current;
    public List<JournalEntry> JournalEntriesList;

    void Awake()
    {
        current = this;
        EventsManager.current.JournalEntryAddedEvent += AddJournalEntry;
    }

    // Add a journal entry to list of entries 
    private void AddJournalEntry(string journalText)
    {
        string id = "ENTRY-" + (JournalEntriesList.Count + 1);
        JournalEntry newEntry = new JournalEntry(journalText, id);
        JournalEntriesList.Add(newEntry); 
    }

    void OnDestroy()
    {
        EventsManager.current.JournalEntryAddedEvent -= AddJournalEntry;
    }

    // Persisting data
    public void LoadData(GameData gameData)
    {
        JournalEntriesList = gameData.JournalEntries;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.JournalEntries = JournalEntriesList;
    }
}
