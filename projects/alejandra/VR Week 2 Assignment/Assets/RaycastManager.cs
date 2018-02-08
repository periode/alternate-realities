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

	}

	void Raycasting(){
		Vector3 fwd = transform.TransformDirection (Vector3.forward); //what is the direction in front of us
		RaycastHit hit = new RaycastHit ();

		if(Physics.Raycast(transform.position, fwd, out hit, raycastDistance, layerMask)){
			Debug.Log ("hit object: " + hit.collider.gameObject.name);

			if(hit.collider.gameObject.name == "Eyeball(Clone)"){
				Debug.Log("Hit an eye. Ouch!");
				hit.collider.gameObject.GetComponent<Renderer> ().enabled = false;
				//hit.collider.gameObject.transform.rotation = Quaternion.identity;
				//hit.collider.gameObject.GetComponent<MeshRenderer>().material.GetTextureOffset = 
			}
		}
	}
}