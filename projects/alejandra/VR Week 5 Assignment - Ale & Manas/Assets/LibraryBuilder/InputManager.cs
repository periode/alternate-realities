using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour {

	public LayerMask layerMask;
	public float raycastDistance;

	private SteamVR_Controller.Device device;
	public SteamVR_TrackedObject controller;



	//This script should be attached to each controller (Controller Left or Controller Right)

	// Getting a reference to the controller GameObject
	private SteamVR_TrackedObject trackedObj;
	// Getting a reference to the controller Interface
	private SteamVR_Controller.Device Controller;

	void Awake()
	{
		// initialize the trackedObj to the component of the controller to which the script is attached
		trackedObj = GetComponent<SteamVR_TrackedObject>();


	}

	// Update is called once per frame
	void Update () {
		Controller = SteamVR_Controller.Input((int)trackedObj.index);


		// Getting the Touchpad Axis
		if (Controller.GetAxis() != Vector2.zero)
		{
			Debug.Log(gameObject.name + Controller.GetAxis());
		}

		// Getting the Trigger press
		if (Controller.GetHairTriggerDown())
		{
			Debug.Log(gameObject.name + " Trigger Press");
			Raycasting ();

		}

		// Getting the Trigger Release
		if (Controller.GetHairTriggerUp())
		{
			Debug.Log(gameObject.name + " Trigger Release");
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
		

	void Raycasting(){
		Vector3 fwd = transform.TransformDirection (Vector3.forward); //what is the direction in front of us
		RaycastHit hit = new RaycastHit ();

		if(Physics.Raycast(transform.position, fwd, out hit, raycastDistance, layerMask)){
			Debug.Log ("hit object: " + hit.collider.gameObject.name);
			if(hit.collider.gameObject.name == "Book(Clone)" ){
				Debug.Log("Hit a book. Ouch!");
				GameObject right_controller = GameObject.Find("RightController");
				Vector3 right_controller_pos = new Vector3 (0, 0, 0);
				hit.collider.gameObject.transform.position = right_controller_pos;
				right_controller_pos = right_controller.transform.position;



				//hit.collider.gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, smooth);
				//Destroy (hit.collider.gameObject, 0.5f);

				//hit.collider.gameObject.GetComponent<Renderer> ().enabled = false;

			}
		}
	}
}