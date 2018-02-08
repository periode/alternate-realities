using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {

	public GameObject myCube;
	public GameObject mySphere;
	public GameObject myIso;

	GameObject[] cubes;
	GameObject[] tower;

	public int numCubes;
	public int towerHeight;
	// Use this for initialization
	void Start () {

		int k = 0;
		float x, y, z;
		cubes = new GameObject[numCubes * numCubes];
		tower = new GameObject[towerHeight];
		for (int i = 0; i < numCubes; i++) {
			for (int j = 0; j < numCubes; j++) {
				x = i - numCubes / 2;
				z = j - numCubes / 2;
				y = -5;
				cubes [k] = GameObject.Instantiate(myIso, new Vector3(x, y, z), Quaternion.identity);
				k++;

			}
		}

		y = -5;
		float[] cornersX = {-numCubes/2, numCubes/2, numCubes/2, -numCubes/2};
		float[] cornersY = {-numCubes/2, -numCubes/2, numCubes/2, numCubes/2};

		for (int a = 0; a < 4; a++) {
			
			for (int i = 0; i < towerHeight; i++) {
				x = cornersX [a];
				z = cornersY [a];
				tower [i] = GameObject.Instantiate (mySphere, new Vector3 (x, y, z), Quaternion.identity);
				tower[i].GetComponent<MeshRenderer> ().material.color = new Color(0.4f, 0.4f, 0.4f);
				y++;
			}
			y = -5;
		}

	}
	float u = 1.73f;
	int c = 0;
	// Update is called once per frame
	void Update () { 
		c++;
		if (c > 20) {
			u++;
			c = 0;
		}
		float r, g, b;

		int x, y, z, k=0;

		for (int i = 0; i < numCubes; i++) {
			for (int j = 0; j < numCubes; j++) {
				x = i - numCubes / 2;
				z = j - numCubes / 2;
				y = -5;
				r = Mathf.PerlinNoise (0.3f*i, 0.5f*1.7f*(numCubes-i));
				g = Mathf.PerlinNoise (0.1f*j*2f*u, 0.2f*0.8f*(numCubes-j*2));
				b = Mathf.PerlinNoise (0.1f*i*j*4f*u, u*0.2f*2.2f*i);
				float newY = Mathf.PerlinNoise (0.1f * i*u, 0.3f * j*u);
				cubes [k].GetComponent<MeshRenderer> ().material.color = new Color (r, g, b);
				cubes [k].GetComponent<Transform> ().position = new Vector3(x, newY-5, z);
				k++;

			}
		}


		
	}
}
