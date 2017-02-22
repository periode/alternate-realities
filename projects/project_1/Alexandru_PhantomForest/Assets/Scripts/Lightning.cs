using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour {

	int isLightning = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (isLightning > 0) {
			GetComponent<Light> ().intensity = Mathf.Sin (Time.time * 20) * 5;
			isLightning -= 1;
		} else {
			GetComponent<Light> ().intensity = 0;
		}
	}

	public void show() {
		isLightning = 50;
	}
}
