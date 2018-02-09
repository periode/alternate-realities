using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public PipeSystem pipeSystem;
	public float velocity;

	public float distanceTraveled;
	private float deltaToRotation;
	private float systemRotation;

	private Pipe currentPipe;
	private Transform world, rotater;

	private float worldRotation, avatarRotation;

	public float rotationVelocity;






	private void Start() {
		world = pipeSystem.transform.parent;
		rotater = transform.GetChild (0);
		currentPipe = pipeSystem.SetupFirstPipe ();
//		deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
		SetupCurrentPipe();
	}



	private void Update() {
		velocity = 1 + Mathf.Sin (2*Time.time);
		float delta = velocity * Time.deltaTime;
		distanceTraveled += delta;
		systemRotation += delta * deltaToRotation;

		if (systemRotation >= currentPipe.CurveAngle) {
			delta = (systemRotation - currentPipe.CurveAngle) / deltaToRotation;
			currentPipe = pipeSystem.SetupNextPipe ();
			SetupCurrentPipe ();
			deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
			systemRotation = delta * deltaToRotation;
		}

		pipeSystem.transform.localRotation = Quaternion.Euler (0f, 0f, systemRotation);
		UpdateAvatarRotation ();

	}


	private void UpdateAvatarRotation () {
		avatarRotation += rotationVelocity * Time.deltaTime * Input.GetAxis ("Horizontal");
		if (avatarRotation < 0f) {
			avatarRotation += 360f;
		}
		else if (avatarRotation >= 360f) {
			avatarRotation -= 360f;
		}
		rotater.localRotation = Quaternion.Euler(avatarRotation, 0f, 0f);
	}


	private void SetupCurrentPipe () {
		deltaToRotation = 360f / (2f * Mathf.PI * currentPipe.CurveRadius);
		worldRotation += currentPipe.RelativeRotation;
		if (worldRotation < 0f) {
			worldRotation += 360f;
		} else if (worldRotation >= 360f) {
			worldRotation -= 360f;
		}
		world.localRotation = Quaternion.Euler (worldRotation, 0f, 0f);
	}

}
