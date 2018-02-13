using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadBlockTriggerManager: MonoBehaviour {
	public float collision_time = 0.0f;
	PlayerInteractionManager player;

	void Setup(){
//		player = GameObject.Find("Player").gameObject.GetComponent<PlayerInteractionManager> ();
	}

	void OnTriggerEnter(Collider collider){
//		collision_time = 0;
//		Debug.Log ("ooops, new visitor, star interaction!");
		if (collider.gameObject.name == "Player") {
			Debug.Log ("player just entered!");
			collider.gameObject.GetComponentInChildren<RaycastManager> ().enabled = true;
			collider.gameObject.GetComponent<PlayerInteractionManager>().StageConnectionWithParticle (true);
//
		}
	}

//	void OnCollisionStay(){
//		collision_time += Time.deltaTime;
//		Debug.Log ("collision time is" +  collision_time);
//	}
//
	void OnTriggerExit(Collider collider){
		if (collider.gameObject.name == "Player") {
			Debug.Log ("player just exited!");
//			collider.gameObject.GetComponentInChildren<RaycastManager> ().enabled = false;
//			collider.gameObject.GetComponent<PlayerInteractionManager>().StageConnectionWithParticle (false);
		}
	}
}
