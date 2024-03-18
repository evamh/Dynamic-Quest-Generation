using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using UnityEngine.UIElements;

/**
    Scriptable object to encompass the logic for a LOCATION object
**/

[CreateAssetMenu(fileName = "LocationSO", menuName = "ScriptableObjects/LocationSO", order = 1)]
public class LocationSO : ScriptableObject
{
    [SerializeField] public string LocationName;
    [SerializeField] UnityEngine.Vector3 startVector;
    [SerializeField] UnityEngine.Vector3 endVector;

    public LocationSO (string name)
    {
        LocationName = name;
    }

    // Generate random position in the game world 
    private UnityEngine.Vector3 GenerateRandomPosition(Terrain terrain)
    {
        UnityEngine.Vector3 terrainSize = terrain.terrainData.size;
        float x = Random.Range(0, terrainSize.x);
        float z = Random.Range(0, terrainSize.z);

        UnityEngine.Vector3 pos = new UnityEngine.Vector3(x, 0, z);

        return pos;

    }

    // Check that the position generated is within the game world 
    private bool PositionWithinLocation(Terrain terrain, UnityEngine.Vector3 pos)
    {
        //RaycastHit hit;
        UnityEngine.Vector3 terrainSize = terrain.terrainData.size;

        UnityEngine.Vector3 posToCheck = pos + UnityEngine.Vector3.up * terrainSize.y; 

        if(Physics.Raycast(posToCheck, UnityEngine.Vector3.down))
        {
            return true;
        }

        return false;
    }

    // Generate a position vector and test that it is in the location
    public UnityEngine.Vector3 GenerateAndTestPoint(Terrain terrain)
    {
        UnityEngine.Vector3 pos = GenerateRandomPosition(terrain);
        if(PositionWithinLocation(terrain, pos)) return pos;

        return UnityEngine.Vector3.zero;
    }
}
