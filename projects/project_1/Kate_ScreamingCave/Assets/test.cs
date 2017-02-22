using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class test : MonoBehaviour {
	bool hasPlayedOnce;

	// Use this for initialization
	void Start () {

		hasPlayedOnce = false;

	}

	// Update is called once per frame
	void Update () {
		Raycast ();

	}
		
	void Raycast(){
		Vector3 directionOfRay = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);

		RaycastHit informationAboutHit = new RaycastHit ();

		if(Physics.Raycast(transform.position, directionOfRay, out informationAboutHit, 100)){
			Debug.Log (informationAboutHit.transform.gameObject.name);
			GameObject hit = informationAboutHit.transform.gameObject;

			//if the raycast hit game object named sphere
			if (hit.name == "Sphere") {
				AudioSource audio = hit.GetComponent <AudioSource> ();


			// if the audio has played once
				if (hasPlayedOnce == true) {
					//do nothing
				} else {
					hasPlayedOnce = true;
					audio.Play ();

				}
			

			}
		}
	}
}