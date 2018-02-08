using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeseverywhere : MonoBehaviour {

	public GameObject theEye;
	public int numEyes;
	public Transform target;

	int minPosition = -8;
	int maxPosition = 8;
	GameObject[] ojitos;

	// Use this for initialization
	void Start () {
		ojitos = new GameObject[numEyes];

		for (int i = 0; i < numEyes; i++) {
			float cons_x = Random.Range (minPosition, maxPosition);
			float cons_y = Random.Range (minPosition, maxPosition);
			float cons_z = Random.Range (minPosition, maxPosition);
			ojitos[i] = GameObject.Instantiate (theEye, new Vector3 (cons_x, cons_y, cons_z), Quaternion.identity);
//			ojitos[i].transform.position = ;
			ojitos[i].transform.SetParent (this.transform);
		
		}

	}
	
	// Update is called once per frame
	void Update () {
	}
}
