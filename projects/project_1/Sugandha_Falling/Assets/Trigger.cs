using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour {

	// Use this for initialization
	void Start () {
		transform.localScale = new Vector3 (50,50,50);
	}

	// Update is called once per frame
	void Update () {

		if(Cardboard.SDK.Triggered){
			GameObject[] alldrops = GameObject.FindGameObjectsWithTag ("drops");

			for (int i = 0; i < alldrops.Length; i++) {
				alldrops[i].GetComponent<shrinker>().shrinkActive = true;

			}
		}
	}
}
