using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaneBehavior : MonoBehaviour {

	void OnTriggerEnter(Collider col){
		
		if(col.gameObject.name == "Sphere"){
			GameObject.Find ("AudioManager").GetComponent<AudioSource> ().Play ();
		}
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {


	}
		
}
