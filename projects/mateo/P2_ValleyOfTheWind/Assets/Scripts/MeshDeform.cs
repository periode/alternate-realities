using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshDeform: MonoBehaviour {
	public int width = 256;
	public int height = 256;
	public int depth = 15;
	public float ndepth = 20;
	public float scale = 10;
	public float offsetX = 100f;
	public float offsetY = 100f;


	void Start()
	{
		offsetX = Random.Range (0f, 999f);
		offsetY = Random.Range (0f, 999f);
		float timing = (Time.time) * 1.5f;

		Terrain terrain = GetComponent<Terrain>();
		terrain.terrainData = GenerateTerrain(terrain.terrainData);		
		//ndepth = depth + (Mathf.Sin (timing) * 8);
		//ndepth = (Mathf.Sin(Time.time)*15);
		//scale = 5 + (Mathf.Sin(Time.time)*2);
		//		if (ndepth > 15){
		//			offsetX += Time.deltaTime * 0.5f;

		//		}
	}



	TerrainData GenerateTerrain (TerrainData terrainData)
	{
		terrainData.heightmapResolution = width + 1;
		terrainData.size = new Vector3 (width, ndepth, height);
		terrainData.SetHeights (0, 0, GenerateHeights ());
		return terrainData;
	}



	float[,] GenerateHeights()
	{
		float[,] heights= new float[width, height];
		for (int x = 0; x < width; x++)
		{
			for (int y = 0; y < height; y++)
			{
				heights [x, y] = CalculateHeight(x, y);
			}
		}
		return heights;
	}




	float CalculateHeight (int x, int y)
	{
		float xCoord = (float)x / width* scale + offsetX;
		float yCoord = (float)y / height * scale + offsetY;
		return Mathf.PerlinNoise (xCoord, yCoord);

	}


	// Use this for initialization
	void Update ()
	{
	}
		

}