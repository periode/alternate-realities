using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class stairWayBehavior : MonoBehaviour {
	
	public int numSteps;

	//determine the x and z coordinates of this particular stairway's platform.
	/*
	     x,z
	0:  (0,0) -> (0,1)  (increment z)
	1:  (0,1) -> (1,1)  (increment x)
	2:  (1,1) -> (1,0)  (decrement z)
	3:  (1,0) -> (0,0)  (decrement x) 

	*/

	public int stairWayNumber;
//	public int stepSize;
	public GameObject stairStep;
	//we will determine later if we need this separately.
	public GameObject[] stairSteps;


	//determine a function to calculate the value of the stepHeight
	public int stepHeight;
	int[,] nextStep = new int[,] { { 0, 1 }, { 1, 0 }, { 0, -1 }, { -1, 0 } };
	int[] startingXZ;

	// Use this for initialization
	void Start () {

		
		stairSteps = new GameObject[numSteps+1];
		startingXZ= new int[] {stairWayNumber>1?numSteps:0, stairWayNumber==1 || stairWayNumber==2 ?numSteps:0 };
		Vector3 temp;
		//generate steps for this stairway
		for (int i = 0; i < numSteps; i++) {
			temp = new Vector3 (startingXZ [0] + nextStep [stairWayNumber, 0] * i, stepHeightCalc (i, true, (float)stairWayNumber * (float)numSteps / 2, 0.5f), startingXZ [1] + nextStep [stairWayNumber, 1] * i);

			stairSteps [i] = GameObject.Instantiate (stairStep, temp, Quaternion.identity);
			stairSteps [i].gameObject.GetComponent<stepBehavior> ().stepNumber = i;
			stairSteps [i].gameObject.GetComponent<stepBehavior> ().stairWay = stairWayNumber;
			stairSteps [i].gameObject.GetComponent<stepBehavior> ().stepPosition = temp;


		}


	}
	
	// Update is called once per frame
	void Update () {





	}

	float stepHeightCalc(int stepNumber, bool stairGoesUp, float stepBase, float stepIncrement){

		float value = (float)stepNumber * stepIncrement;
		return stairGoesUp ? stepBase + value : stepBase - value;

	}


}
