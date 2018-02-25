using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Huge thanks to Hyperion from this 5 year-old thread for the code base:
// https://answers.unity.com/questions/528334/dynamic-terrain-painting.html

public class TexturePainting : MonoBehaviour
{
    public float[,,] original;
    public TerrainData terrainData;
    public Texture2D drawSquare;
    public Color[] textureData;
    public int drawSize = 18;
    public int fileX;
    public int fileY;
    public int layers;
    public float secs = 0.27f;

    void Start()
    {
        // Set painting size and get the terrain:
        drawSquare = new Texture2D(drawSize, drawSize);
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

    public void Restart(int newDrawSize)
    {
        drawSize = newDrawSize;
        drawSquare = new Texture2D(drawSize, drawSize);
        terrainData = Terrain.activeTerrain.terrainData;
        fileX = terrainData.alphamapWidth;
        fileY = terrainData.alphamapHeight;
        layers = terrainData.alphamapLayers;
        textureData = drawSquare.GetPixels();
    }

    void OnApplicationQuit()
    {
        terrainData.SetAlphamaps(0, 0, original);
    }

    void Update()
    {
        // Live update drawSize in Unity Inspector:
        //drawSquare = new Texture2D(drawSize, drawSize);
        //textureData = drawSquare.GetPixels();

        // Update raycast:
        CheckForTerrain();
    }

    void CheckForTerrain()
    {
        // Cast the same ray as the RaycastManager attached to Main Camera:
        Vector3 forward = GameObject.Find("Main Camera").transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(GameObject.Find("Main Camera").transform.position, forward, out hit, maxDistance: 200))
        {   // NOTE: While functional, the following may be outdated, code was adapted from old internet advice.
            if (!hit.collider.gameObject.CompareTag("HideFromPaint"))
            {
                // An InverseLerp calculates where along the entire terrain the hit.point hit.
                // A regular Lerp uses that datapoint to find on where the alphamap was hit.
                int w = (int)Mathf.Lerp(0, fileX, Mathf.InverseLerp(0, terrainData.size.x, hit.point.x));
                int h = (int)Mathf.Lerp(0, fileY, Mathf.InverseLerp(0, terrainData.size.z, hit.point.z));

                // These values are then fixed within the alphamap based on the the bounds of the desired drawing size:
                w = Mathf.Clamp(w, drawSize / 2, fileX - drawSize / 2);
                h = Mathf.Clamp(h, drawSize / 2, fileY - drawSize / 2);

                // Assigns the appropriate section of the Alphamap to draw the new square upon:
                var area = terrainData.GetAlphamaps(w - drawSize / 2, h - drawSize / 2, drawSize, drawSize);

                // Loop over the drawSquare pixels and populate the fill area:
                for (int x = 0; x < drawSize; x++)
                {
                    for (int y = 0; y < drawSize; y++)
                    {
                        // Apply both components of the texture:
                        for (int z = 0; z < layers; z++)
                        {
                            if (z == 1)
                            {
                                area[x, y, z] += textureData[x * drawSize + y].a;
                            }
                            else
                            {
                                area[x, y, z] -= textureData[x * drawSize + y].a;
                            }
                        }
                    }
                }
                // Draw the new texture over the area:
                // terrainData.SetAlphamaps(w - drawSize / 2, h - drawSize / 2, area);

                int wOut = w - drawSize / 2;
                int hOut = h - drawSize / 2;

                // Aesthetic delay:
                StartCoroutine(DrawTextureDelay(wOut, hOut, area, secs));
            }
        }
    }

    IEnumerator DrawTextureDelay(int w, int h, float[,,] area, float s)
    {
        yield return new WaitForSeconds(s);
        terrainData.SetAlphamaps(w, h, area);
    }
}