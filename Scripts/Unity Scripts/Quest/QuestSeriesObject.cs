using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/**
    Class that encapsulates the logic for each QuestSeriesObject 
**/

[System.Serializable]
// For larger quests that are made up of a load of smaller quests 
public class QuestSeriesObject
{
    public string SeriesID;
    public string SeriesTitle;
    public string SeriesDesc;
    public List<QuestObject> QuestSeries;
    public string UIColor;
    public int Counter;
    public QuestState State;

    private QuestObject currentQuest;

    public QuestSeriesObject()
    {
        SeriesID = String.Empty;
        SeriesTitle = String.Empty;
        SeriesDesc = String.Empty;
        QuestSeries = new List<QuestObject>();
        UIColor = Color.white.ToString();
        Counter = 0;
        State = QuestState.INACTIVE;
        
    }

    public QuestSeriesObject(string title, string desc, List<QuestObject> quests, string id = null)
    {
        SeriesID = (id == null) ? Guid.NewGuid().ToString() : id;
        SeriesTitle = title;
        SeriesDesc = desc;
        QuestSeries = quests;
        UIColor = HelperFunctions.GetRandomColor(0.1f, 0.5f).ToString();
        Counter = 0;
        State = QuestState.INACTIVE;

        // Make first step inactive
        SetFirstQuestInactive();
    }

    // By default, the first quest step should be inactive until the quest series starts 
    private void SetFirstQuestInactive()
    {
        GetQuestAtCounter().State = QuestState.INACTIVE;
    }

    // Start the quest series
    public void StartQuestSeries()
    {
        State = QuestState.ACTIVE; // set as active 
        currentQuest = GetQuestAtCounter();
        Debug.Log("CurrentQuest is: " + currentQuest.ToString());
        StartQuestStep();
    }

    // Start a quest step and call its PostInitialise function 
    private void StartQuestStep()
    {
        EventsManager.current.InvokeQuestAddedEvent(currentQuest);
        MarkQuestStepAs(QuestState.ACTIVE);
        Debug.Log("starting quest step: " + currentQuest.ToString());
        currentQuest.PostInitialise();
    }

    // Mark the current quest to the specified QuestState 
    private void MarkQuestStepAs(QuestState newState)
    {
        currentQuest.State = newState;
        EventsManager.current.InvokeQuestChangeEvent(currentQuest);
    }

    // Complete the current quest step and call its PostComplete function 
    public void CompleteQuestStep()
    {
        MarkQuestStepAs(QuestState.COMPLETED);
        currentQuest.PostComplete();
        CoinManager.current.IncreaseCoins(currentQuest.CoinReward);
    }

    // Complete the quest series 
    public bool QuestSeriesCompleted()
    {
        foreach(QuestObject quest in QuestSeries)
        {
            if(quest.State != QuestState.COMPLETED) return false;
        }

        return true;
    }

    // Start the next quest step 
    public void NextQuestStep()
    {
        Counter++;

        // Move on to next step
        currentQuest = GetQuestAtCounter();
        if(currentQuest == null) // quest series is over 
        {
            CompleteSeries();
        } else {
            StartQuestStep();
        }

    }

    // Complete the series 
    public void CompleteSeries()
    {
        State = QuestState.COMPLETED;
    }

    // Get the quest object at the current counter 
    private QuestObject GetQuestAtCounter()
    {
        if(Counter < QuestSeries.Count) return QuestSeries[Counter];
        
        Debug.Log("No quests left for series: " + SeriesID + ". Returning null");
        return null;
    }

    // Return the current quest object 
    public QuestObject GetCurrentQuest()
    {
        return currentQuest;
    }

    // Given an ID, return the specific quest object if one exists
    public QuestObject GetQuestForID(string id)
    {
        foreach(QuestObject q in QuestSeries)
        {
            if(q.ID == id) return q;
        }

        Debug.Log("Series: " + SeriesID + ", no quest found for quest ID: " + id
                    + ". Returning null.");
        return null;
    }

}
