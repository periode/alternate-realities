using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class BeingController : Photon.MonoBehaviour {

	float speed = 0.1f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(photonView.isMine)
			Movement ();
		
	}

	void Movement(){
		//move forward
		if (Input.GetKey (KeyCode.UpArrow))
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + speed);

		//move back
		if (Input.GetKey (KeyCode.DownArrow)) 
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - speed);

		//move right
		if (Input.GetKey (KeyCode.RightArrow))
			transform.position = new Vector3 (transform.position.x + speed, transform.position.y, transform.position.z);

		//move left
		if (Input.GetKey (KeyCode.LeftArrow))
			transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);
	}
}
