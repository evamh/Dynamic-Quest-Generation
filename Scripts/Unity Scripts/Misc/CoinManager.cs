using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
    Singleton class to manage the logic for coins
**/

public class CoinManager : MonoBehaviour, IDataSave
{
    public static CoinManager current;
    [SerializeField] public int startCoins = 10;
    public int CurrentCoins;

    void Awake()
    {
        current = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        EventsManager.current.InvokeCoinChangeEvent(CurrentCoins);
    }

    // Increase total number of coins by given amount
    public void IncreaseCoins(int amount)
    {
        CurrentCoins += amount;
        EventsManager.current.InvokeCoinChangeEvent(CurrentCoins);
    }

    // Decrease total number of coins by given amount 
    public void DecreaseCoins(int amount)
    {
        CurrentCoins -= amount; 
        EventsManager.current.InvokeCoinChangeEvent(CurrentCoins);
    }

    // Data persisting 
    public void LoadData(GameData gameData)
    {
        CurrentCoins = gameData.CoinCount;
    }

    public void SaveData(ref GameData gameData)
    {
        gameData.CoinCount = CurrentCoins;
    }
}

