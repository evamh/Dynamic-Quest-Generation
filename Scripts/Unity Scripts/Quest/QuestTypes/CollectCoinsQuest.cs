using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

/**
    Class for the COLLECT COINS quest type 
    Extends the QuestObject class
**/

[System.Serializable]
public class CollectCoinsQuest : QuestObject
{
    public int CoinsToCollect;
    public int OriginalCoins;
    private InventoryItemSO coinItem;

    public CollectCoinsQuest() : base(String.Empty, String.Empty, 0, null, Constants.Quest_Types.COLLECT_COINS) 
    {}
    
    public CollectCoinsQuest(string id, string name, string desc, int coinReward, int collectCoins) :
        base(name, desc, coinReward, id, Constants.Quest_Types.COLLECT_COINS)
    {
        CoinsToCollect = collectCoins;
        OriginalCoins = CoinManager.current.CurrentCoins;

        coinItem = ItemSOManager.current.GetItemFromTag(Constants.Items.COIN);

    }

    public override QuestPostInitStates PostInitialise()
    {
        // Spawn coins
        bool coinsSpawnedCorrectly = SpawnCoinsInWorld();

        // If there was an issue spawning coins, quit 
        if(!coinsSpawnedCorrectly) return QuestPostInitStates.QUIT;
        
        return QuestPostInitStates.CONTINUE;
    }

    public override bool QuestCompleted()
    {
        if(Input.GetKeyDown(KeyCode.Space))
        {
            GameObject potentialCoinFound = HelperFunctions.GameObjectIsNearbyWithReturn(coinItem.Tag, 5);
            if(potentialCoinFound != null) 
            {
                coinItem.Pool.Release(potentialCoinFound);
                CoinManager.current.IncreaseCoins(1);
            }
        }

        return (CoinManager.current.CurrentCoins - OriginalCoins) >= CoinsToCollect;
    }

    public override string ToString()
    {
        string result = base.ToString();
        result += "\n coins to collect: " + CoinsToCollect;
        return result;
    }

    public static new CollectCoinsQuest FromJSONStructure(Dictionary<string, object> json)
    {
        try
        {
            CollectCoinsQuest quest = new CollectCoinsQuest(
                null,
                json[Constants.JSON_Fields.QUEST_TITLE].ToString(),
                json[Constants.JSON_Fields.QUEST_DESC].ToString(),
                GetCoinRewardFromJson(json),
                Math.Min(10, Convert.ToInt32(json[Constants.JSON_Fields.NUM_COINS])) // ensure max of 10 in case model outputs too many 
            );

            return quest;

        } catch(System.Exception e)
        {
            string message = "[CollectCoinsQuest - FromJSONStructure()] Exception thrown for JSON : " + json;
            message += "\nException: " + e;
            Debug.Log(message);

            return null;
        }
    }

    // Spawn the corresponding number of coins in the game world 
    private bool SpawnCoinsInWorld()
    {
        try
        {
            for(int i = 0; i < CoinsToCollect; i++)
            {
                Vector3 position = HelperFunctions.GenerateRandomPositionInWorld();
                GameObject coin = coinItem.Pool.Get();
                coin.transform.position = new Vector3(position.x, position.y + 0.8f, position.z);
            }
            
            return true;
        } catch(System.Exception e)
        {
            string message = "[CollectCoinsQuest - SpawnCoinsInWorld()] Exception thrown: \n";
            message += e;
            Debug.Log(message);

            return false;
        }
    }
}
