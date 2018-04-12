using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {

	public AudioMixer mixer;
	public AudioMixerSnapshot snapshot_base;
	public AudioMixerSnapshot snapshot_high;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown(KeyCode.P)){
			snapshot_high.TransitionTo (1.0f);
		}

		if(Input.GetKeyDown(KeyCode.I)){
			snapshot_base.TransitionTo (3.0f);
		}
		
	}
}
