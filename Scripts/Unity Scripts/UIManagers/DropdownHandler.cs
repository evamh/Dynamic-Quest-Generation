using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/**
    Class for drop down logic and associated handlers 
**/

public class DropdownHandler : MonoBehaviour
{
    [SerializeField] private TMP_Dropdown dropdown;
    public List<TMP_Dropdown.OptionData> Options;
    public Dictionary<int, QuestSeriesObject> IDs;
    public int CurrentIndex;

    void Start()
    {
        Options = new List<TMP_Dropdown.OptionData>();
        IDs = new Dictionary<int, QuestSeriesObject>();
        CurrentIndex = 0;
    }

    // When resetting the game, clear all the dropdown options in quest UI
    public void Reset()
    {
        dropdown.ClearOptions();
        Options = new List<TMP_Dropdown.OptionData>();
        IDs = new Dictionary<int, QuestSeriesObject>();
        CurrentIndex = 0;
    }

    // Add quest series to list of dropdown options 
    public void AddDropdownOption(QuestSeriesObject series)
    {
        Options.Add(new TMP_Dropdown.OptionData(series.SeriesTitle));
        UpdateDropdownOptions();
        IDs.Add(Options.Count - 1, series);
    }

    // Update dropdown options 
    public void UpdateDropdownOptions()
    {
        dropdown.ClearOptions();
        dropdown.AddOptions(Options);
    }

    // Populate associated series dropdown option with COMPLETE when quest series is completed 
    public void CompleteDropdown(QuestSeriesObject series)
    {
        int index = GetIndex(series);
        if(index != -1)
        {
            Options[index].text += " - COMPLETED";
            UpdateDropdownOptions();
        }
    }

    // Given a QuestSeriesObject, return the associated dropdown index if one exists 
    public int GetIndex(QuestSeriesObject series)
    {
        foreach(var kvp in IDs)
        {
            if(kvp.Value == series) return kvp.Key;
        }

        return -1;
    }

    // Given an index, return the associated QuestSeriesObject if index is valid 
    public QuestSeriesObject GetSeries(int index)
    {
        if(IDs.TryGetValue(index, out QuestSeriesObject series))
        {
            return series;
        } else 
        {
            Debug.LogWarning("No dropdown information stored for: " + index);
            return null;
        }
    }

    // Handler for when player changes the dropdown value, such as populating the correct quest series object in quest UI 
    public void OnDropdownValueChanged(int index)
    {
        CurrentIndex = index;
        QuestSeriesObject series = GetSeries(index);
        if(series != null) EventsManager.current.InvokeDisplaySeriesEvent(series);
    }
    
}
