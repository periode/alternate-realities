using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Script : MonoBehaviour {

    public int numCubes;
    public GameObject cube;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < numCubes; i++)
        {
            for (int j = 0; j < numCubes; j++)
            {
                for (int k = 0; k < numCubes; k++)
                {
                    Instantiate(cube, new Vector3(i * 1.5f, j * 1.5f, k * 1.5f), Quaternion.identity);
                }
            }
        }


	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
