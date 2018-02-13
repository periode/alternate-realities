using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereFader : MonoBehaviour {

	bool hasPlayed = false;

	float startTime;
	float timer = 0.0f;

	// Use this for initialization
	void Start () {
		startTime = Time.time;
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (DoSomethingAfterSomeSeconds (2.0f));

		GetComponent<AudioSource> ().volume = Mathf.Lerp (0.0f, 1.0f, Time.time);
		Debug.Log (Time.time);
		if(Time.time - startTime > timer){
			if(GetComponent<AudioSource>().isPlaying == false){
				GetComponent<AudioSource> ().Play ();
			}

			startTime = Time.time;
		}

	}

	IEnumerator DoSomethingAfterSomeSeconds(float seconds) {
		for(float i = 0f; i < 1.0f; i += 0.1f){
			this.GetComponent<Renderer> ().material.color = new Color (0f, i, 0f);
		}

		yield return new WaitForSeconds(seconds);
	}
}
