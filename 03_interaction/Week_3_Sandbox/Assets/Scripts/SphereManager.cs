using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void ChangeTag(string new_tag) {
		this.gameObject.tag = new_tag;
		Debug.Log ("changed tag to " + new_tag);
	}
}
