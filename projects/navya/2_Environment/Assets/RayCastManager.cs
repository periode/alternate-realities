using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastManager : MonoBehaviour {

	public LayerMask layerMask;
	public float raycastDistance;
	public GameObject dirLight;

	int hitCounter;
	Quaternion rotator = Quaternion.identity;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		Raycasting ();

		if (hitCounter > 10) {
			rotator.eulerAngles = new Vector3 (150 + hitCounter/5, 0, 0);
			dirLight.GetComponent<Transform> ().rotation = rotator;
		}
	}

	void Raycasting(){
		Vector3 fwd = transform.TransformDirection (Vector3.forward); //what is the direction in front of us
		RaycastHit hit = new RaycastHit ();

		if(Physics.Raycast(transform.position, fwd, out hit, raycastDistance, layerMask)){
			Debug.Log ("hit object: " + hit.collider.gameObject.name);

			if(hit.collider.gameObject.tag == "cuber"){
				hit.collider.gameObject.GetComponent<Renderer> ().enabled = true;
				hitCounter++;
			}
		}
	}
}