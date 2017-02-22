using UnityEngine;
using System.Collections;

public class Fire : MonoBehaviour {

	// Use this for initialization
	void Start () {
		ParticleSystem fire = GetComponent<ParticleSystem> ();
		var emission = fire.emission;
		emission.enabled = true;
	}
	
	// Update is called once per frame
	void Update () {
	}
}
