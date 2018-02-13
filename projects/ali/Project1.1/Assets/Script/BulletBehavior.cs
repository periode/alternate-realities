using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletBehavior : MonoBehaviour {

	// The fly speed (used by the weapon later)
	public float speed = 2000.0f;

	// explosion prefab (particles)
	public GameObject explosionPrefab;

	// find out when it hit something
	void OnCollisionEnter(Collision c) {
		Instantiate(explosionPrefab, transform.position, Quaternion.identity);

		// destroy the rocket
		Destroy(gameObject);
	}
}
