using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Cardboard.SDK.Triggered){
			GameObject myCube = GameObject.Find ("MyCube");
			myCube.GetComponent<CubeBehaviour> ().userHasTriggered = true;
		}

		Raycast ();
	}

	void Raycast(){
		Vector3 directionOfRay = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);

		RaycastHit informationAboutHit = new RaycastHit ();

		if(Physics.Raycast(transform.position, directionOfRay, out informationAboutHit, 20)){
			Debug.Log (informationAboutHit.transform.gameObject.name);

			if(informationAboutHit.transform.gameObject.name == "MyCube"){
				informationAboutHit.transform.gameObject.GetComponent<Renderer> ().material.color = Color.red;

                
                }
		}
	}
}
