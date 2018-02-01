using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {

	public LayerMask layerMask;
	public float raycastDistance;

	// Use this for initialization
	void Start () {



	}

	// Update is called once per frame
	void Update () {
		Raycasting ();

		if(Input.GetMouseButtonDown(0)){
			Debug.Log ("mouse pressed!");
		}
	}

	void Raycasting(){
		Vector3 fwd = transform.TransformDirection (Vector3.forward); //what is the direction in front of us
		RaycastHit hit = new RaycastHit ();

		if(Physics.Raycast(transform.position, fwd, out hit, raycastDistance, layerMask)){
			Debug.Log ("hit object: " + hit.collider.gameObject.name);

			if(hit.collider.gameObject.name == "Cube"){
				hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.red;
			}
		}
	}
}
