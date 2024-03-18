using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;
using Unity.VisualScripting;

/**
    Class of helper functions 
**/

public class HelperFunctions 
{
    // Given a tag, get the first game object associated with that tag 
    public static GameObject GetGameObjectWithTag(string tag)
    {   
        try
        {
            GameObject[] objectsWithTag = GameObject.FindGameObjectsWithTag(tag);

            if(objectsWithTag.Length > 0) return objectsWithTag[0]; // return first element;

        } catch(Exception)
        {
            string message = "Error looking for gameObject with tag: "  + tag + ". Returning null.";
            Debug.Log(message);
        }

        return null;
    }

    // Determine if a tag exists in the game
    public static bool TagIsDefined(string tag)
    {
        if(GetGameObjectWithTag(tag) != null) return true;

        return false;
    }

    // Given a tag and radius, find if the corresponding GameObject is nearby
    public static bool GameObjectIsNearby(string tag, int radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(FirstPersonController.current.PlayerTransform.position, radius);
        bool found = hitColliders.Any(hit => hit.CompareTag(tag));
        return found;
    }

    // Given a GameObject and radius, find if the GameObject is nearby
    public static bool GameObjectIsNearby(GameObject obj, int radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(FirstPersonController.current.PlayerTransform.position, radius);
        bool found = hitColliders.Any(hit => hit.gameObject == obj);
        return found;
    }

    // Given a tag and radius, find if the corresponding GameObject is nearby and return it 
    public static GameObject GameObjectIsNearbyWithReturn(string tag, int radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(FirstPersonController.current.PlayerTransform.position, radius);
        foreach(Collider hit in hitColliders)
        {
            if(hit.CompareTag(tag)) return hit.gameObject;
        }
        
        return null;
    }

    // Given a radius, return the list of nearby collisions 
    public static Collider[] NearbyProps(int radius)
    {
        Collider[] hitColliders = Physics.OverlapSphere(FirstPersonController.current.PlayerTransform.position, radius);
        Collider[] propColliders = hitColliders.Where(hit => hit.CompareTag(Constants.Props.PROPS)).ToArray();

        return propColliders;
    }

    // Given a tag, destroy the associated game object if it exists 
    public static void DestroyGameObjectByTag(string tag)
    {
        GameObject potentialObject = GetGameObjectWithTag(tag);

        if(potentialObject != null) MonoBehaviour.Destroy(potentialObject);
    }

    // Get a random color within a brightness range
    public static Color GetRandomColor(float minBright, float maxBright)
    {
        float red = UnityEngine.Random.value;
        float green = UnityEngine.Random.value;
        float blue = UnityEngine.Random.value;

        float bright = UnityEngine.Random.Range(minBright, maxBright);

        return Color.Lerp(Color.black, new Color(red, green, blue), bright);
    }

    // Determine if an NPC is randomly generated
    public static bool IsNPCRandomlyGenerated(string tag)
    {
        if(Constants.Character_Tags.CHARACTERS.Contains(tag)) return false;

        return true;
    }

    // Generate random point in game world 
    public static Vector3 GenerateRandomPositionInWorld()
    {
        Terrain terrain = GameObject.Find("Terrain").GetComponent<Terrain>();
        Vector3 terrainSize = terrain.terrainData.size;

        float x = UnityEngine.Random.Range(0f, terrainSize.x);
        float z = UnityEngine.Random.Range(0f, terrainSize.z);
        float y = terrain.SampleHeight(new Vector3(x, 0f, z));

        return new Vector3(x, y, z);
    }
}
