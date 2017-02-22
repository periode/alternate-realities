using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastFirefly : MonoBehaviour {
	public LayerMask layerMask;
	float raycastDistance = 111f;
	public float timer;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		Vector3 fwd = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);
		RaycastHit hit = new RaycastHit ();

		if(Physics.Raycast(transform.position, fwd, out hit, raycastDistance)){
			Debug.Log ("hit object: " + hit.collider.gameObject.name);

			if(hit.collider.gameObject.tag == "Tree"){
				ParticleSystem.EmissionModule em = hit.collider.gameObject.GetComponent<ParticleSystem> ().emission;
				em.enabled = true; 
			}
		}
			

	}
}
