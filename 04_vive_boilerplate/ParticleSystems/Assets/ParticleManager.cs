using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManager : MonoBehaviour {

	GameObject fish;

	ParticleSystem ps;

	// Use this for initialization
	void Start () {
		ps = GetComponent<ParticleSystem> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			ps.Play ();
		}

		transform.localRotation = Quaternion.Euler(0, 90 + (Mathf.Sin (Time.time * 5f) * 10f), 0); 

		transform.position = new Vector3 (0, Mathf.Cos (Time.time * 2f), 0);
	}
}
