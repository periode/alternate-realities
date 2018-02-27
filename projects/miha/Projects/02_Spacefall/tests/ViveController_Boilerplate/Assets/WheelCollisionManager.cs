using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCollisionManager : MonoBehaviour {

	// Use this for initialization
	void OnCollisionEnter(Collision other){
		Debug.Log ("we have a collision!!!");
//		Debug.Log (other.name);
	}
}
