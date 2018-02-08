using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sphere : MonoBehaviour {

	public Vector3 newPos;
	public float speed;

	// Use this for initialization
//	public GameObject projectile;

	void Start () {
		this.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
		speed = 5.0f;
	}
	
	// Update is called once per frame
	void Update () {

		newPos = transform.position;
		if (Input.GetMouseButton (0)) {
//			Debug.Log("Pressed left click.");

			newPos.x += 0.1f;
			transform.position = newPos;
			transform.Rotate (Vector3.right*20.0f);
		}
//			Instantiate(projectile, transform.position, transform.rotation);
	}
}
