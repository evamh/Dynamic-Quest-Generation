using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.Pool;


/**
    Scriptable Object class to encapsulate the logic for objects (both coins and objects to find in quests)
**/

[CreateAssetMenu(fileName = "InventoryItemSO", menuName = "ScriptableObjects/InventoryItemSO", order = 1)]
public class InventoryItemSO : ScriptableObject
{
    [SerializeField] public string Tag;
    [SerializeField] public ItemTypes Type;
    [SerializeField] public GameObject Prefab;
    [SerializeField] public Image GameImage;
    [SerializeField] public bool ExcludeForFindObjectQuest = false; // this will be true for Coins, as they are their own separate quest type 
   
    public IObjectPool<GameObject> Pool;

    public InventoryItemSO(string name, ItemTypes objectType, GameObject prefab, Image image, bool exclude)
    {
        Tag = name;
        Type = objectType;
        Prefab = prefab;
        GameImage = image;
        ExcludeForFindObjectQuest = exclude;
    }
}
