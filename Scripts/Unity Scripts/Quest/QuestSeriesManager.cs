using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;

/**
    Singleton class for the Quest Series Manager
    Manages the list of all QuestSeries and their states
    Activates the following QuestSeries (if one exists) when the previous completes 
**/

public class QuestSeriesManager : MonoBehaviour, IDataSave
{
    public static QuestSeriesManager current;
    public List<QuestSeriesObject> Series;

    private QuestSeriesObject activeSeries;
    private int counter;

    void Awake()
    {
        current = this;
        counter = 0;

        EventsManager.current.GPTResponseEvent += CreateQuestSeriesFromJSON;
    }

    void OnDestroy()
    {
        EventsManager.current.GPTResponseEvent -= CreateQuestSeriesFromJSON;
    }

    // Update is called once per frame
    void Update()
    {
        if(activeSeries != null && activeSeries.GetCurrentQuest().QuestCompleted())
        {
            NextStepOrSeries();
        }
    }

    // Data persisting 
    private void InitialiseLoadedGame()
    {
        // Set counter from previous game run 
        counter = 0;
        SetCounter();

        if(!AllSeriesAreInState(QuestState.COMPLETED))
        {
            SetActiveSeries(Series[counter]);
            activeSeries.StartQuestSeries();
        }    
    }

    // Determine and activate the next quest step
    // If the series is finished, complete the current series and move onto the next one
    private void NextStepOrSeries()
    {
        // Complete quest step 
        activeSeries.CompleteQuestStep();

        // If series is finished, move on 
        if(activeSeries.QuestSeriesCompleted())
        {
            activeSeries.CompleteSeries();
            Debug.Log("[QuestSeriesManager] series completed!");
            EventsManager.current.InvokeQuestSeriesChangeEvent(activeSeries);
            EventsManager.current.InvokeQuestSeriesCompletedEvent(activeSeries);

            NextQuestSeries();
        } else
        {
            // othewise move on to next step
            activeSeries.NextQuestStep();
            Debug.Log("[QuestSeriesManager] moving onto next quest of series");
            EventsManager.current.InvokeQuestSeriesChangeEvent(activeSeries);
        } 

    }

    // Return the current active quest series 
    private QuestSeriesObject GetActiveSeries()
    {
        foreach(QuestSeriesObject s in Series)
        {
            if(s.State == QuestState.ACTIVE) 
            {
                return s;
            };
        }

        return null;
    }

    // Set the current active quest series to the given QuestSeriesObject 
    private void SetActiveSeries(QuestSeriesObject s)
    {
        activeSeries = s;
        activeSeries.StartQuestSeries();
    }

    // Get the next quest series in the list and set it as the active series 
    private void NextQuestSeries()
    {
        counter++;
        if(counter >= Series.Count) 
        {
            Debug.Log("All quest series completed!");
            activeSeries = null;
            return;
        }

        SetActiveSeries(Series[counter]);
        //EventsManager.current.InvokeQuestSeriesChangeEvent(activeSeries);
        
    }

    // Check if all series are in the specified state 
    private bool AllSeriesAreInState(QuestState state)
    {
        foreach(QuestSeriesObject s in Series)
        {
            if(s.State != state) return false;
        }

        return true;
    }

    // Add a QuestSeriesObject to the list 
    private void AddSeries(QuestSeriesObject series)
    {
        // Check it doesn't already exist
        if(GetAllIDs().Contains(series.SeriesID)) return;

        Series.Add(series);

        if(activeSeries == null || AllSeriesAreInState(QuestState.COMPLETED)) SetActiveSeries(series);

        EventsManager.current.InvokeQuestSeriesAddedEvent(series);
    }

    // Set the counter 
    private void SetCounter()
    {
        foreach(QuestSeriesObject s in Series)
        {
            if(s.State == QuestState.COMPLETED) counter++;
        }

        Debug.Log("counter is: " + counter);
    }

    // Return the list of all QuestSeries IDs
    private List<string> GetAllIDs()
    {
        List<string> IDs = new List<string>();
        foreach(QuestSeriesObject s in Series)
        {
            IDs.Add(s.SeriesID);
        }

        return IDs;
    }

    // Wrapper function for creating a QuestSeriesObject from a given JSON 
    public void CreateQuestSeriesFromJSON(string json)
    {
        QuestSeriesObject series = JSONParserManager.current.CreateQuestSeriesFromJSON(json);
        if(series != null) AddSeries(series); // don't add if there was an issue parsing the GPT response 
    }

    // Persisting data
    public void LoadData(GameData gameData)
    {
        Series = gameData.QuestSeries;
        if(Series.Count > 0) InitialiseLoadedGame(); // there's something to initialise 
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.QuestSeries = Series;
    }

}
