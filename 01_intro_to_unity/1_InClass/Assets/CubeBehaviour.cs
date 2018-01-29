using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour {

	float xpos = 0;
	public float xspeed;

	// Use this for initialization
	void Start () {
		float myheight = transform.position.y;
		float mycolor = myheight / 10.0f; //now we have a value from 0.1

		GetComponent<MeshRenderer> ().material.color = new Color (mycolor, mycolor, mycolor);

		transform.localScale = new Vector3 (1-mycolor, 1-mycolor, 1-mycolor);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
