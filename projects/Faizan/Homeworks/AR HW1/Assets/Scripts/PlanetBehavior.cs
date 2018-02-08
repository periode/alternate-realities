using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetBehavior : MonoBehaviour {

	public Vector3 circularMotion;
	public float angle;
	public float radius;

	// Use this for initialization
	void Start () {
//		this.gameObject.GetComponent<MeshRenderer>().material.color = new Color( 1.0f, 1.0f, 0.0f);
		radius = 3.0f;

	}
	
	// Update is called once per frame
	void Update () {
//		newPos = new Vector3 (radius+ count, radius+ count, Random.Range(3.0f,6.0f));;
//		GameObject.Instantiate (myPrefab as Object, newPos, Quaternion.identity);

		transform.Rotate (Vector3.down * 2.0f);

		circularMotion = transform.position;
		circularMotion.x = (radius * Mathf.Sin (angle)); 
		circularMotion.y = (radius * Mathf.Cos (angle));

		transform.position=circularMotion;
		angle+=0.1f;
//		radius += 0.05f;
//		newPos = new Vector3 (radius+ count, radius+ count, Random.Range(3.0f,6.0f));;
//		GameObject.Instantiate (myPrefab as Object, newPos, Quaternion.identity);
//		count += 2.0f;
//
	}
}
