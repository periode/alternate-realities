using UnityEngine;
using System.Collections;

public class RaycastManager : MonoBehaviour {

	public LayerMask layerMask;
	public float raycastDistance;
	public float timer;

	// Use this for initialization
	void Start () {
        

	}
	
	// Update is called once per frame
	void Update () {
		Raycasting ();

		if(Cardboard.SDK.Triggered){
			Debug.Log ("Event heard!");
			GameObject.Find ("Cube").GetComponent<Renderer> ().material.color = Color.white;
		}
	}

	void Raycasting(){
		Vector3 fwd = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);
		RaycastHit hit = new RaycastHit ();

		if(Physics.Raycast(transform.position, fwd, out hit, raycastDistance, layerMask)){
			Debug.Log ("hit object: " + hit.collider.gameObject.name);

			if(hit.collider.gameObject.name == "Cube"){
				hit.collider.gameObject.GetComponent<Renderer> ().material.color = Color.red;

                    if (hit.collider.gameObject.tag == "flower05")
                        GetComponent<AudioSource>().Play();

                }
			}
		}
	}


