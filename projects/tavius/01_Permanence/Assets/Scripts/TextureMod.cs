using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Huge thanks to Hyperion from this 5 year-old thread for the code base:
// https://answers.unity.com/questions/528334/dynamic-terrain-painting.html

public class TextureMod : MonoBehaviour
{
    public float[,,] original;
    public TerrainData terrainData;
    public Texture2D drawSquare;
    public Color[] textureData;
    public int fileX;
    public int fileY;
    public int layers;
    public int terrainRayDist = 10;
    public int secs;

    void Start()
    {
        // Set 'painting' size and get the terrain:
        drawSquare = new Texture2D(16, 16);
        terrainData = Terrain.activeTerrain.terrainData;
        // Get resolutions:
        fileX = terrainData.alphamapWidth;
        fileY = terrainData.alphamapHeight;
        // Get textures (preset on Terrain object):
        layers = terrainData.alphamapLayers;
        // Set the array of Color pixel values:
        textureData = drawSquare.GetPixels();
        // Backup current terrain texture to reset on exit:
        original = terrainData.GetAlphamaps(0, 0, fileX, fileY);
    }

    void OnApplicationQuit()
    {
        terrainData.SetAlphamaps(0, 0, original);
    }

    void Update()
    {
        CheckForTerrain();
    }

    void CheckForTerrain()
    {
        // Cast the same ray as the RaycastManager attached to Main Camera:
        Vector3 forward = GameObject.Find("Main Camera").transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(GameObject.Find("Main Camera").transform.position, forward, out hit, maxDistance: terrainRayDist))
        {    // NOTE: The following may be outdated, code was adapted from old internet advice.
            Debug.Log("Ray go terrain");

            // An InverseLerp calculates where along the entire terrain the hit.point hit.
            // A regular Lerp uses that datapoint to find on where the alphamap was hit.
            int w = (int)Mathf.Lerp(0, fileX, Mathf.InverseLerp(0, terrainData.size.x, hit.point.x));
            int h = (int)Mathf.Lerp(0, fileY, Mathf.InverseLerp(0, terrainData.size.z, hit.point.z));

            // These values are then fixed within the alphamap based on the the bounds of the desired drawing size:
            w = Mathf.Clamp(w, drawSquare.width / 2, fileX - drawSquare.width / 2);
            h = Mathf.Clamp(h, drawSquare.height / 2, fileY - drawSquare.height / 2);

            // Finds the appropriate section of the Alphamap to draw the new square upon:
            var area = terrainData.GetAlphamaps(w - drawSquare.width / 2, h - drawSquare.height / 2, drawSquare.width, drawSquare.height);

            // Loop over the drawSquare pixels and populate the fill area:
            for (int x = 0; x < drawSquare.height; x++)
            {
                for (int y = 0; y < drawSquare.width; y++)
                {
                    for (int z = 0; z < layers; z++)
                    {
                        if (z == 1)
                        {
                            area[x, y, z] += textureData[x * drawSquare.width + y].a;
                        }
                        else
                        {
                            area[x, y, z] -= textureData[x * drawSquare.width + y].a;
                        }
                    }
                }
            }
            // Draw the new texture over the area:
            terrainData.SetAlphamaps(w - drawSquare.width / 2, h - drawSquare.height / 2, area);


            //int[] data = new int[] { w, drawSquare.width, h, drawSquare.height };
            //DrawTextureDelay(data, area);
        }
    }

    //IEnumerator DrawTextureDelay(int[] data, float[,,] area)
    //{
    //    Debug.Log("Texture Draw Delay!");
    //    terrainData.SetAlphamaps(data[0] - data[1] / 2, data[2] - data[3] / 2, area);
    //    yield return new WaitForSeconds(secs);

    //}
}