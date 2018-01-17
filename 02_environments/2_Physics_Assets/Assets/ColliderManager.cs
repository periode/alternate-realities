using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter(Collider col){
		Debug.Log ("something entered "+col);
		Debug.Log ("we collided with " + col.gameObject.name);

		if(col.gameObject.tag == "Colliding"){
			GameObject.Find ("AudioManager").GetComponent<AudioSource> ().Play ();
		}
	}
}
