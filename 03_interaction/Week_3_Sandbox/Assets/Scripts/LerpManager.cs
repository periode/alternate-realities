using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpManager : MonoBehaviour {

	float lerpAmt = 0f;
	float lerpSpeed = 0.001f;

	Vector3 startPos;
	Vector3 destPos;

	// Use this for initialization
	void Start () {
		startPos = transform.position;
		destPos = Vector3.zero;
	}
	
	// Update is called once per frame
	void Update () {

		// Linear Interpolation
		// a value that is at a certain percentage of a distance between two values
		if (lerpAmt < 1.0f)
			lerpAmt += lerpSpeed;

		GetComponent<MeshRenderer> ().material.color = Color.Lerp (Color.red, Color.green, lerpAmt);

		destPos.x = Mathf.Sin (Time.time * 0.5f)*3f;
		transform.position = Vector3.Lerp (startPos, destPos, lerpAmt);
	}
}
