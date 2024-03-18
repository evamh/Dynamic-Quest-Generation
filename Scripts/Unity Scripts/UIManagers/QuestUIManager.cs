using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;
using Unity.VisualScripting;
using UnityEditor;
using System;

/**
    Manager for the Quest UI
**/

public class QuestUIManager : MonoBehaviour, IUpdateUI
{
    [SerializeField] private GameObject questUIElementPrefab;
    [SerializeField] private GameObject questMenu;
    [SerializeField] private GameObject questMenuContent;
    [SerializeField] private GameObject seriesPanel;

    // TODO - create a separate dropdown manager 
    private DropdownHandler dropdownHandler;
    
    //private Dictionary<string, GameObject> questUIElements;
    private Dictionary<string, List<GameObject>> uiElements;
    private string activeSeries;

    //TODO: subscribe to the quest added event
    void Awake()
    {
        dropdownHandler = GetComponent<DropdownHandler>();

        Reset();

        EventsManager.current.QuestSeriesAddedEvent += AddNewSeries;
        EventsManager.current.QuestSeriesChangeEvent += UpdateQuestElements;
        EventsManager.current.QuestSeriesCompletedEvent += CompleteSeries;
        EventsManager.current.DisplaySeriesEvent += DropdownValueChanged;
    }

    // Toggles the Quest UI when button is clicked 
    public void QuestMenuButtonClick()
    {
        UIManager.current.ToggleUIElement(questMenu);
    }

    // Reset the Quest UI
    private void Reset()
    {
        uiElements = new Dictionary<string, List<GameObject>>();
        activeSeries = null;
        seriesPanel.transform.Find("SeriesText").GetComponent<TextMeshProUGUI>().text = String.Empty;

        dropdownHandler.Reset();
    }

    // Add new quest series to the UI
    private void AddNewSeries(QuestSeriesObject series)
    {
        List<GameObject> questUIElements = new List<GameObject>();

        // Create UI element for each quest object in series 
        foreach(QuestObject quest in series.QuestSeries)
        {
            GameObject ui = CreateQuestUIElement(quest);
            questUIElements.Add(ui);

            // Initially set everything as inactive
            ui.SetActive(false);
        }

        // Store in dictionary
        uiElements.Add(series.SeriesID, questUIElements);

        // Add dropdown option
        dropdownHandler.AddDropdownOption(series);

        if(activeSeries == null) MakeActiveSeries(series);
    }

    // Create UI element for given quest object 
    private GameObject CreateQuestUIElement(QuestObject quest)
    {  
        try
        {
            //Debug.Log("Creating Quest UI element for: " + quest.ToString());
            GameObject questUIElement = Instantiate(questUIElementPrefab, questMenuContent.transform);

            Transform questPanelBG = questUIElement.transform.Find("QuestPanelBG");
        
            questPanelBG.transform.Find("QuestTitleText").GetComponent<TextMeshProUGUI>().text = quest.QuestTitle;
            questPanelBG.transform.Find("QuestDescText").GetComponent<TextMeshProUGUI>().text = quest.QuestDesc;

            // Add ID
            questUIElement.GetComponent<QuestInfoUI>().ID = quest.ID;

            return questUIElement;

        } catch(System.Exception e)
        {
            Debug.Log("[CreateQuestUIElement] Error thrown: " + e);
            return null;
        }  
    }

    // UI changes for when the quest series is completed 
    private void CompleteSeries(QuestSeriesObject series)
    {
        dropdownHandler.CompleteDropdown(series);
    }

    // Remove the series from the UI 
    private void RemoveSeries(string seriesID)
    {
        List<GameObject> elements = GetUIElementsForSeries(seriesID);

        if(elements != null)
        {
            foreach(GameObject elt in elements) elt.SetActive(false);
        }
    }

    // Update the UI elements for the given quest series 
    private void UpdateQuestElements(QuestSeriesObject series)
    {
        List<GameObject> elements = GetUIElementsForSeries(series.SeriesID);
        if(elements != null)
        {
            foreach(GameObject elt in elements) 
            {
                QuestObject q = series.GetQuestForID(elt.GetComponent<QuestInfoUI>().ID);

                // Make inactive if it hasn't started
                if(q.State != QuestState.NOT_STARTED) elt.SetActive(true);

                // Change color if quest is completed 
                if(q.State == QuestState.COMPLETED) SetPanelColor(elt);
            }
        }
    }

    // Display the given quest series as the active series in the UI 
    private void MakeActiveSeries(QuestSeriesObject series)
    {
        // Set up panel with description
        seriesPanel.transform.Find("SeriesText").GetComponent<TextMeshProUGUI>().text = series.SeriesDesc;
        UpdateQuestElements(series);
        activeSeries = series.SeriesID;
    }

    // Set the panel color for the given GameObject 
    private void SetPanelColor(GameObject elt)
    {
        Image background = elt.transform.Find("QuestPanelBG").GetComponent<Image>();
        Color current = background.color;
        background.color = new Color(current.r, current.g, current.b, 0.3f);
    }

    // When the player clicks on a different series in the dropdown, populate the quest UI accordingly 
    public void SwitchSeries(QuestSeriesObject series)
    {
        // Remove old series
        if(activeSeries!= null) RemoveSeries(activeSeries);

        // Populate new series if it already exists
        if(!uiElements.ContainsKey(series.SeriesID)) AddNewSeries(series);

        MakeActiveSeries(series);
    }

    // Return list of all UI elements for a given series 
    private List<GameObject> GetUIElementsForSeries(string seriesID)
    {
        if(uiElements.TryGetValue(seriesID, out List<GameObject> elements)) return elements;

        Debug.Log("No UI elements found for series: " + seriesID);
        return null;
    }

    // Wrapper function when the dropdown value changes - switch the series displayed 
    public void DropdownValueChanged(QuestSeriesObject series)
    {
        SwitchSeries(series);
    }

    // Update UI 
    public void UpdateDataUI()
    {
        // Loop through each quest series
        foreach(QuestSeriesObject series in QuestSeriesManager.current.Series)
        {
            AddNewSeries(series);

            if(series.State == QuestState.COMPLETED) CompleteSeries(series);
        }
    }

    // Reset UI
    public void ResetUI()
    {
         // Loop through each quest
        foreach(var kvp in uiElements)
        {
            foreach(GameObject elt in kvp.Value) Destroy(elt);
        }

        Reset();
    }

    void OnDestroy()
    {
        EventsManager.current.QuestSeriesAddedEvent -= AddNewSeries;
        EventsManager.current.QuestSeriesChangeEvent -= UpdateQuestElements;
        EventsManager.current.QuestSeriesCompletedEvent -= CompleteSeries;
        EventsManager.current.DisplaySeriesEvent -= DropdownValueChanged;
    }

}