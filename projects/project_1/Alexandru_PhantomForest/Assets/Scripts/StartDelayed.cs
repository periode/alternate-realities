using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartDelayed : MonoBehaviour {
	public AudioClip delayedClip;
	public AudioClip normalClip;
	float timer;
	float lastTrigger;

	// Use this for initialization
	void Start () {
		lastTrigger = 0;
		AudioSource.PlayClipAtPoint (normalClip, new Vector3 (0, 0, 0));
	}
	
	// Update is called once per frame
	void Update () {

		timer = Time.timeSinceLevelLoad;

		if (timer - lastTrigger > 10.00f) {
			AudioSource.PlayClipAtPoint (delayedClip, new Vector3 (0, 0, 0));
			lastTrigger = timer;
		}
	}
}
