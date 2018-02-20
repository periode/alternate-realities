using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvObjGenerator : MonoBehaviour
{
    // Many thanks to Fabi from this 2014 thread:
    // http://forum.brackeys.com/thread/random-objects/

    public Terrain terrain;

    public int numMedRocks;
    public int numSmallRocks;
    public int numTrees;
    private int currentMedRocks;
    private int currentSmallRocks;
    private int currentTrees;
    public GameObject mediumRock;
    public GameObject smallRock;
    public GameObject tree;

    private int terrainWidth; // Terrain size (x)
    private int terrainLength; // Terrain size (z)
    private int terrainPosX; // To store points along the terrain
    private int terrainPosZ;

    void Start()
    {
        // Get terrain data:
        terrainWidth = (int)terrain.terrainData.size.x;
        terrainLength = (int)terrain.terrainData.size.z;
        terrainPosX = (int)terrain.transform.position.x;
        terrainPosZ = (int)terrain.transform.position.z;

        // Generate medium rocks
        while (currentMedRocks < numMedRocks)
        {
            // Generate random x and z positions along the terrain:
            int posx = Random.Range(terrainPosX + terrainWidth / 8, terrainWidth - terrainWidth / 8);
            int posz = Random.Range(terrainPosZ + terrainLength / 8, terrainLength - terrainLength / 8);
            // Get the terrain height at the random position:
            float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
            // Create new mediumRock:
            GameObject.Instantiate(mediumRock, new Vector3(posx, posy - 2, posz), Quaternion.Euler(Random.Range(0.0f, 359.9f), Random.Range(0.0f, 359.9f), Random.Range(0.0f, 359.9f)));
            currentMedRocks += 1;
        }

        // Generate small rocks
        while (currentSmallRocks < numSmallRocks)
        {
            // Generate random x and z positions within the center of the terrain:
            int posx = Random.Range(terrainPosX + terrainWidth / 3, terrainWidth - terrainWidth / 3);
            int posz = Random.Range(terrainPosZ + terrainLength / 3, terrainLength - terrainLength / 3);
            // Get the terrain height:
            float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
            // Create new smallRock:
            GameObject.Instantiate(smallRock, new Vector3(posx, posy, posz), Quaternion.Euler(0, Random.Range(0.0f, 359.9f), 0));
            currentSmallRocks += 1;
        }

        //Generate trees
        //while (currentTrees < numTrees)
        //{
        //    int posx = Random.Range(terrainPosX + terrainWidth / 8, terrainWidth - terrainWidth / 8);
        //    int posz = Random.Range(terrainPosZ + terrainLength / 8, terrainLength - terrainLength / 8);
        //    float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
        //    // create new tree, slightly sunk into the ground:
        //    GameObject.Instantiate(tree, new Vector3(posx, posy - 1.0f, posz), Quaternion.Euler(0, Random.Range(0.0f, 359.9f), 0));
        //    currentTrees += 1;
        //}
    }
}
