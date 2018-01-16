using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour {

	public Vector3 newPos;

	// Use this for initialization
	void Start () {
		// ------------ WHAT'S UP WORLD
		// -- this is a standard debug / print / console log
//		Debug.Log ("Hey there, fellas");

		// ------------ CHANGING THE COLOR
		// -- the two lines below are equivalent
//		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
		GetComponent<MeshRenderer>().material.color = Color.black;



		// ------------ CHANGING THE POSITION
		// -- here we initialize the variable
		newPos = new Vector3 (1, 0, 1);

		// -- the three lines below are equivalent
//		this.gameObject.GetComponent<Transform> ().position = newPos;
//		GetComponent<Transform> ().position = newPos;
		transform.position = newPos;
	}
	
	// Update is called once per frame
	void Update () {
		// ------------ DEBUG STATEMENT EVERY FRAME
//		Debug.Log ("Let me say it again, fellas");

		// ------------ RANDOMIZING THE COLOR
		// -- Random.value returns a random number between 0 and 1
		if(Random.value > 0.9){
			GetComponent<MeshRenderer>().material.color = Color.black;
		}else{
			GetComponent<MeshRenderer>().material.color = Color.white;
		}

		// ------------ MOVING THE CUBE SMOOTHLY
		Vector3 temp; //we declare a temporary variable
		temp = transform.position; //we store the current position of the cube
		temp.x += 0.01f; //we increase the value of the x component of the cube's position by a little bit
		transform.position = temp; //finally, we assign our modified value to the cube's position
	}
}
