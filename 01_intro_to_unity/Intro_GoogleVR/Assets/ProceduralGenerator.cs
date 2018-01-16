using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {

	public GameObject myPrefab;

	// Use this for initialization
	void Start () {
		GameObject.Instantiate (myPrefab as Object, Vector3.zero, Quaternion.identity);
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
