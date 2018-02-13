using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastManager : MonoBehaviour {
	public float speed;
	public float raycastDistance;
	float raycastNirvanaDistance;
	public float maxHeight;
	bool setFirst = false;
	bool gameNotOver = true;
	int startedSeeing;

//	public float timer;
	Color[] colors = new Color[5] {Color.red, Color.blue,  Color.black, Color.green, Color.white};
	int colorCnt;
	LayerMask nirvanaLayerMask;
	GameObject currStep;
	int currStepValue;
	// Use this for initialization
	void Start () {
		startedSeeing = 0;
		nirvanaLayerMask = LayerMask.GetMask ("Nirvana");
		raycastNirvanaDistance = 100.0f;
		colorCnt = 0;
		speed = 2.0f;
		maxHeight = 38.5f;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (gameNotOver) {
			
			RaycastingSteps ();
		}
			RaycastingNirvana ();


		

	}

	void RaycastingNirvana(){

		Vector3 fwd = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);
		RaycastHit hit = new RaycastHit ();
		//we hit nirvana
		if( Physics.Raycast(transform.position, fwd, out hit, raycastNirvanaDistance, nirvanaLayerMask) ){

			if (startedSeeing != 0) {
			
				//compare if the time we're seeing this 
				if((int)Time.frameCount - startedSeeing > 50){
					//end game

					transform.position = Vector3.Lerp(transform.position, new Vector3 (10, 300, 10), Time.deltaTime * 3.0f);
					gameNotOver = false;

					Debug.Log ("end of Game!");
				}
			
			} else {
			
				startedSeeing = (int)Time.frameCount;

			}

		
		}	
			
	}

	void RaycastingSteps(){
		Vector3 fwd = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);
		RaycastHit hit = new RaycastHit ();


		//we hit something
		if( Physics.Raycast(transform.position, fwd, out hit, raycastDistance  )  ){

			//let's see which step we're hitting
			int hittingStepValue = ( (hit.collider.gameObject.GetComponent<stepBehavior>().stairWay *20 ) + (hit.collider.gameObject.GetComponent<stepBehavior>().stepNumber)) % 80;



			//initialization msut happne here
			if (!setFirst) {
				currStepValue = hittingStepValue - 1;
				setFirst = true;
			}

			//we hit a step that we're not on
			if (hit.collider.gameObject != currStep  && (hittingStepValue == (currStepValue + 1) % 80 )) {

				currStep = hit.collider.gameObject;

				if (hittingStepValue == (currStepValue + 1) % 80 ){	
					//convert the block color to the next one
					if (currStep.GetComponent<Renderer> ().material.GetColor("_Color") == (Color)colors[colorCnt]) {
						colorCnt= (colorCnt+ 1) % 5;
					}

					currStep.GetComponent<Renderer> ().material.color = (Color)colors[colorCnt];


					//change the height of the block on the opposite side
					int stairWayNum = currStep.GetComponent<stepBehavior> ().stairWay;
					int stepNum = currStep.GetComponent<stepBehavior> ().stepNumber;


					var staircase = GameObject.FindWithTag ("Player");
					var stairToChange = staircase.GetComponent<StairCaseBehavior> ().stairWays [(stairWayNum + 2) % 4].GetComponent<stairWayBehavior> ().stairSteps [stepNum];
					stairToChange.transform.position = new Vector3 (stairToChange.transform.position.x, maxHeight, stairToChange.transform.position.z);
					maxHeight += 0.5f;
					currStepValue = (currStepValue+ 1) % 80;

				}
			}

			//move player
			transform.position = Vector3.Lerp(transform.position, new Vector3 (currStep.transform.position.x, currStep.transform.position.y + 2.0f, currStep.transform.position.z), Time.deltaTime * speed);


		}	
	}
}

