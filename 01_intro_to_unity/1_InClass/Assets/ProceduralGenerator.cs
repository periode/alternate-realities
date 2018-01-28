using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {

	public GameObject myCube;
	public int numCubes;

	GameObject[] all_cubes;

	// Use this for initialization
	void Start () {
		
		all_cubes = new GameObject[numCubes*numCubes];



		int k = 0;

		for (int j = 0; j < numCubes; j++) {
			for (int i = 0; i < numCubes; i++) {

				float newy = Mathf.PerlinNoise (j*0.1f, i*0.1f)*10.0f;
				all_cubes [k] = GameObject.Instantiate (myCube, new Vector3 (i-numCubes/2, newy, j-numCubes/2), Quaternion.identity);
				k++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		

	}
}
















