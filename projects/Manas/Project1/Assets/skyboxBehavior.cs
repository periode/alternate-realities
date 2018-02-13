using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class skyboxBehavior : MonoBehaviour {
    public Material[] myMaterials = new Material[6];



	// Use this for initialization
	void Start () {
        RenderSettings.skybox = myMaterials[Random.Range(0, myMaterials.Length)];
		
	}
	
	// Update is called once per frame
	void Update () {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time);
		
	}
}