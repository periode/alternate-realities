using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour {

	public GameObject preFab1;
	public GameObject preFab2;
	public GameObject preFab3;
	public GameObject preFab4;
	public GameObject preFab5;
	public GameObject preFab6;

	Vector3 newPos;

	GameObject [] preFabs;
	GameObject [] allPreFabs;

	public int numPreFabs;
	public int eachPreFab;
	public int counter;
	public float max ;
	public float min ;

	// Use this for initialization
	void Start () {

//		newPos = new Vector3 (Random.Range (minf, max), Random.Range (minf, max), Random.Range (minf, max));
		max = 20.0f;
		min = -20.0f;
		counter = 0;

		numPreFabs = 5;
		eachPreFab = 20;
		preFabs = new GameObject[numPreFabs];
		allPreFabs = new GameObject[numPreFabs*eachPreFab];;

		preFabs[0]=preFab1;
		preFabs[1]=preFab2;
		preFabs[2]=preFab3;
		preFabs[3]=preFab4;
		preFabs[4]=preFab5;
//		preFabs[5]=preFab6;

		for (int i = 0; i < numPreFabs; i++) {
			for (int j = 0; j < eachPreFab; j++) {

				newPos = new Vector3 (Random.Range (min, max), Random.Range (min, max), Random.Range (min, max));
				transform.position = newPos;
				allPreFabs[counter]= GameObject.Instantiate (preFabs [i] as GameObject, newPos, Quaternion.identity);
				counter++;
			}
//			for (int j = 0; j < eachPreFab; j++) {
//				float xPos = Random.Range (min, max);
//				float yPos = Random.Range (min, max);
//				float zPos= Random.Range (min, max);
//				allPreFabs[counter] = GameObject.Instantiate (preFabs[i] as GameObject, new Vector3 (xPos, yPos, zPos), 
//					Quaternion.identity);
//				counter++;
//				allPreFabs [counter].transform.SetParent (this.transform);
//			}
		}

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
