using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Interface for saving and loading data 

    The Save/Load code was modified from the one in the video "How to make a Save & Load System in Unity | 2022" by Shaped by Rain Studios. 
**/

public interface IDataSave 
{
    public void LoadData(GameData gameData);

    public void SaveData(ref GameData gameData);
}
