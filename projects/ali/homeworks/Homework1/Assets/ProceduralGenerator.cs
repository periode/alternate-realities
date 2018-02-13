using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {
	public GameObject myPrefab;
	public int numObjects;

	// Use this for initialization
	void Start () {
		Vector3 center = transform.position;
		for (int i = 0; i < numObjects; i++) {
			Vector3 pos = RandomCircle (center, 5.0f);
			Quaternion rot = Quaternion.FromToRotation (Vector3.forward, center - pos);
			Instantiate (myPrefab, pos, rot);
			GameObject.Instantiate (myPrefab as Object, Vector3.zero, Quaternion.identity);
		}
	}
		Vector3 RandomCircle ( Vector3 center ,   float radius  ){
			float ang = Random.value * 360;
			Vector3 pos;
			pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
			pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
			pos.z = center.z;
			return pos;
		}

	// Update is called once per frame
	void Update () {
		
	}
}
