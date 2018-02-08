using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {


	public GameObject myCube;
	public GameObject mySphere;
	public GameObject myIso;

	int num = 200;

	GameObject[] cubes;
	GameObject[] spheres;
	GameObject[] isos;

	// Use this for initialization
	void Start () {
		cubes = new GameObject[num];
		for (int i = 0; i < num; i++) {
			cubes [i] = GameObject.Instantiate (myCube, new Vector3 (Random.Range (-70, 70) * 1.0f, Random.Range (0, 10) * 1.0f, Random.Range (-70, 70) * 1.0f), Quaternion.identity);
			cubes [i].GetComponent<Renderer> ().enabled = false;
			cubes [i].GetComponent<MeshRenderer> ().material.color = new Color (Random.Range(0.6f, 1.0f), Random.Range(0.6f, 1.0f), 1.0f);
		}
		spheres = new GameObject[num];
		for (int i = 0; i < num; i++) {
			spheres [i] = GameObject.Instantiate (mySphere, new Vector3 (Random.Range (-70, 70) * 1.0f, Random.Range (0, 10) * 1.0f, Random.Range (-70, 70) * 1.0f), Quaternion.identity);
			spheres [i].GetComponent<Renderer> ().enabled = false;
			spheres [i].GetComponent<MeshRenderer> ().material.color = new Color (Random.Range(0.6f, 1.0f), Random.Range(0.6f, 1.0f), 1.0f);
		}
		isos = new GameObject[num];
		for (int i = 0; i < num; i++) {
			isos [i] = GameObject.Instantiate (myIso, new Vector3 (Random.Range (-70, 70) * 1.0f, Random.Range (0, 10) * 1.0f, Random.Range (-70, 70) * 1.0f), Quaternion.identity);
			isos [i].GetComponent<Renderer> ().enabled = false;
			isos [i].GetComponent<MeshRenderer> ().material.color = new Color (Random.Range(0.6f, 1.0f), Random.Range(0.6f, 1.0f), 1.0f);
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
