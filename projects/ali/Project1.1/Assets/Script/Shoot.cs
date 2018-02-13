using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shoot : MonoBehaviour {

	// Rocket Prefab
	public GameObject bulletPrefab;

	// Update is called once per frame
	void Update () {
		// left mouse clicked?
		if (Input.GetMouseButtonDown(0)) {
			GameObject g = (GameObject)Instantiate(bulletPrefab,
				transform.position,
				transform.parent.rotation);
			float force = g.GetComponent<BulletBehavior>().speed;
			GetComponent<Rigidbody>().AddForce(g.transform.forward * force);
		}
	}
}