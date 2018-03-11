using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	//This script should be attached to each controller (Controller Left or Controller Right)

	// Getting a reference to the controller GameObject
	private SteamVR_TrackedObject trackedObj;
	// Getting a reference to the controller Interface
	private SteamVR_Controller.Device Controller;

	private Interactions InteractionManager;

	public bool triggerPress = false;
	private bool handOnTheWheel;


	void Awake()
	{
		// initialize the trackedObj to the component of the controller to which the script is attached
		trackedObj = GetComponent<SteamVR_TrackedObject>();
		InteractionManager = GameObject.Find ("GameManager").GetComponent<Interactions> ();
	}

	// Update is called once per frame
	void Update () {

		Controller = SteamVR_Controller.Input ((int)trackedObj.index);

		// Getting the Touchpad Axis
		if (Controller.GetAxis() != Vector2.zero)
		{
			Debug.Log(gameObject.name + Controller.GetAxis());
		}

		// Getting the Trigger press
		if (Controller.GetHairTriggerDown())
		{
			triggerPress = true;
//			Debug.Log(gameObject.name + " Trigger Press");
//			if (gameObject.name == "Controller (right)") {
//				InteractionManager.right_trigger_press = true;
//			} else if (gameObject.name == "Controller (left)") {
//				InteractionManager.left_trigger_press = true;
//			}
//
			//InteractionManager.ActiveHairlineTrigger(gameObject.name);

			//if we're on controller right
//			if(gameObject.name == "Controller (right)"){
//				Debug.Log ("both at the same time!!!");
//
//				//find the interaction manager and call the function we wrote
//
//				//GameObject.Find ("InteractionManager").GetComponent<Interactions> ().HandleRightTriggerPressed ();
//			}
		}

		// Getting the Trigger Release
		if (Controller.GetHairTriggerUp())
		{
			triggerPress = false;
//			if (gameObject.name == "Controller (right)") {
//				InteractionManager.right_trigger_press = false;
//			} else if (gameObject.name == "Controller (left)") {
//				InteractionManager.left_trigger_press = false;
//			}
//			Debug.Log(gameObject.name + " Trigger Release");
		}

		// Getting the Grip Press
		if (Controller.GetPressDown(SteamVR_Controller.ButtonMask.Grip))
		{
			Debug.Log(gameObject.name + " Grip Press");
		}

		// Getting the Grip Release
		if (Controller.GetPressUp(SteamVR_Controller.ButtonMask.Grip))
		{
			Debug.Log(gameObject.name + " Grip Release");
		}
	}


	public bool GetTriggerPress(){
		return triggerPress;
	}

	public void HandIsOnTheWheel(bool _newStatus){
		handOnTheWheel = _newStatus;
	}

	public bool GetHandOnTheWheel(){
		return handOnTheWheel;
	}

//	public static 
}