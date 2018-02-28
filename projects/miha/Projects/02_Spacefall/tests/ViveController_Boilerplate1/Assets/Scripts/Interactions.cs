using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour {

//	public bool leftTriggerPress = false;
//	public bool rightTriggerPress = false;

	public InputManager leftController;
	public InputManager rightController;
	bool foundControllers = false;

	// Use this for initialization
	void Start () {
//		StartCoroutine (FindControllers ());
//		leftController = GameObject.Find ("Controller (left)").gameObject.GetComponent<InputManager>();
//		rightController = GameObject.Find ("Controller (right)").gameObject.GetComponent<InputManager>();

	}
	
	// Update is called once per frame
	void Update () {
		if (!foundControllers) {
			FindControllers();
		};
//		bool answer = leftController.GetTriggerPress ();
//		Debug.Log (answer);
//		left_trigger_press = false;
//		right_trigger_press = false;
		if(foundControllers){
//			DetectSimultaneousTriggerPress();
//			Debug.Log(leftController.GetTriggerPress());
		}

		
	}

	public void HandleLeftTriggerPressed(){
		Debug.Log ("handling the left trigger press from the GameManager script");
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = Color.red;
	}

	public void HandleRightTriggerPressed(){
		Debug.Log ("handling the right trigger press from the GameManager script");
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = Color.red;
	}

	private void FindControllers(){
			if(leftController != null && rightController != null){
				foundControllers = true;
			}else{
				leftController = GameObject.Find ("Controller (left)").gameObject.GetComponent<InputManager>();
				rightController = GameObject.Find ("Controller (right)").gameObject.GetComponent<InputManager>();
			}
		}

	public bool DetectSimultaneousTriggerPress(){
//		Debug.Log ("left controller is pressed:" + LeftController.GetTriggerPress ());
		if (leftController.GetTriggerPress() == true && rightController.GetTriggerPress() == true) {
//			Debug.Log ("the triggers are pressed at the same time!");
			return true;
		} else {
//			Debug.Log ("nothing");
			return false;
		}
	}

	public bool DetectHandsOnTheWheel(){
		//		Debug.Log ("left controller is pressed:" + LeftController.GetTriggerPress ());
		if (leftController.GetHandOnTheWheel() == true && rightController.GetHandOnTheWheel() == true) {
			//			Debug.Log ("the triggers are pressed at the same time!");
			return true;
		} else {
			//			Debug.Log ("nothing");
			return false;
		}
	}


//	public bool DetectSimultaneousTriggerPress(){
//
//		if (leftTriggerPress == true && rightTriggerPress == true) {
//			//			Debug.Log ("the triggers are pressed at the same time!");
//			return true;
//		} else {
//			//			Debug.Log ("nothing");
//			return false;
//		}
//	}
//		


		


}
