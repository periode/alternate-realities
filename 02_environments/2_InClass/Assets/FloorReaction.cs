using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorReaction : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		Debug.Log ("collided with " + col.gameObject.name);
	}

	void OnCollisionEnter(Collision col){
		Debug.Log ("collision with " + col.gameObject.name);
	}
}
