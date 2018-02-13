using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {

	public LayerMask layerMask;
	public float raycastDistance;

	public float smooth = 0.1f;

	risas audio_laugh;
	// Use this for initialization
	void Start () {
		audio_laugh = GameObject.Find ("Audio").GetComponent<risas>();
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

			if(hit.collider.gameObject.name == "BlueEye(Clone)" || hit.collider.gameObject.name == "BrownEye(Clone)" || hit.collider.gameObject.name == "GreenEye(Clone)" || hit.collider.gameObject.name == "HazelEye(Clone)" ){
				Debug.Log("Hit an eye. Ouch!");
				hit.collider.gameObject.transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.identity, smooth);
				Destroy (hit.collider.gameObject, 0.5f);

				audio_laugh.maximum -= 0.001f;
				//hit.collider.gameObject.GetComponent<Renderer> ().enabled = false;

			}
		}
	}
}