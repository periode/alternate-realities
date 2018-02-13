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

		if(Physics.Raycast(transform.position, forward, out hit, raycastDistance, layers)){
		//	string tag = hit.collider.gameObject.tag;
			if (Input.GetMouseButtonDown (0)) {
				hit.collider.gameObject.GetComponent<Renderer> ().enabled = false;
				hit.collider.gameObject.GetComponent<AudioSource> ().volume = 0;
			}
			//noHit = true;
		//	if(tag == "Interactables"){
				
				//hit.collider.gameObject.GetComponent<SphereManager> ().ChangeTag ("Non-interactable");
			//}
		} 
	}
}
