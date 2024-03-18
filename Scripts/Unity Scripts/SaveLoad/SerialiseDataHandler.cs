using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using Newtonsoft.Json;

/**
    SerailsieDataHandler class for Save/Load functionality

    This code was modified from the one in the video "How to make a Save & Load System in Unity | 2022" by Shaped by Rain Studios. 
**/


public class SerialiseDataHandler
{
    private string pathToFile;
    private string fileName;
    private string fullPath;

    // Serialiser settings
    private JsonSerializerSettings settings; 

    public SerialiseDataHandler(string path, string name)
    {
        fullPath = Path.Combine(path, name);
        Debug.Log("full path is: " + fullPath);
        settings = new JsonSerializerSettings{TypeNameHandling = TypeNameHandling.Auto};
    }

    // Load data from file and deserialise into a GameData object 
    public GameData LoadData()
    {
        GameData gameData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string loadData = "";
                using(FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using(StreamReader reader = new StreamReader(stream))
                    {
                        loadData = reader.ReadToEnd();
                    }
                }
                gameData = JsonConvert.DeserializeObject<GameData>(loadData, settings);

            } catch(Exception e)
            {
                Debug.Log("[SerialiseDataHandler.LoadData] Error trying to load from " + fullPath + "\n" + e);
            }
        }

        return gameData;
    }

    // Serialise the GameData object and save in file 
    public void SaveData(GameData dataToSave)
    {
        try
        {
            // Create directory
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // Serialise data to json
            string json = JsonConvert.SerializeObject(dataToSave, Formatting.Indented, settings);

            // Write to file
            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(json);
                    Debug.Log("[SerialiseDataHandler.SaveData] Stored data to " + fullPath);
                }
            }

        } catch(Exception e)
        {
            Debug.Log("[SerialiseDataHandler.SaveData] Error trying to save data to: " + fullPath + "\n" + e);
        }
    }
}
