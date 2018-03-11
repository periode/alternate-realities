using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningController : MonoBehaviour {

	public GameObject Lightning; 
	private ParticleSystem PSystem; 
	private ParticleCollisionEvent[] CollisionEvents;
	public GameObject Orb;
	private Vector3 center;
	public float power = 100f;



	public float radius = 5f;
	float next_generate_time;

	public void OnParticleCollision (GameObject Orb)
	{
		CollisionEvents = new ParticleCollisionEvent[8];

		int collCount = PSystem.GetSafeCollisionEventSize();

		if (collCount > CollisionEvents.Length)
			CollisionEvents = new ParticleCollisionEvent[collCount];

		int eventCount = PSystem.GetCollisionEvents(Orb, CollisionEvents);

		for (int i = 0; i < eventCount; i++) {
			
		}
			
	}


	void Strike(Vector3 intersection) {
		Vector3 strikePos = intersection; 
		Collider[] colliders = Physics.OverlapSphere (strikePos, radius);
		foreach (Collider hit in colliders) {
			if (hit && hit.GetComponent<Rigidbody> ()) {
				hit.GetComponent<Rigidbody> ().AddExplosionForce (power, strikePos, radius);
				 
			}
		}
	}
}
//	void Start()
//	{
//
//		next_generate_time = Time.time+5.0f;
//	}
//
//	void Update()  
//	{
//		if(Orb 
//		if(Time.time > next_generate_time)
//		{
//			
//			Instantiate (Lightning);
//
//
//			next_generate_time += 5.0f;
//		}
//
//	}
//}

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
