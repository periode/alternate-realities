using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasBehavior : MonoBehaviour {

	float time = 0.0F;
	bool fadeOut = false;
	bool close = false;
	bool is_active = false;
	// Use this for initialization
	void Start () {
		is_active = true;
	}
	
	// Update is called once per frame
	void Update () {
		time += Time.deltaTime;
		if (time > 8.0F && !close) {
//			gameObject.GetComponent<CanvasRenderer>().enabled = false;
			gameObject.SetActive(false);
			close = true;
			Debug.Log ("five seconds have passed!");
//			StartCoroutine (FadeOutTitle ());
//			fadeOut = false;
		}
	}

	public bool IsVisible(){
		return is_active;
	}

	public void MakeInvisible(){
		is_active = false;
		gameObject.SetActive (false);
		Debug.Log ("shut down the object");
	}
//	IEnumerator FadeOutTitle(){
//		float lerpAmt = 0;
//		float lerpSpeed = 0.01F;
//		while(
//		yield null;
//	}
}
