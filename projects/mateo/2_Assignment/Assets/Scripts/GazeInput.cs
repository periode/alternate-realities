using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GazeInput : MonoBehaviour {

    Camera cam;

	// Use this for initialization
	void Start () {
        cam = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool bHit = Physics.Raycast(ray, out hit);

        if (bHit) {
            GameObject other = hit.transform.gameObject;
            MeshRenderer omr = other.GetComponent<MeshRenderer>();

            if (other.CompareTag("Fadeable")) {
                FadeBehavior fb = other.GetComponent<FadeBehavior>();
                if (fb) {
                    fb.Reset();
                }
            }
            else {
               // omr.material.color = Color.red;
            }
        }
	}
}
