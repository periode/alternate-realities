using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class leaf_controller : MonoBehaviour {


	public float fallRate = 0.0f;

	// Use this for initialization
	void Start () {
		//At the beginning, no leaves are falling
		var em = GetComponent<ParticleSystem>().emission;
		em.rateOverTime = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

		var em = GetComponent<ParticleSystem>().emission;
		em.rateOverTime = fallRate;

		if (fallRate > 0.0f) {
			fallRate = fallRate - 0.01f;
		}
	}
}
