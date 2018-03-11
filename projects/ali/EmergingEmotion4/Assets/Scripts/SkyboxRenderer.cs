using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkyboxRenderer : MonoBehaviour {

	public Material Skybox; 

	// Use this for initialization
	void Start () {
		RenderSettings.skybox = Skybox; 
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
