using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {

	public GameObject myPrefab;
	public GameObject myPrefab1;
	public GameObject myPrefab2;
	public Vector3 newPos;
	public 	List<GameObject> shapes = new List<GameObject>();

	// Use this for initialization
	void Start () {
		
		shapes.Add(myPrefab as GameObject);
		shapes.Add(myPrefab1 as GameObject);
		shapes.Add(myPrefab2 as GameObject);

	}
	
	// Update is called once per frame
	void Update () {
//		myPrefab = shapes[Random.Range(0,4)];
		int random = Random.Range(0,3);
		newPos = new Vector3 (Random.Range(-10.0f,10.0f),Random.Range(-10.0f,10.0f), Random.Range(-10.0f,10.0f));
		GameObject.Instantiate (shapes[random] as Object, newPos, Quaternion.identity);


	}
}
