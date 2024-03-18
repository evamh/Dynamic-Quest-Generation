using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Singleton class to generate a default NPC
    
    This is in case the GPT model returns an undefined NPC 
**/

public class DefaultNPCGenerator : MonoBehaviour
{
    public static DefaultNPCGenerator current;
    [SerializeField] GameObject NPCPrefab;

    void Awake()
    {
        current = this;
    }

    // Generate a random NPC using the prefab and assign its position and tag 
    public GameObject GenerateNPC()
    {
        // Generate random location
        Vector3 location = HelperFunctions.GenerateRandomPositionInWorld();

        // Instantiate prefab
        GameObject NPC = Instantiate(NPCPrefab);
        NPC.transform.position = new Vector3(location.x, location.y + 0.5f, location.z);

        // Assign tag 
        NPC.tag = Constants.Character_Tags.RANDOMLY_GENERATED;

        return NPC;
    }


}
