using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelatedBehavior : MonoBehaviour {

	// Use this for initialization
	void Start () {
//		this.gameObject.GetComponent<Transform>().position.x

		var videoPlayer = this.gameObject.AddComponent<UnityEngine.Video.VideoPlayer>();
		videoPlayer.playOnAwake = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}

}
