using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardboardBehaviour : MonoBehaviour {


	float scaleDec = 0.05f;
	public int wallsDown;
	public bool finish;
	// Use this for initialization
	void Start () {
		finish = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!finish) {
			transform.position = transform.position + Camera.main.transform.forward * 0.6f * Time.deltaTime;
			Raycast ();
		}
		if (finish) {
			transform.position = transform.position;
			GameObject[] allWalls = GameObject.FindGameObjectsWithTag ("WallToMove");
			for (int i = 0; i < allWalls.Length; i++) {
				//if (allWalls[i].transform.position.y > -0.5){
				allWalls[i].transform.gameObject.tag = "FallAfter";
			}
			GameObject[] allRise = GameObject.FindGameObjectsWithTag ("Rise");
			for (int i = 0; i < allRise.Length; i++) {
				allRise [i].GetComponent<Transform> ().Translate (new Vector3 (0.0f, 0.02f, 0.0f));
			}
			GameObject[] allFallAfter = GameObject.FindGameObjectsWithTag ("FallAfter");
			for (int j = 0; j < allFallAfter.Length; j++) {
				allFallAfter [j].GetComponent<Transform> ().Translate (new Vector3 (0.0f, -0.02f,0.0f));
			}
			GameObject plane = GameObject.Find ("Plane");
			plane.SetActive (false);
		}

		//transform.position += transform.forward * Time.deltaTime*0.2f;
	}

	void OnCollisionEnter(Collision collision) {
		if(collision.transform.gameObject.tag == "Finish")
		{
			finish = true;
			GameObject go = gameObject;
			go.GetComponent<AudioSource> ().Stop ();
			GameObject a = GameObject.FindGameObjectWithTag ("Noise");
			a.GetComponent<AudioSource> ().Stop ();
			GameObject b = GameObject.FindGameObjectWithTag ("EndingNoise");
			b.GetComponent<AudioSource> ().Play ();
		}
	}

	void Raycast(){
		Vector3 directionOfRay = GetComponentInChildren<Camera> ().transform.TransformDirection (Vector3.forward);

		RaycastHit informationAboutHit = new RaycastHit ();

		if(Physics.Raycast(transform.position, directionOfRay, out informationAboutHit, 20)){
			
			if(informationAboutHit.transform.gameObject.tag == "WallToMove"){
				informationAboutHit.transform.gameObject.GetComponent<Transform>().position = new Vector3 (informationAboutHit.transform.gameObject.GetComponent<Transform>().position.x, informationAboutHit.transform.gameObject.GetComponent<Transform>().position.y - scaleDec, informationAboutHit.transform.gameObject.GetComponent<Transform>().position.z);
				GameObject go = gameObject;
				if(go.GetComponent<AudioSource>().isPlaying == false){
						go.GetComponent<AudioSource> ().Play ();
				}
				//Once this goes y<-0.5 then wall is gone so active the move for the cardboard;
			}


		}
	}
}
