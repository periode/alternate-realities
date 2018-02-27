using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Librarian : MonoBehaviour {

	public GameObject shelf;
	public float roomSize = 13.0f;

	// Use this for initialization
	void Start () {
		roomSize = 13.0f;
		for (float i = 0; i < 6; i += 1) {
			if (i != 0 && i != 3) {
				for (int j = 0; j < 5; j++) {
					GameObject s = GameObject.Instantiate (shelf, new Vector3 (Mathf.Cos (i * Mathf.PI / 3f) * roomSize * 0.75f, 0, Mathf.Sin (i * Mathf.PI / 3f) * roomSize), Quaternion.identity);
				//	s.transform.position = new Vector3 (s.transform.localPosition.x + j, s.transform.localPosition.y, s.transform.localPosition.z);
					s.GetComponent<PanelManager> ().hexagon = i;
					s.GetComponent<PanelManager> ().rowNumber = j;
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
