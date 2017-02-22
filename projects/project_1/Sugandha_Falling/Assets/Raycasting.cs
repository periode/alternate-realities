using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Raycasting : MonoBehaviour {

	public bool stare = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Raycast ();
	}

	void Raycast(){
		Vector3 directionOfRay = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);

		RaycastHit information = new RaycastHit ();

		if (Physics.Raycast (transform.position, directionOfRay, out information, 200)) {
				Debug.Log ("hit" + information.transform.gameObject.name);

			GameObject go = information.transform.gameObject;
				
			if (go.tag == "drops") {
				Rigidbody stop = go.GetComponent<Rigidbody> ();
				Destroy (stop);
			}
		}
	}
}

