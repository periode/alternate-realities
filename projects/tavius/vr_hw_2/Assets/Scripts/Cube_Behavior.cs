using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube_Behavior : MonoBehaviour {

    // Use this for initialization
    void Start () {
        float myHeight = transform.position.y;
        float myColor = myHeight / 10.0f;
        GetComponent<MeshRenderer>().material.color = new Color(myColor, myColor, myColor);
        //transform.localScale = new Vector3(1 - myColor, 1 - myColor, 1 - myColor);
	}
	
	// Update is called once per frame
	void Update () {

    }
}
