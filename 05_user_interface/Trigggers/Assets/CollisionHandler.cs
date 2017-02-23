using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionHandler : MonoBehaviour {

	public GameObject myParticlePrefab;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider other){
		//we need an object, a position and a rotation
//		Instantiate()

		Instantiate (myParticlePrefab, other.transform.position, Quaternion.identity);
	}
}
