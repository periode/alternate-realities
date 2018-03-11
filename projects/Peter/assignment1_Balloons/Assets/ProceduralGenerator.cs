using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {

	public GameObject myBaloon;
	public int numBalloons;
	GameObject[] all_Baloons;



	// Use this for initialization
	void Start () {

		all_Baloons = new GameObject[numBalloons * numBalloons];


		for (int i = 0; i < numBalloons; i++) {
			for (int j = 0; j < numBalloons; j++) {

				float newy = Mathf.PerlinNoise (i * 0.1f, j * 0.1f) * 20.0f;

				//Random.Range(-10.0f, 10.0f),
				all_Baloons[i*numBalloons +j ] = GameObject.Instantiate(myBaloon, new Vector3( Random.Range(-10.0f, 10.0f), newy,Random.Range(-10.0f, 10.0f)), Quaternion.identity );

			}
		}

	
	}
	
	// Update is called once per frame
	void Update () {


		for (int i = 0; i < numBalloons * numBalloons; i++) {
			if (all_Baloons [i]) {
				
				if (all_Baloons [i].transform.position.y > 80) {

					Destroy (all_Baloons [i]);
					
				}else if (all_Baloons [i].transform.position.y > 50) {

					all_Baloons [i].GetComponent<MeshRenderer> ().material.color = Color.black;
			
				}

			}

		}
		
	}
}
