using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldControl : MonoBehaviour {

    bool worldStarted;
    public GameObject wholeWorld;
    public GameObject exposition;
    GameObject player;

	// Use this for initialization
	void Start () {
        worldStarted = false;
        player = GameObject.Find("player");
        exposition = GameObject.Find("exposition");
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetMouseButtonDown(0) && !worldStarted) {
            worldStarted = true;
            Destroy(exposition);
            Instantiate(wholeWorld, Vector3.zero, Quaternion.identity);
        }
	}
}
