using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelCollisionManager : MonoBehaviour {

	// access interaction manager
	private Interactions InteractionManager;
	// boolean that determines whether the user can rotate the wheel
	private WheelBehavior wheel;


	// Use this for initialization

	void Start(){
		InteractionManager = GameObject.Find ("GameManager").GetComponent<Interactions> (); 
		wheel = this.GetComponent<WheelBehavior> ();
	}



	// use this for when the controllers enter the wheel collider

	void OnTriggerEnter(Collider collider){
//		Debug.Log ("collision enter, collider is:" + collider.gameObject.name);
		if (collider.gameObject.name == "handLeft") {
			InteractionManager.leftController.HandIsOnTheWheel (true);
		} else if (collider.gameObject.name == "handRight") {
			InteractionManager.rightController.HandIsOnTheWheel (true);
		}

	}

	// use this for when the controllers stay on the wheel collider

	void OnTriggerStay(Collider collider){
//		Debug.Log ("collision stays, collider is:" + collider.gameObject.name);

		// if the collider is our astronaut glove, check if both controller triggers are pressed
		// if both controller triggers are pressed and at least one glove is 
		if (collider.gameObject.tag == "Hand") {
			
			bool triggersArePressed = GameObject.Find ("GameManager").gameObject.GetComponent<Interactions> ().DetectSimultaneousTriggerPress ();
			bool handsAreOnTheWheel = GameObject.Find ("GameManager").gameObject.GetComponent<Interactions> ().DetectHandsOnTheWheel ();

			if (handsAreOnTheWheel == true && triggersArePressed == true && wheel.GetWheelRotationStatus() == false) {
				Debug.Log ("rotate the wheel!");
				wheel.SetWheelRotationStatus (true);

				//find locations
				float leftControllerX = GameObject.Find("Controller (left)").transform.position.x;
				float rightControllerX = GameObject.Find("Controller (right)").transform.position.x;
				Debug.Log ("left controller x position is:" + leftControllerX + " and right controller x position is:" + rightControllerX);

//				wheelRotation = true;
			} else if (handsAreOnTheWheel && !triggersArePressed) {
				wheel.SetWheelRotationStatus (false);
//				wheelRotation = false;
			}
//			if (triggersArePressed) {
//				Debug.Log ("both triggers are pressed!");
//			};
//
//			if (handsAreOnTheWheel) {
//				Debug.Log ("both hands are on the wheel!");
//			};
//			
//			if (triggersArePressed && handsAreOnTheWheel) {
//			if (triggersArePressed) {
//				Debug.Log ("someone is grabbing the wheel!");

//				//rotate
//
//				// record init position
			}
//		}
			

	}


	// use this for when the controllers exit the wheel collider

	void OnTriggerExit(Collider collider){
		Debug.Log ("collision exit, collider is:" + collider.gameObject.name);
//		Debug.Log ("collision is persistent");
		if (collider.gameObject.tag == "Hand") {
//			stage_wheel_rotation = false;
			Debug.Log("collision is over!");
//			wheelRotation = false;
			wheel.SetWheelRotationStatus (false);
		}

		if (collider.gameObject.name == "handLeft") {
			InteractionManager.leftController.HandIsOnTheWheel (false);
		} else if (collider.gameObject.name == "handRight") {
			InteractionManager.rightController.HandIsOnTheWheel (false);
		}
	}
}
