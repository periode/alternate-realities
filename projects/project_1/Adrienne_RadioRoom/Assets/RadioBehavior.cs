using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioBehavior : MonoBehaviour {

	public bool playingState;
	private bool goingUp = false;
	GameObject wallLeft;
	GameObject wallRight;
	private Color myColorLeft;
	private Color myColorRight;
	float timeElapsed;
	float timer = 0.5f;
	// Use this for initialization
	void Start () {
		playingState = false;
		wallLeft = GameObject.Find ("Wall 1");
		wallRight = GameObject.Find ("Wall 2");
		myColorLeft = wallLeft.GetComponent<Renderer> ().material.color;
		myColorRight = wallRight.GetComponent<Renderer> ().material.color;
		timeElapsed = Time.time;
	}

	// Update is called once per frame
	void Update () {
		
		if (Cardboard.SDK.Triggered) {
			playingState = !playingState;
			if (!GetComponent<AudioSource> ().isPlaying && playingState == true) {
				GetComponent<AudioSource> ().Play ();
				goingUp = true;
				transform.position = (new Vector3 (transform.position.x, transform.position.y +0.1f, transform.position.z));
				wallLeft.GetComponent<Renderer> ().material.color = new Color (Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
				wallRight.GetComponent<Renderer> ().material.color = new Color (Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
				timeElapsed=Time.time;

			}
			
			if (GetComponent<AudioSource> ().isPlaying && playingState == false) {
				GetComponent<AudioSource> ().Stop ();
				transform.position = new Vector3 (-4f,1.5f,-3.5f);
				wallLeft.GetComponent<Renderer> ().material.color = myColorLeft;
				wallRight.GetComponent<Renderer> ().material.color = myColorRight;
			}

		}

		if (GetComponent<AudioSource> ().isPlaying) {
			if (goingUp == true)
				transform.position = (new Vector3 (transform.position.x, transform.position.y + 0.05f, transform.position.z));
			if (transform.position.y >= 3) {
				goingUp = false;
			} 
			if (goingUp == false) {
				transform.position = (new Vector3 (transform.position.x, transform.position.y - 0.05f, transform.position.z));
			}
			if (transform.position.y <=2) {
				goingUp = true;
			} 
			if ((Time.time-timeElapsed) > timer) {
				wallLeft.GetComponent<Renderer> ().material.color = new Color (Random.Range (0f, 1f), Random.Range (0f, 1f), Random.Range (0f, 1f));
				wallRight.GetComponent<Renderer> ().material.color = new Color (Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
				timeElapsed = Time.time;
			} 
		}
			//transform.Rotate (0, 0, 10);
	}

	public bool getPlayingState(){
		return playingState;
	}

	void Raycast(){
				Vector3 directionOfRay = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);

				RaycastHit informationAboutHit = new RaycastHit ();

				if(Physics.Raycast(transform.position, directionOfRay, out informationAboutHit, 20)){
					Debug.Log (informationAboutHit.transform.gameObject.name);

					if(informationAboutHit.transform.gameObject.name == "Radio"){
						if (playingState == false) {
							
							playingState = true;

						}
					}
				}
			}
}
