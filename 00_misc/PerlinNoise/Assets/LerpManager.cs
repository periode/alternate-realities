using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpManager : MonoBehaviour {

	public Vector3 start;
	public Vector3 end;
	public float lerpInc;
	float lerpVal = 0;



	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Vector3.Lerp (start, end, (Mathf.Sin(Time.time*lerpInc)+1)/2);

		if (lerpVal < 1)
			lerpVal += lerpInc;


		GetComponent<Renderer> ().material.color = Color.Lerp (Color.white, Color.red, lerpVal);
	}
}
