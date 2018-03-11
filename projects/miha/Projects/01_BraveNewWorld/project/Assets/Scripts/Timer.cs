using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

	public float t;
	public float wait_until;
	// Use this for initialization
	void Start () {
		wait_until = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {

	}


	public bool CheckForUpdate(){

		bool update;
		t += Time.deltaTime;
		if (t > wait_until) {
			t = 0;
			wait_until = RandomizeWaitTime (0.2f, 2.5f);
			update =  true;
		} else {
			update = false;
		}

		return update;
	}

	private float RandomizeWaitTime(float min, float max){
		return Random.Range (min, max);
	}

}
