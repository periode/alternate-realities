using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProcedureGenerator : MonoBehaviour {

	public GameObject room;
	public GameObject skull;
	public GameObject bone;
	public AudioSource shakeSound;
	public AudioSource bgMusic;

	GameObject[] walls;

//		GameObject top;
//		GameObject bottom ;
//		GameObject xLess;
//		GameObject xMore;
//		GameObject zLess;
//		GameObject zMore;

	Vector3[] start;
	Vector3[] end;
	Vector3 smallBuffer = new Vector3 (0.2f, 0.2f, 0.2f);
//	public Vector3[] end;

	float lerpAmt = 0f;
	float lerpSpeed = 0.001f;
	bool last = false;

	// Use this for initialization
	void Start () {

		Transform[] myArray = room.GetComponentsInChildren<Transform> ();

		walls = new GameObject[myArray.Length-1];
		start = new Vector3[myArray.Length - 1];
		end = new Vector3[myArray.Length - 1];


		end [0] = start [1] + smallBuffer;
		end [1] = start [1];
		end [2] = start [3] - smallBuffer;
		end [3] = start [2] + smallBuffer;
		end [4] = start [5] - smallBuffer;
		end [5] = start [4] + smallBuffer;

		for (int i = 0; i < myArray.Length-1; i++) {
			walls [i] = myArray [i+1].gameObject;
			start [i] = walls [i].transform.position;
		}

		Debug.Log ("walls" + walls[0].name);	

		bgMusic.GetComponent<AudioSource> ().Play ();
	}
	
	// Update is called once per frame
	void Update () {

		for (int i = 0; i < walls.Length; i++) {
			walls[i].transform.position = Vector3.Lerp (start [i], end [i], lerpAmt);
		}

		lerpAmt += lerpSpeed;

		if (Input.GetMouseButtonDown (0)) {
			if (!last)
				lerpSpeed -=0.0001f;
			SpawnSkull (walls);
			shakeSound.GetComponent<AudioSource> ().Play ();
			 	
		}

		if (lerpSpeed < 0) {
			last = true;
			lerpSpeed = 0.0001f;
		}

		if (start == end)
			Application.Quit();
		
	}

	void SpawnSkull(GameObject[] walls){

		Vector3[] positions = new Vector3[walls.Length];
		for (int i = 0; i < positions.Length; i++) {
			positions [i] = walls [i].transform.position;
		}
		Vector3 top = positions [0];
		float myY = top.y;
		float myX = Random.Range (positions [2].x, positions [3].x);
		float myZ = Random.Range (positions [4].z, positions [5].z);

		GameObject[] choice = { skull, bone };
		int chosen = Random.Range (0, 2);

		Instantiate (choice[chosen], new Vector3 (myX, myY-0.1f, myZ), Quaternion.identity);
	}

		
}