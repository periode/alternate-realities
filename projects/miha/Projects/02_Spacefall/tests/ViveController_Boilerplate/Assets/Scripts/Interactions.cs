using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour {

	public bool left_trigger_press = false;
	public bool right_trigger_press = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		left_trigger_press = false;
//		right_trigger_press = false;
		Debug.Log("simultaneous press is:" + DetectSimultaneousTriggerPress());
		
	}

	public void HandleLeftTriggerPressed(){
		Debug.Log ("handling the left trigger press from the GameManager script");
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = Color.red;
	}

	public void HandleRightTriggerPressed(){
		Debug.Log ("handling the right trigger press from the GameManager script");
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = Color.red;
	}



	private bool DetectSimultaneousTriggerPress(){

		if (left_trigger_press == true && right_trigger_press == true) {
//			Debug.Log ("the triggers are pressed at the same time!");
			return true;
		} else {
//			Debug.Log ("nothing");
			return false;
		}
	}
		


		


}
