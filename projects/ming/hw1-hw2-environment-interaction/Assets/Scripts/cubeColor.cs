using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cubeColor : MonoBehaviour {

    public Color baseColor;
    public bool rayCasted = false;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if(!rayCasted)
            GetComponent<MeshRenderer>().material.color = baseColor;
	}

    public void SetColor(Color c) {
        GetComponent<MeshRenderer>().material.color = c;
    }
}
