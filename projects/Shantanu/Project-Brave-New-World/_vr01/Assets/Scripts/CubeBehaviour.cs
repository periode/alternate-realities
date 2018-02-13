using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehaviour : MonoBehaviour {


    public float xpos = 0.0f;
    public float xspeed = 0.3f;
	// Use this for initialization
	void Start () {
        GetComponent<MeshRenderer>().material.color = Color.red;
        //GameObject.Find("MuhSphere").GetComponent<MeshRenderer>().material.color = Color.blue;
	}
	
	// Update is called once per frame
	void Update () {
        xpos = transform.position.x;
        xpos += xspeed;

        GetComponent<MeshRenderer>().material.color = Color.HSVToRGB(Mathf.PerlinNoise(xpos, xpos), 0.8f,0.5f);
        Debug.Log(Mathf.PerlinNoise(xpos, xpos));

    }
}
