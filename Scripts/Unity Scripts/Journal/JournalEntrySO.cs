using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Scriptable Object class to encapsulate the values for a journal entry
**/

[System.Serializable]
public class JournalEntrySO : ScriptableObject
{
    public string ID;
    public string entryText;

    public JournalEntrySO(string id, string message)
    {
        ID = id;
        entryText = message;
    }
}