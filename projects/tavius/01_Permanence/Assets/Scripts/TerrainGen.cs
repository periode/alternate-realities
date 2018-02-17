using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TerrainGen : MonoBehaviour {

    public int width = 256;
    public int height = 256;
    public float depth = 1.5f;
    public float scale = 20f;


    void Start()
    {
        // Load world:
        Terrain World = GetComponent<Terrain>();
        World.terrainData = MakeWorld(World.terrainData);
        // Set player in the middle:
        GameObject.Find("GvrEditorEmulator").transform.Translate(-width / 2, 2.5f, -height / 2);
    }

    void Update ()
    {
        Terrain World = GetComponent<Terrain>();
        World.terrainData = MakeWorld(World.terrainData);

    }

    TerrainData MakeWorld(TerrainData terrainData)
    {
        terrainData.heightmapResolution = width + 1;
        terrainData.size = new Vector3(width, depth, height);
        terrainData.SetHeights(0, 0, PerlinHeights());
        return terrainData;
    }

    float[,] PerlinHeights()
    {
        float[,] heights = new float[width, height];
        for(int i = 0; i < width; i++)
        {
            for(int j = 0; j < height; j++)
            {
                heights[i, j] = Mathf.PerlinNoise((float)i / width * scale, (float)j / height * scale);
            }
        }
        return heights;
    }
}
