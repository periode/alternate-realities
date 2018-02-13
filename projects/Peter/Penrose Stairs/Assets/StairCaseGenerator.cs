using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StairCaseGenerator : MonoBehaviour {

	public GameObject myStairCase;
	public GameObject myNirvana;

	// Use this for initialization
	void Start () {
		
		GameObject.Instantiate (myStairCase, Vector3.zero, Quaternion.identity);
		GameObject.Instantiate (myNirvana, new Vector3 (10, 100, 10), Quaternion.identity);

	}
	
	// Update is called once per frame
	void Update () {
		
//		myStairCase.update ();

	}
}
