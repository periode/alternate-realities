using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCaseBehavior : MonoBehaviour {

	public GameObject myStairWay;
	public int numSteps;
	public GameObject[] stairWays;

	// Use this for initialization
	void Start () {
		
		stairWays = new GameObject[4];

		for (int i = 0; i < 4; i++) {
			
			stairWays[i] = GameObject.Instantiate (
				myStairWay, 
				Vector3.zero, 
				Quaternion.identity	
			);
			
			stairWays[i].GetComponent<stairWayBehavior> ().numSteps = numSteps;
			stairWays[i].GetComponent<stairWayBehavior> ().stairWayNumber = i;


		}

	}
	
	// Update is called once per frame
	void Update () {


		
	}
}
