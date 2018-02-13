using System.Collections;
using System.Collections.Generic;
using UnityEngine;
	

public class CreateStars : MonoBehaviour {


	public GameObject myStar;
	public int numStars;

	GameObject[] constellation;

	// Use this for initialization
	void Start () {
		constellation = new GameObject[numStars];

		for (int i = 0; i < numStars; i++) {
			float cons_x = Random.Range (-500, 500);
			float cons_y = Random.Range (-500, 500);
			float cons_z = Random.Range (-500, 500);
			constellation[i] = GameObject.Instantiate (myStar, Vector3.zero, Quaternion.identity);
			constellation [i].transform.position = new Vector3 (cons_x, cons_y, cons_z);
			constellation[i].transform.SetParent (this.transform);


		}

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime, Space.World);
	}
}
