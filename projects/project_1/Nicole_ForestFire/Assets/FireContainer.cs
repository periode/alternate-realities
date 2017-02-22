using UnityEngine;
using System.Collections;

public class FireContainer : MonoBehaviour {

	Vector3 direction;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		transform.position += transform.forward * Time.deltaTime * 10f;
	}
}
