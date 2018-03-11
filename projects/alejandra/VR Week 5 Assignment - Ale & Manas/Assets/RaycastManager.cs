using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {

	public LayerMask layerMask;
	public float raycastDistance;

	public float smooth = 0.1f;

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
			if(hit.collider.gameObject.name == "Book(Clone)" ){
				Debug.Log("Hit a book. Ouch!");



				//hit.collider.gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, smooth);
				//Destroy (hit.collider.gameObject, 0.5f);

				//hit.collider.gameObject.GetComponent<Renderer> ().enabled = false;

			}
		}
	}
}