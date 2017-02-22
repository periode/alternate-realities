using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class newCube : MonoBehaviour {

	// Use this for initialization
	void Start () {
        print(GetComponent<Transform>().position);

        int myNumber = 5;
        GetComponent<Renderer>().material.color = Color.red;
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
