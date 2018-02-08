using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
