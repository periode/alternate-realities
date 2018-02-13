using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {

    public GameObject myCube;
    public int numCubes;

    GameObject[] all_cubes;

    // Use this for initialization
    void Start()
    {
        all_cubes = new GameObject[numCubes*numCubes];
        int k = 0;

        for (int j = 0; j < numCubes; j++)
        {
            for (int i = 0; i < numCubes; i++)
            {
                // Challenge: Generate calmer terrain below a threshold.
                // Ex: If y value is below 0.6 there should be hilly valleys, otherwise peaks.

                // Backing up: first generate different terrain each time.
                // What effects Perlin Noise?
                // - The inner multipliers should be between 0 and 0.1, ideally on the low end.
                // - Multiplier at the end exaggerates the scale of the whole mapping.
                float newy = Mathf.PerlinNoise(j*0.02f, i*0.02f) * 25.0f;

                all_cubes[k] = GameObject.Instantiate(myCube, new Vector3(i-numCubes/2, newy, j-numCubes/2), Quaternion.identity);
                k++;
            }
        }
    }
	
	// Update is called once per frame
//	void Update () {
		//for(int i=0; i < all_cubes.Length; i++)
  //      {
  //          // float newx = Mathf.Sin(i + Time.time);
  //          //float newy = Mathf.PerlinNoise(Time.time, i);
  //          float newy = Mathf.PerlinNoise(i+0.1f, i*0.1f+0.5f)*10.0f;

  //          all_cubes[i].transform.position = new Vector3(all_cubes[i].transform.position.x,
  //              newy,
  //              all_cubes[i].transform.position.z);
  //      }
	//}
}


// ctrl+k+c = comment
// ctrl+k+u = uncomment
