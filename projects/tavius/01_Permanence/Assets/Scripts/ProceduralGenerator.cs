using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    // Many thanks to Fabi from this 2014 thread:
    // http://forum.brackeys.com/thread/random-objects/

    public Terrain terrain;
    public int numMedRocks; // number of objects to place
    public int numSmallRocks; // number of objects to place
    public int numTrees; // number of objects to place
    private int currentMedRocks; // number of placed objects
    private int currentSmallRocks; // number of placed objects
    private int currentTrees; // number of placed objects
    public GameObject mediumRock; // GameObject to place
    public GameObject smallRock; // GameObject to place
    public GameObject tree; // GameObject to place
    private int terrainWidth; // terrain size (x)
    private int terrainLength; // terrain size (z)
    private int terrainPosX; // terrain position x
    private int terrainPosZ; // terrain position 

    void Start()
    {
        // terrain size x
        terrainWidth = (int)terrain.terrainData.size.x;
        // terrain size z
        terrainLength = (int)terrain.terrainData.size.z;
        // terrain x position
        terrainPosX = (int)terrain.transform.position.x;
        // terrain z position
        terrainPosZ = (int)terrain.transform.position.z;
    }

    void Update()
    {
        // generate objects
        if (currentMedRocks <= numMedRocks)
        {
            // generate random x position
            int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
            // generate random z position
            int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
            // get the terrain height at the random position
            float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
            // create new gameObject on random position
            GameObject newObject = (GameObject)Instantiate(mediumRock, new Vector3(posx, posy - 2, posz), Quaternion.Euler(posx, posy, posz));
            newObject.AddComponent<TestFade>();
            currentMedRocks += 1;
        }

        // Generate small rocks
        if (currentSmallRocks <= numSmallRocks)
        {
            // generate random x position
            int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
            // generate random z position
            int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
            // get the terrain height at the random position
            float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
            // create new gameObject on random position
            GameObject newObject = (GameObject)Instantiate(smallRock, new Vector3(posx / 2, posy, posz / 2), Quaternion.Euler(posx, posy, posz));
            newObject.AddComponent<TestFade>();
            currentSmallRocks += 1;
        }       
        
        // Generate trees
        //if (currentTrees <= numTrees)
        //{
        //    // generate random x position
        //    int posx = Random.Range(terrainPosX, terrainPosX + terrainWidth);
        //    // generate random z position
        //    int posz = Random.Range(terrainPosZ, terrainPosZ + terrainLength);
        //    // get the terrain height at the random position
        //    float posy = Terrain.activeTerrain.SampleHeight(new Vector3(posx, 0, posz));
        //    // create new gameObject on random position
        //    GameObject newObject = (GameObject)Instantiate(tree, new Vector3(posx, posy - 1, posz), Quaternion.identity);
        //    newObject.AddComponent<TestFade>();
        //    currentTrees += 1;
        //}
    }
}





//    public GameObject mediumRock;
//    public int numMediumRocks;

//    GameObject[] all_mediumRocks;

//    // Use this for initialization
//    void Start()
//    {
//        all_mediumRocks = new GameObject[numMediumRocks*numMediumRocks];
//        int k = 0;

//        for (int j = 0; j < numMediumRocks; j++)
//        {
//            for (int i = 0; i < numMediumRocks; i++)
//            {
//                float newy = 1.5f;

//                all_mediumRocks[k] = Instantiate(mediumRock, new Vector3(i-numMediumRocks/5, newy, j-numMediumRocks/3), Quaternion.identity);
//                k++;
//            }
//        }
//    }
//}


// ctrl+k+c = comment
// ctrl+k+u = uncomment
