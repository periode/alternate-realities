using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jump : MonoBehaviour {

	public float speed;
	public float heightNew;

	// Use this for initialization
	void Start () {
		speed = 15f;
		heightNew = 5f;
		Cardboard.Create ();
	}

	// Update is called once per frame
	void Update () {
		Vector3 forward = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);
		RaycastHit whatDidIHit = new RaycastHit();

		if (Physics.Raycast (transform.position, forward, out whatDidIHit, 20)) {
			GameObject platform = whatDidIHit.transform.gameObject;
			if ((platform.tag == "JumpPlatform") && platform.GetComponent <Platform> ().seen == false) {
				Instantiate( platform, new Vector3(Random.Range(-5f, 5f), platform.transform.position.y+heightNew, Random.Range(-5f, 5f)),Quaternion.identity);
				Instantiate( platform, new Vector3(Random.Range(-5f, 5f), platform.transform.position.y+heightNew+2, Random.Range(-5f, 5f)),Quaternion.identity);
				Debug.Log ("create new platform");
				platform.GetComponent <Platform> ().seen = true;
			}
			float heightPlatform = platform.transform.position.y;
			if (platform.tag == "JumpPlatform" && transform.position.y <= (heightPlatform + 0.3f)) {
				Debug.Log ("jump");
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards(transform.position, new Vector3 (platform.transform.position.x, platform.transform.position.y + 0.1f, platform.transform.position.z), step);
				Debug.Log ("me" + transform.position.y);
				Debug.Log ("platform" + platform.transform.position.y);
				if (transform.position.y >= (heightPlatform - 0.5f)) {
					transform.position = new Vector3(transform.position.x, (heightPlatform + 0.3f), transform.position.z);
				}
			}
		}

		if (Cardboard.SDK.Triggered) {
			gameObject.AddComponent<Rigidbody> ();
			Debug.Log ("Haha");
			GetComponentInChildren <AudioSource> ().Play ();
//			GameObject water = GameObject.Find ("WaterBasicDaytime");
//			water.GetComponentInChildren <AudioSource> ().Play ();
		}

	}
}
