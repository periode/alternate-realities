using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackWhiteRenderer : MonoBehaviour {

//	private IEnumerator coroutine;
	bool hasPlayed = false;

	public float delay = 2.0f;
	public int counter;
	public int divider =2;

	// Use this for initialization
	void Start () {

		counter = 0;
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (DoSomethingAfterSomeSeconds (delay));

//		if(GetComponent<AudioSource>().isPlaying == false){
//			GetComponent<AudioSource> ().Play ();
//		}

	}

	IEnumerator DoSomethingAfterSomeSeconds(float seconds) {

		if (counter % divider == 0) {
			this.GetComponent<Renderer> ().material.color = Color.black;
		} else {
			this.GetComponent<Renderer> ().material.color = Color.white;
		}

		counter +=1;
		yield return new WaitForSeconds(seconds);
	}
}
