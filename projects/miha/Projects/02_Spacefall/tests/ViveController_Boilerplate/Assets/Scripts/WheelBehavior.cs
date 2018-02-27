using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WheelBehavior : MonoBehaviour {

	GameObject car;
	float time = 0;
//	float speed 
	// Use this for initialization
	void Start () {
		car = GameObject.Find ("Car");
//		StartCoroutine(RotateForFiveSeconds ());
	}
	
	// Update is called once per frame
	void Update () {
//		this.transform.Rotate (0, 0, Time.deltaTime,Space.Self);
//		car.transform.Rotate(0, Time.deltaTime, 0,Space.Self);
//		time += Time.deltaTime;
//		transform.Rotate(Vector3.right * Time.deltaTime*10);
//		Debug.Log ("time is:" + time + " and delta time is:" + Time.deltaTime);
	}

	IEnumerator RotateForFiveSeconds(){

		float elapsed_time = 0;
		float duration = 10.0F;
		float wheel_rotation = 0;

		while(elapsed_time < duration){
			elapsed_time += Time.deltaTime;
			this.transform.Rotate (0, 0, 1,Space.Self);
			car.transform.Rotate(0, 1, 0,Space.Self);
			yield return null;
		}
	} 
}
