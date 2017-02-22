using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour {

	public float redScale;
	public bool userHasTriggered = false;
	Vector3 speed;

	// Use this for initialization
	void Start () {
		speed = new Vector3 (0.0f, 0.01f, 0.0f);
	}
	
	// Update is called once per frame
	void Update () {

		//if this is true

		if(userHasTriggered){
			//start moving in the direction of the speed variable
			GetComponent<Transform> ().Translate (speed);
		}

//		GetComponent<Renderer> ().material.color = new Color((Mathf.Sin(Time.time*redScale)+1)*0.5f, 1.0f, 0.0f);
	}
}
