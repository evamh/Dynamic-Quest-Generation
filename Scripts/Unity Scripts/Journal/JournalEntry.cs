using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Class to encapsulate the values for a journal entry
**/

[System.Serializable]
public class JournalEntry 
{
    public string JournalText; 
    public string EntryID;

    public JournalEntry(string text, string id)
    {
        JournalText = text;
        EntryID = id;
    }

    public override string ToString()
    {
        string result = "Journal entry: " + EntryID;
        result += "\nText:" + JournalText;
        return result;
    }

}
