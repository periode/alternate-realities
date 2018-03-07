using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour {

	public GameObject Lightning; 
	public Vector3 center;

	float next_generate_time;

	void Start()
	{
		next_generate_time = Time.time+5.0f;
	}

	void Update()  
	{
		if(Time.time > next_generate_time)
		{
			
			Instantiate (Lightning);


			next_generate_time += 5.0f;
		}

	}
}

	//public void GenerateLightning()
//	{
		//Vector3 pos = center + new Vector3(Random.Range());
			//Instantiate(Lightning, pos, Quaternion.identity); 
//	}
//			}

//		lightningPlacement ();
//	}
//
//	void lightningPlacement()
//	{
//		for (int i = 0; i < numberofLightning; i++) {
//			Instantiate (Lightning, GeneratedPosition (), Quaternion.identity);
//		}
//
//	}
//	Vector3 GeneratedPosition() 
//	{
//		int x, y, z;
//		x = UnityEngine.Random.Range (min, max);
//		y = UnityEngine.Random.Range (min, max);
//		z = UnityEngine.Random.Range (min, max);
//		return new Vector3 (x,y,z);
//	}
	// Update is called once per frame
//	void Update () {
//		//transform.rotation = Quaternion.Euler(90, 0, 0);
//	}
//}
