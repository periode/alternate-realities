using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour {
	public Vector3 circularMotion;
	public float angle;
	public float radius;

	// Use this for initialization
	void Start () {
//		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.black;
		radius = 3.0f;

	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (Vector3.down * 2.0f);

//		circularMotion = transform.position;
//		circularMotion.x = (radius * Mathf.Sin (angle)); 
//		circularMotion.y = (radius * Mathf.Cos (angle));
//
//		transform.position=circularMotion;
		circularMotion = new Vector3 ((radius * Mathf.Sin (angle)), (radius * Mathf.Cos (angle)), 0);
		transform.position=circularMotion;
		angle+=0.1f;
	}
}
