using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class cubescript : MonoBehaviour {

    public float xpos = 0;
    float x = 0.3f;

    public Color lblue = new Color(0.2f, 0.3f, 0.5f);
    public Color mblue = new Color(0.2f, 0.3f, 0.7f);
    public Color hblue = new Color(0.2f, 0.3f, 1.0f);

    // Use this for initialization
    void Start () {

        this.GetComponent<MeshRenderer>().material.color = Color.red;

        GameObject.Find("sphere").GetComponent<MeshRenderer>().material.color = Color.blue;
       
       
    }
	
	// Update is called once per frame
	void Update () {
       

        //GetComponent<Transform>().position = new Vector3(xpos, 0, 0);
        //xpos++;

    }
}
