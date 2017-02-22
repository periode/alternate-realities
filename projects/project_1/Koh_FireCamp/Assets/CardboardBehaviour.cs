using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (Cardboard.SDK.Triggered) {
			//
			Vector3 directionOfRay = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);
			RaycastHit informationAboutHit = new RaycastHit ();

//			print ("TRIGGER DETECTED");
			if (Physics.Raycast (transform.position, directionOfRay, out informationAboutHit, 20)) {
				if (informationAboutHit.transform.gameObject.name == "MyTree") {
					GameObject leaves = GameObject.Find ("Leaves");
					leaves.GetComponent<leaf_controller> ().fallRate = leaves.GetComponent<leaf_controller> ().fallRate + 1.0f;
				}
			}
		}
		 
		Raycast ();
	}

	void Raycast(){
		Vector3 directionOfRay = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);

		RaycastHit informationAboutHit = new RaycastHit ();

		if (Physics.Raycast (transform.position, directionOfRay, out informationAboutHit, 20)) {
//			Debug.Log (informationAboutHit.transform.gameObject.name);

//			print (informationAboutHit.transform.gameObject.name);

			if (informationAboutHit.transform.gameObject.name == "Bonfire") {
				GameObject myLight = GameObject.Find ("MyTorch");
				myLight.GetComponent<fire_controler> ().burnRate = myLight.GetComponent<fire_controler> ().burnRate + 0.3f;


//				print (myLight.GetComponent<fire_controler> ().burnRate);
			}
		}
	}
}
