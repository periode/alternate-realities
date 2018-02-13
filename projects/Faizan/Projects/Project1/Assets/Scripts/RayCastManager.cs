using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastManager : MonoBehaviour {

	public int raycastDistance;
	public LayerMask layers;
	public GameObject temp;
	AudioSource adio;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Raycast ();
	}

	void Raycast(){
	
		Vector3 forward = transform.forward;
		RaycastHit hit = new RaycastHit();

		if (Physics.Raycast (transform.position, forward, out hit, raycastDistance, layers)) {
			Debug.Log ("hit object: " + hit.collider.gameObject.name);
			temp = hit.collider.gameObject;
			adio = temp.GetComponent<AudioSource> ();
			if (hit.collider.gameObject.name == "BlackWhiteParent 1 1(Clone)") {
				playAudio ();
				hit.collider.gameObject.GetComponent<BoxCollider> ().enabled = false;
			} else if (hit.collider.gameObject.name == "ErrorScreen 1 1(Clone)") {
				playAudio ();
				hit.collider.gameObject.GetComponent<BoxCollider> ().enabled = false;
			} else if (hit.collider.gameObject.name == "matrixCodeParent 1 1(Clone)") {
				playAudio();
				hit.collider.gameObject.GetComponent<BoxCollider> ().enabled = false;
			} else if (hit.collider.gameObject.name == "PixelatedParent 1 1(Clone)") {
				playAudio();
				hit.collider.gameObject.GetComponent<BoxCollider> ().enabled = false;
			} else {
				playAudio();
				hit.collider.gameObject.GetComponent<BoxCollider> ().enabled = false;
			}
			
		}
	}

	IEnumerator DoSomethingAfterSomeSeconds(float seconds) {

		yield return new WaitForSecondsRealtime(seconds);
		temp.GetComponent<Renderer> ().enabled = false;

	}

	void playAudio(){
		if (temp.GetComponent<AudioSource> ().isPlaying == false) {
			adio.Play ();
			Time.timeScale = 0.0f;
		} else {
			adio.Stop ();
		}
		temp.GetComponent<Renderer> ().enabled = false;
		Time.timeScale = 1.0f;
	}

//	void playVideo(){
//		if ( == false) {
//			adio.Play ();
//			Time.timeScale = 0.0f;
//		} else {
//			adio.Stop ();
//		}
//		temp.GetComponent<Renderer> ().enabled = false;
//		Time.timeScale = 1.0f;
//	
//	}

}


