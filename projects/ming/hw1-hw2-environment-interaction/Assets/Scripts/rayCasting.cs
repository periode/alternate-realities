using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class rayCasting : MonoBehaviour {
	Camera cam;
	public GameObject[] players;
	// Use this for initialization
	void Start () {
		cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		RaycastHit hit; // raycasthit: a struct that stores the info
		Ray aRay = cam.ScreenPointToRay(Input.mousePosition);//casting from camera through mouse till infinity
		if (Physics.Raycast (aRay, out hit)) {
			if (hit.transform.name.Contains("Cube")) {
				players = GameObject.FindGameObjectsWithTag("Player");
				for (int i = 0; i < players.Length; i++) {
                    cubeColor c = players[i].GetComponent<cubeColor>();

                    if (c) {
                        c.rayCasted = true;

                        c.SetColor(new Color(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
                    }
				}
			}
		}
        else{
            players = GameObject.FindGameObjectsWithTag("Player");
            for (int i = 0; i < players.Length; i++)
            {
                cubeColor c = players[i].GetComponent<cubeColor>();
                if (c)
                {
                    c.rayCasted = false;
                }
            }
        }
	}
}
