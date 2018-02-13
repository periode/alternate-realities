using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour {
	public Vector3 newPos;

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.white;
	}
	
	// Update is called once per frame
	void Update () {
//		Vector3 temp;
//		temp = transform.position;
//		temp.x += 0.1f;
//		transform.position = temp;
	}
}
