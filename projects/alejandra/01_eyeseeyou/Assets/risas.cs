using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class risas : MonoBehaviour {

	public AudioSource myAudio;

	public float minimum = 0.1f;
	public float maximum = 1.0f;
	public float volumeSpeed = 0.1f;

	// Use this for initialization
	void Start () {
		myAudio = GetComponent<AudioSource> ();
	}
	
	// Update is called once per frame
	void Update () {
		myAudio.volume = Mathf.Lerp(minimum, maximum, Time.time * volumeSpeed);

	}
}
