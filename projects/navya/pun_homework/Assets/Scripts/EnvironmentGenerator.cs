using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentGenerator : MonoBehaviour {


	public GameObject player;
	public GameObject attack;


	// Use this for initialization
	void Start () {

		GameObject.Instantiate (player, new Vector3 (0-attack.GetComponent<Transform>().localScale.x, 0, 0), Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
