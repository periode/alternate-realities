using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spherebehavior : MonoBehaviour {
	int myNumber;
	// declaring variable myNumber as an int

	string myMessage;
	// Use this for initialization
	void Start () {
		myNumber = 8;
		myMessage = "hello";
		Debug.Log (myMessage);
		myMessage = "hello again";
		Debug.Log (myNumber);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
