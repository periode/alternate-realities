using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {

	public int raycastDistance;
	public LayerMask layers;
	public bool noHit = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Raycast ();
		noHit = false;
	}

	void Raycast() {
		Vector3 forward = transform.forward;
		RaycastHit hit;

		if(Physics.Raycast(transform.position, forward, out hit, raycastDistance, layers)) {
			float size = hit.collider.gameObject.GetComponent<Transform> ().localScale.y;
		//	string tag = hit.collider.gameObject.tag;
			//if (Input.GetMouseButton (0)) {
			if (size > 0.1) {
				
				print (size);
				hit.collider.gameObject.transform.localScale -= new Vector3 (0, 0.02f, 0);
				hit.collider.gameObject.GetComponent<AudioSource> ().volume = 0.3f;
			}
			//hit.collider.gameObject.GetComponent<AudioSource> ().volume = 0.8f;
			if (size <= 0.2) {
				hit.collider.gameObject.GetComponent<AudioSource> ().volume = 0;
				//hit.collider.gameObject.GetComponent<Renderer> ().enabled = false;
			}
		} 
	}
}
