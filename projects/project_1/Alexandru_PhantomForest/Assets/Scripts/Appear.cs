using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Appear : MonoBehaviour {

	public float speed;
	public Transform target;
	bool showingUp = false;

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (0, 0, 0);
	}
	
	// Update is called once per frame
	void Update () {
		if (showingUp) {
			transform.localScale = new Vector3 (0.1f, 0.1f, 0.1f);
			float step = speed * Time.deltaTime;
			transform.position = Vector3.MoveTowards(transform.position, target.position, step);
		}
	}
		


	public void showUp() {
		showingUp = true;
	}
}
