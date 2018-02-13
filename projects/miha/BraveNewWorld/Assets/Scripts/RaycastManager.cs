
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {

	public LayerMask layerMask;
	public float raycastDistance;
	private int old_hit_id;
	private int new_hit_id;

	bool current_track_hit = false;
	bool prev_track_hit = false;

	// Use this for initialization
	void Start () {



	}

	// Update is called once per frame
	void Update () {
		Raycasting ();

//		StartCoroutine (CameraMoveTowardsTrack());

//			if(Input.GetMouseButtonDown(0)){
//			Debug.Log ("mouse pressed!");
//		}
	}

	void Raycasting(){
		Vector3 fwd = transform.TransformDirection (Vector3.forward); //what is the direction in front of us
		RaycastHit hit = new RaycastHit ();

		if(Physics.Raycast(transform.position, fwd, out hit, raycastDistance, layerMask)){
//			Debug.Log ("hit object: " + hit.collider.gameObject.name);


			if (hit.collider.gameObject.name == "Particle(Clone)") {
				Particle p = hit.collider.gameObject.GetComponent<Particle> ();
				if (p.seek_interaction == true && p.hover == true) {
					p.AttentionCountUpdate (Time.time,Time.deltaTime);

				}
//				
			}

		}
	}

}