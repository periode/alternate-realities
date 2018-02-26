using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{

    public GameObject cube;

    GameObject[] all_cubes;

    public int numCubes= 10000;
    public int numRow = 3;

    // Use this for initialization
    void Start()
    {


        all_cubes = new GameObject[numCubes];



        double height = 10; 
        int k = 0;

        for (int f = 0; f < 1000; f += 35)
        {
            height = Random.Range(5, 20);
            for (int p = 0; p < 1000; p += 35)
            {
                height = Random.Range(5, 25);

                for (int x = 0; x < numRow; x++)
                {


                    for (int z = 0; z < numRow; z++)
                    {

                        for (int y = 0; y < height; y++)
                        {

                           all_cubes[k] = GameObject.Instantiate(cube, new Vector3(x + p, y, z + f), Quaternion.identity);
                            k++;

                        }
                    }
                }

            }

        }
/*
        for (int a = 15; a < 25; a++)
        {
            for (int b= 0; b < numRow; b++)
            {

                for (int c = 0; c < numRow; c++)
                {

                    all_cubes[k] = GameObject.Instantiate(cube, new Vector3(a, b, c), Quaternion.identity);
                    k++;

                }
            }
        }

        */


    }

    // Update is called once per frame
    void Update()
    {
     /*   for (int i = 0; i < all_cubes.Length; i++)
        {
            float newy = Mathf.PerlinNoise(Time.time, i);

                all_cubes[i].transform.position = new Vector3(all_cubes[i].transform.position.x, newy,
                all_cubes[i].transform.position.z);
        }*/
    }
}