using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

/**
    Singleton class for the persisting data manager

    Manages the save/load functionality and the corresponding UI functionality

    The Save/Load code was modified from the one in the video "How to make a Save & Load System in Unity | 2022" by Shaped by Rain Studios. 
**/

public class PersistDataManager : MonoBehaviour
{
    public static PersistDataManager current;

    [SerializeField] private string fileName;

    private GameData gameData;
    private SerialiseDataHandler serialiseDataHandler;

    private List<IDataSave> dataSaveObjs;
    private List<IUpdateUI> updateUIObjs;

    void Awake()
    {
        current = this;

        EventsManager.current.NewGameEvent += NewGame;
    }

    // Start is called before the first frame update
    void Start()
    {
        serialiseDataHandler = new SerialiseDataHandler(Application.persistentDataPath, fileName);
        dataSaveObjs = FindAllDataSaveObjs();
        updateUIObjs = FindAllUpdateUIObjs();
        LoadGame();
    }

    // Return list of all objects that implement the IDataSave interface 
    private List<IDataSave> FindAllDataSaveObjs()
    {
        IEnumerable<IDataSave> objs = FindObjectsOfType<MonoBehaviour>().OfType<IDataSave>();
        return new List<IDataSave>(objs);
    }

    // Return list of all objects that implement the IUpdateUI interface 
    private List<IUpdateUI> FindAllUpdateUIObjs()
    {
        IEnumerable<IUpdateUI> objs = FindObjectsOfType<MonoBehaviour>().OfType<IUpdateUI>();
        return new List<IUpdateUI>(objs);
    }

    // Wrapper function for new game, which resets the GameData object and resets the UI 
    public void NewGame()
    {
        gameData = new GameData();
        LoadDataForObjs();
        ResetUI(); // Reset UI element
    }

    // Load the data  
    private void LoadDataForObjs()
    {
        // Load all IDataSave objs
        foreach(IDataSave obj in dataSaveObjs)
        {
            obj.LoadData(gameData);
        }
    }

    // Display UI elements 
    private void DisplayUIElements()
    {
        foreach(IUpdateUI obj in updateUIObjs)
        {
            obj.UpdateDataUI();
        }
    }

    // Reset the UI 
    private void ResetUI()
    {
        foreach(IUpdateUI obj in updateUIObjs)
        {
            obj.ResetUI();
        }
    }

    // Load the game 
    public void LoadGame()
    {
        gameData = serialiseDataHandler.LoadData();

        if(gameData == null)
        {
            Debug.Log("[PersistDataManager.LoadGame] No game data found, creating new game.");
            NewGame();
        }

        LoadDataForObjs();      // Load game data
        DisplayUIElements();    // Update UI elements 
    }

    // Save the game 
    public void SaveGame()
    {
        foreach(IDataSave obj in dataSaveObjs)
        {
            obj.SaveData(ref gameData);
        }

        serialiseDataHandler.SaveData(gameData);
    }

    // Automatically save game when application quits 
    private void OnApplicationQuit()
    {
        SaveGame();
    }

    void OnDestroy()
    {
        EventsManager.current.NewGameEvent -= NewGame;
    }
}
