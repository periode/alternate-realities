using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestFade : MonoBehaviour
{
    public float alpha;
	// Use this for initialization
	void Start () {
        alpha = 0.2f;
	}
	
	// Update is called once per frame
	void Update () {
        this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
    }
}