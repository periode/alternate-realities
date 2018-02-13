using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BalloonBehavior : MonoBehaviour {

	public Vector3 newPos;

	// Use this for initialization
	void Start () {
//		newPos = new Vector3(1, 0, 1);
		this.gameObject.GetComponent<MeshRenderer>().material.color =  Random.ColorHSV(0f, 1f, 1f, 1f, 0.5f, 1f);
			
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 temp;
		temp = transform.position;
		temp.y += 0.1f;
		transform.position =  temp;


	}
}
