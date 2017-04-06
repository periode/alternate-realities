using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Blinking : MonoBehaviour {

	public float stepX = 1;
	public float noiseCoeffX = 0.01f;
	float valueX;

	public float stepZ = 1;
	public float noiseCoeffZ = 0.01f;
	float valueZ;

	public float amplification = 2f;

	Vector3 position;

	public GameObject brick;

	// Use this for initialization
	void Start () {
		for(float x = -10; x < 10; x += stepX){
			for(float z = -10; z < 10; z += stepZ){
				
				float y = Mathf.PerlinNoise(x*noiseCoeffX, z*noiseCoeffZ)*amplification;

				position = new Vector3 (x, y, z);

				Quaternion angle = Quaternion.identity;

				angle.eulerAngles = new Vector3 (0, Mathf.PerlinNoise (x * noiseCoeffX, z * noiseCoeffZ) * 360, Mathf.PerlinNoise (x * noiseCoeffX, z * noiseCoeffZ) * 360);


				y = Mathf.PerlinNoise(z*noiseCoeffX+10, x*noiseCoeffZ)*amplification;
				GameObject inst = (GameObject)Instantiate (brick, position, angle);
				inst.transform.localScale *= y;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		

	}

 // :( RIP
}
