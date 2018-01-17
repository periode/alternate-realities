using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKey(KeyCode.UpArrow)){
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + 0.01f);
		}

		if(Input.GetKey(KeyCode.DownArrow)){
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - 0.01f);
		}

		if(Input.GetKey(KeyCode.LeftArrow)){
			transform.position = new Vector3 (transform.position.x + 0.01f, transform.position.y, transform.position.z);
		}

		if(Input.GetKey(KeyCode.RightArrow)){
			transform.position = new Vector3 (transform.position.x - 0.01f, transform.position.y, transform.position.z);
		}
	}
}
