using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/**
    Class for prop interaction framework, such as which items have been last seen by the player 
    These items would be sent to GPT to tailor quests further 

    Implementing this logic remains a TODO for this project 
**/

public class PropInteraction : MonoBehaviour
{
    [SerializeField] Transform Player;
    public List<string> PropsSeen;
    private int radius = 5;

    void Start()
    {
        PropsSeen = new List<string>();
    }

    // Update the list of props seen by the player
    private void UpdateObjectsSeen()
    {
        // Physics collider to get objects in radius 
        // Have to have tag "PROP"
        // Add to stack 
        Collider[] propColliders = HelperFunctions.NearbyProps(radius);

        foreach(Collider prop in propColliders) PropsSeen.Add(prop.gameObject.name);
    }

    // String representation is enough as is it being sent to GPT
    public string GenerateObjectsSeenString()
    {
        string result = "";

        // TODO: get last 10 items and remove any duplicates 

        return result;
    }
}
