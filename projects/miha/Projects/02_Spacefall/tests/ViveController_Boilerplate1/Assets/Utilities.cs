using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public static float FindMidpoint(float _xLeft, float _xRight){
		//first find the absolute
		float dist = Mathf.Abs(_xLeft - _xRight);
		float midPoint = _xLeft - dist / 2;
		return midPoint;
	}
}
