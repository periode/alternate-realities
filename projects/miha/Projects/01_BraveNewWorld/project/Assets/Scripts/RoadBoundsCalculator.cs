using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBoundsCalculator : MonoBehaviour {
	public Bounds my_bounds;
	// Use this for initialization
	void Start () {
		my_bounds = GetComponent<MeshRenderer>().bounds;
		Debug.Log (my_bounds.center.x);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void LayTheRoad(){
		
	}
}
