using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
    Functions for generating and testing position vectors in the game world
**/

public class EnvLocationGenerator : MonoBehaviour
{
    public static EnvLocationGenerator current;
    [SerializeField] Terrain terrain;

    void Awake()
    {
        current = this;
    }

    // Function to generate random point in the world
    private Vector3 GenerateRandomPoint()
    {
        float x = UnityEngine.Random.Range(0f, terrain.terrainData.size.x);
        float z = UnityEngine.Random.Range(0f, terrain.terrainData.size.z);
        float y = terrain.SampleHeight(new Vector3(x, 0, z));

        return new Vector3(x, y, z);
    }

    // Determine if a position vector collides with the Forest, Beach or Village 
    private string GetLocationFromPoint(Vector3 position)
    {
        RaycastHit hit;

        if(Physics.Raycast(position + Vector3.up * 100f, Vector3.down, out hit, Mathf.Infinity))
        {
            if(hit.collider.CompareTag(Constants.Locations.FOREST)) return Constants.Locations.FOREST;
            if(hit.collider.CompareTag(Constants.Locations.BEACH)) return Constants.Locations.BEACH;
            if(hit.collider.CompareTag(Constants.Locations.VILLAGE)) return Constants.Locations.VILLAGE;
        }

        return String.Empty;
    }

    // Given a location, generate a position vector in location 
    public Vector3 GenerateCoordinateInLocation(string location)
    {
        Vector3 position = new Vector3();
        string returned = String.Empty;
        int counter = 0;

        while(returned != location && counter < 50)
        {
            position = GenerateRandomPoint();
            returned = GetLocationFromPoint(position);
            counter++;
        }

        return position;
    }
}
