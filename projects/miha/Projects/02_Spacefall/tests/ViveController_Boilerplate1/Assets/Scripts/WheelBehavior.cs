using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBehavior : MonoBehaviour {

	GameObject Car;
	GameObject Player;
	float time = 0;
	public bool rotateWheel = false;
//	float speed 
	// Use this for initialization
	void Start () {
		Car = GameObject.Find ("Car");
		Player = GameObject.Find ("[CameraRig]");
//		StartCoroutine(RotateForFiveSeconds ());
	}
	
	// Update is called once per frame
	void Update () {
//		this.transform.Rotate (0, 0, Time.deltaTime,Space.Self);
//		car.transform.Rotate(0, Time.deltaTime, 0,Space.Self);
//		player.transform.Rotate(0, Time.deltaTime, 0,Space.Self);
//		time += Time.deltaTime;
//		transform.Rotate(Vector3.right * Time.deltaTime*10);
//		Debug.Log ("time is:" + time + " and delta time is:" + Time.deltaTime);
	}

	public bool GetWheelRotationStatus(){
		return rotateWheel;
	}

	public void SetWheelRotationStatus(bool _newStatus){
		rotateWheel = _newStatus;
	}


	public void StartRotation(){
		//IEnumerator?

	
	}

	public void StopRotation(){

	}
}
