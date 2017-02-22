using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fire_controler : MonoBehaviour {

	public float burnRate = 100.0f;

	// Use this for initialization
	void Start () {
//		ParticleSystem ps = GetComponent<ParticleSystem> ();
//		GameObject myTorch = GameObject.Find ("MyTorch");
		var em = GetComponent<ParticleSystem>().emission;
		em.rateOverTime = burnRate;

		// Change brightness of the Light
		GameObject myLight = GameObject.Find ("MyLight");
		myLight.GetComponent<Light>().intensity = scale(0.0f,100.0f,0.0f,8.0f,burnRate);


	}
	
	// Update is called once per frame
	void Update () {
//		GameObject myTorch = GameObject.Find ("MyTorch");
		var em = GetComponent<ParticleSystem>().emission;
//		print (em);

		em.rateOverTime = burnRate;
//		print ("burnRate");
		// Only make burnrate slower if it is still burning
		if (burnRate > 0) {
			burnRate -= 0.05f;
		}


		// Change brightness of the Light
		GameObject myLight = GameObject.Find ("MyLight");
		myLight.GetComponent<Light>().intensity = scale(0.0f,100.0f,0.0f,8.0f,burnRate);



	}


	// Scale function from https://forum.unity3d.com/threads/mapping-or-scaling-values-to-a-new-range.180090/
	float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue){

		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;

		return(NewValue);
	}
}

//Scale function
