using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shrinker : MonoBehaviour {
	public bool shrinkActive = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if ((transform.localScale.x > 0) && (shrinkActive == true)) {
			transform.localScale = new Vector3 (transform.localScale.x - 1, transform.localScale.y - 1, transform.localScale.z - 1);
				
		}
	}
}
