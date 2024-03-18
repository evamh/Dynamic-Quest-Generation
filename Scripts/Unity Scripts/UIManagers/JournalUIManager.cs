using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.UIElements;
using System;

/**
    Manager for the Journal UI 
**/

public class JournalUIManager : MonoBehaviour, IUpdateUI
{
    [SerializeField] private GameObject journalUI;
    [SerializeField] private GameObject journalEntryPrefab;
    [SerializeField] private TMP_InputField inputField;
    //[SerializeField] private ScrollView scrollView;
    [SerializeField] private GameObject journalContentParent;

    private List<GameObject> journalUIEntries;

    // Displays the Journal UI 
    public void JournalButtonOnClick()
    {
       UIManager.current.ToggleUIElement(journalUI);     
    }

    // Creates journal entry 
    public void JournalEntryButtonOnClick()
    {
        if(inputField.text != String.Empty) CreateJournalEntry(inputField.text);
        inputField.text = String.Empty; // clear input field 
    }

    // Displays the journal entry for the given message 
    private void DisplayJournalEntry(string message)
    {
        GameObject journalEntry = Instantiate(journalEntryPrefab, journalContentParent.transform);
        journalEntry.transform.Find("JournalEntryText").GetComponent<TextMeshProUGUI>().text = message;

        journalUIEntries.Add(journalEntry);
    }

    // Creates the journal entry for the given message 
    private void CreateJournalEntry(string message)
    {   
        DisplayJournalEntry(message);
        EventsManager.current.InvokeJournalEntryAddedEvent(message); 
    }

    // Update the UI 
    public void UpdateDataUI()
    {
        journalUIEntries = new List<GameObject>();

        // Display existing journal entries
        foreach(JournalEntry entry in JournalManager.current.JournalEntriesList)
        {
            DisplayJournalEntry(entry.JournalText);
        }   
    }

    // Reset the UI
    public void ResetUI()
    {
        foreach(GameObject ui in journalUIEntries)
        {
            Destroy(ui);
        }

        journalUIEntries = new List<GameObject>();
    }
}
