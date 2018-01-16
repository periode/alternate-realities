using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CylinderStretching : MonoBehaviour {

	public float scaleInc;
	public bool isGrowing = true;

	// Use this for initialization
	void Start () {
		
		transform.localScale = new Vector3 (1, 1, 1);

	}

	// Update is called once per frame
	void Update () {
		if(isGrowing == true){
			transform.localScale = new Vector3 (1, transform.localScale.y + scaleInc, 1);
		}

	}
}
