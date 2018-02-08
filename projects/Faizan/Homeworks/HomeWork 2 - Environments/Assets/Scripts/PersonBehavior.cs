using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonBehavior : MonoBehaviour {

	Rigidbody rb;
	public float speed = 5f;
	public float turnSpeed = 5f;


	// Use this for initialization
	void Start () {

		rb = GetComponent<Rigidbody>();

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (Input.GetKey(KeyCode.W))
		{
			rb.AddForce(new Vector3(0f, 0f, speed));
		}
		if (Input.GetKey(KeyCode.S))
		{
			rb.AddForce(new Vector3(0f, 0f, -speed));
		}
		if (Input.GetKey(KeyCode.D))
		{
			rb.AddTorque(new Vector3(0f, turnSpeed, 0f));
		}
		if (Input.GetKey(KeyCode.A))
		{
			rb.AddTorque(new Vector3(0f, -turnSpeed, 0f));
		}


	}
}
