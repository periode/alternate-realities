using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour1 : MonoBehaviour {

	public float mySpeed; 

	bool isSphereViolet = false; 

	// Use this for initialization
	void Start () {

		Debug.Log (transform.position);

		transform.position = new Vector3 (0, 0, 0);
		
	}
	
	// Update is called once per frame
	void Update () {

		GetComponent<Transform> ().position = new Vector3 (transform.position.x + mySpeed, transform.position.y, transform.position.z);

		
	}
}
