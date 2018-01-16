using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereBehaviour : MonoBehaviour {

	//here we declare a variable of type float (a decimal number)
	//we set it to public so that we can see it in the main Unity window
	//and we name it mySpeed
	public float mySpeed;

	//here we declare a boolean variable named isSphereViolet and we set its value to false
	bool isSphereViolet = false;

	// Use this for initialization
	void Start () {
		//we print to the console the current position of the sphere
		Debug.Log (transform.position);

		//we set that position to 0, 0, 0
		transform.position = new Vector3(0, 0, 0);
	}

	// Update is called once per frame
	void Update () {

		//this line accesses the position of the sphere (on the left)
		//on the right, we set it to a new value. the Y and Z components are the same,
		//but we increase slightly the X component, by adding mySpeed to it.
		GetComponent<Transform>().position = new Vector3 (transform.position.x + mySpeed, transform.position.y, transform.position.z);

		//we check if the user has pulled the Cardboard trigger
		if(Cardboard.SDK.Triggered){

			//if the user has pulled the Cardboard trigger, we check the value of isSphereViolet
			if(isSphereViolet == false){
				//if the value of isSphereViolet is indeed 'false', we execute the following code
				//if the value is not 'false', we jump ahead to line 47

				//we access the render component, then the material component, then the color component
				//and we set it to a new color (purple)
				GetComponent<MeshRenderer> ().material.color = new Color (0.5f, 0f, 0.5f);

				//because we've changed that value, we set the value of isSphereViolet to 'true'
				isSphereViolet = true;

			}else if(isSphereViolet == true){
				//otherwise, if the value of isSphereViolet is true...

				//we access the color by getting the renderer, getting the material, getting the color
				//and we set its color to maximum, so that it becomes white
				GetComponent<MeshRenderer> ().material.color = new Color (1f, 1f, 1f);

				//and, because the sphere is now white, we set the value of isSphereViolet to 'true'
				isSphereViolet = false;
			}

			//no matter what, if the user has pulled the trigger, we print the state of the isSphereViolet variable
			Debug.Log ("Is the sphere violet? " + isSphereViolet);
		}

	}
}
