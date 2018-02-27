using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCollisionManager : MonoBehaviour {

	bool stage_wheel_rotation = false;
	bool commit_wheel_rotation = false;

	// Use this for initialization
	void OnCollisionEnter(Collision other){
		
		if (other.gameObject.name == "Controller (right)" || other.gameObject.name == "Controller (left)") {
			stage_wheel_rotation = true;
		}
//		Debug.Log ("we have a collision!!! The object that hit the wheel is:" + other.gameObject.name);
//		Debug.Log (other.name);
	}

	void OnCollisionStay(Collision collider){

		if (collider.gameObject.name == "Controller (right)" || collider.gameObject.name == "Controller (left)") {
			
			bool wheel_is_grabbed = GameObject.Find ("GameManager").gameObject.GetComponent<Interactions> ().DetectSimultaneousTriggerPress ();
			if (wheel_is_grabbed && stage_wheel_rotation) {
				//rotate
				Debug.Log("rotate!!!!");
			}
		}
			

	}

	void OnCollisionLeave(Collision collider){
		Debug.Log ("collision is persistent");
		if (collider.gameObject.name == "Controller (right)" || collider.gameObject.name == "Controller (left)") {
			stage_wheel_rotation = false;
		}
	}
}
