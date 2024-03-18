using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Scriptable object class to encapsulate the key values for each Quest Object 
**/

[System.Serializable]
public class QuestInfoSO : ScriptableObject
{
    [SerializeField] public string ID;
    [SerializeField] public string QuestType;
    [SerializeField] public string QuestTitle;
    [SerializeField] public string QuestDesc;
    [SerializeField] public int CoinReward;

    public void Initialize(string questType, string name, string desc, int reward)
    {
        ID = Guid.NewGuid().ToString();
        QuestType = questType;
        QuestTitle = name;
        QuestDesc = desc;
        CoinReward = reward;
    }
}
