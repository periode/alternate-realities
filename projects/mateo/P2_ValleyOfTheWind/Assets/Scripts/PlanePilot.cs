using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanePilot : MonoBehaviour
{
	[Header("VR Settings")]
	public Transform head;
	public SteamVR_TrackedObject leftHand;
	public SteamVR_TrackedObject rightHand;
	public float initHeight = 0f;
	private float checkInitHeight = 0f;

	static PlanePilot _instance;

	public static PlanePilot Instance {
		get { return _instance; }
	}

	[Header("Flight Settings")]
	public float flyingSpeed = 0.0f;
	public float distanceToGround = 1f;
	public LayerMask mask;

	[Header("Wind Settings")]
	[Range (0, 1)]
	public float maxVolume = 1f;
	[Range(0, 0.1f)]
	public float fadeSpeed = 0.02f;
	AudioSource windSource;

	[Header("Tilt Settings")]
	[Range(0, 30)]
	public float maxTilt = 15f;
	private bool startGame = false;


	//For tutorial



	void Start ()
	{
		Debug.Log ("plane pilot script added to:" + gameObject.name);
		_instance = this;

		windSource = GetComponent<AudioSource> ();
	}



	void Update ()
	{
		// every frame we are moving forward
		transform.position += transform.forward * Time.deltaTime * flyingSpeed;

		// get left and right controller's y position
		float leftDir = leftHand.transform.localPosition.y; //- head.position;
		float rightDir = rightHand.transform.localPosition.y; //- head.position;

		//Initial calibration for controller height
		if (checkInitHeight == 50) {
			initHeight = (leftDir + rightDir) / 2;
			checkInitHeight++;
		} else if (checkInitHeight < 60) {
			checkInitHeight++;
		}

		// determine upward or downward movement based on the controllers relative position to initial height
		float dir = ((leftDir + rightDir) / 2.0f) - initHeight;

		// turn left or right based on the relative position of left and right hands
		float turnControl = leftDir - rightDir;

		//Movement through rotation
		transform.Rotate (-dir, 0f, 0f, Space.Self);
		transform.Rotate (0f, turnControl, 0f, Space.World);

		// Left Controller Button actions
		//To recalibrate controller height
		var lDevice = SteamVR_Controller.Input ((int)leftHand.index);
		if (lDevice.GetPressDown (SteamVR_Controller.ButtonMask.Grip)) {
			initHeight = ((leftDir + rightDir) / 2.0f);
		}

		//To change flying speed
		float targetSpeed =  flyingSpeed;
		flyingSpeed = Mathf.Lerp (flyingSpeed, targetSpeed, 0.02f);
		if (startGame == false) {
			targetSpeed = 0;
		} else if (lDevice.GetPress (SteamVR_Controller.ButtonMask.Touchpad)) {
			targetSpeed = 0f;
		} else if (lDevice.GetPress (SteamVR_Controller.ButtonMask.Trigger)) {
			targetSpeed = 50f;
		} else {
			targetSpeed = 15f;
		}

		//to start game
		lDevice.GetPressUp (SteamVR_Controller.ButtonMask.Trigger){
			startGame = true;
		}


		//Raycast terrain for collisions
		RaycastTerrain ();

		//tilt glider on turn
		TiltGlider (turnControl);

		//Wind volume on speed change
		AdjustWindVolume ();
	}

	// Our terrain is now a mesh so we use a raycast to figure out our distance to the ground
	void RaycastTerrain() {
		Ray ray = new Ray (transform.position, Vector3.down);
		RaycastHit hit;

		if (Physics.Raycast(ray, out hit, distanceToGround, mask)) {
			float height = hit.distance;
			transform.Translate (0, distanceToGround - height, 0);
		}
	}

	// Utility to convert angles from [0, 360) to (-180, 180]
	float SignedAngle(float angle) {
		return angle >= 180 ? angle - 360 : angle;
	}

	// Utility to determine when to allow tilting
	bool ShouldWeTilt(float currentAngle, float turnAmount) {
		// Convert angle to range (-180, 180]
		float angle = SignedAngle(currentAngle);
		// Check if we are past our maximum tilt
		if ((angle >= maxTilt) || (angle <= - maxTilt)) {
			// Allow rotation only if we are trying to rotate back to the center
			// In mathspeak, this will be the case if our turn and our current angle have the same sign
			// You would think otherwise, but you'd be wrong
			// (Either we messed up a sign, or it's a result of Unity being left-handed)
			// Debug.Log("Angle: " + angle + " :: Turn: " + turnAmount);
			return Mathf.Sign (angle) == Mathf.Sign (turnAmount);
		}

		// Default to true
		return true;
	}

	// Tilt glider as we turn
	void TiltGlider(float turn) {
		float z = turn; 

		// Find model - could be test or final version
		Transform model = transform.Find("Glider Model");
		if (!model) {
			model = transform.Find ("Glider Model");
		}

		if (model) {
			// Retrieve our local z rotation
			float tiltAngle = model.localRotation.eulerAngles.x;

			// Tilt only if allowed
			if (ShouldWeTilt(tiltAngle, z)) {
				transform.Find ("Glider Model").Rotate (-z, 0f, 0f, Space.Self);
				model.Rotate (-z, 0f, 0f, Space.Self);
			} 
		} else {
			// Warn on missing model
			Debug.LogWarning ("Glider model not found!");
		}
	}

	// Adjust wind SFX volume depending on our speed
	void AdjustWindVolume ()
	{
		float factor = flyingSpeed / 50f; // Normalize our flying speed relative to our max speed
		float targetVolume = Mathf.Lerp (0, maxVolume, factor); // Lerp between no sound and full sound
		windSource.volume = Mathf.Lerp (windSource.volume, targetVolume, fadeSpeed); // Lerp every frame towards our target volume
	}
}
