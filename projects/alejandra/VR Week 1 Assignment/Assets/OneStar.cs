using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneStar : MonoBehaviour {

	float xpos = 0;
	float ypos = 0;
	float zpos = 0;
	float radius = 2;
	public float speed = 10f;

	GameObject[] stars_vertices; 
	// Use this for initialization
	void Start () {

		stars_vertices = new GameObject[30];
		int k = 0;

		for (float t = 0; t < Mathf.PI * 2; t += Mathf.PI / 4) {
			for (int i = 0; i < 3; i++) {
				xpos = radius * Mathf.Cos (t) * i + 1 / 3;
				ypos = radius * Mathf.Sin (t) * i + 1 / 3;
				stars_vertices [k] = GameObject.CreatePrimitive (PrimitiveType.Cube);
				stars_vertices [k].transform.SetParent (this.transform);
				stars_vertices [k].transform.localPosition = new Vector3(xpos, ypos, zpos);
				k++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, speed * Time.deltaTime);
	}
}
