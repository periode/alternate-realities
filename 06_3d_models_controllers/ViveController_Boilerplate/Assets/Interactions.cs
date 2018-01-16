using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void HandleLeftTriggerPressed(){
		Debug.Log ("handling the left trigger press from the GameManager script");
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = Color.red;
	}

	public void HandleRightTriggerPressed(){
		Debug.Log ("handling the right trigger press from the GameManager script");
		GameObject.Find ("Sphere").GetComponent<Renderer> ().material.color = Color.red;
	}
}
