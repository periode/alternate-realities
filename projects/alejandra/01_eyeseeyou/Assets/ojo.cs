using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ojo : MonoBehaviour {

	Transform target;
	public float rotationSpeed = 0.01f;
	public float shakeSpeed = 10.0f;
	public float shakeAmount = 1.0f;
	float posX;

	// Use this for initialization
	void Start () {
		target = GameObject.Find ("Main Camera").transform;
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 direction = target.position - transform.position;
		Quaternion rotation = Quaternion.LookRotation (direction);
		transform.rotation = Quaternion.Lerp (transform.rotation, rotation, rotationSpeed);

	}
		
}
