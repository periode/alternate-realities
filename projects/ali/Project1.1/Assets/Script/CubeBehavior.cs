using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour {

	public Vector3 newPos;

	// Use this for initialization
	void Start () {
		newPos = new Vector3 (1.0f, -0.5f, 3.0f);
		transform.position = newPos;
	}
		
	void Update () {
		Vector3 temp; 
		temp = transform.position; 
		temp.x += 0.01f; 
		transform.position = temp; 
	}
}

