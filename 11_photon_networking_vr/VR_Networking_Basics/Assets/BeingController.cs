using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; // in order to access Photon-specific variables, we include this line

// here, we change MonoBehaviour to Photon.MonoBehaviour
public class BeingController : Photon.MonoBehaviour {

	float speed = 0.1f;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		// the variable photonView.isMine returns TRUE if the current GameObject belongs to this user
		// i.e. has been instantiated by the player through PhotonNetwork.Instantiate (see MyNetwork.cs, line 41)
		// or if ownership has been transferred to the player (not that important right now)

		if(photonView.isMine)
			Movement ();

		// putting the Movement() method inside the if statement allows us to make sure that our user's input
		// will only affect one object at a time.

	}

	void Movement(){
		//move forward
		if (Input.GetKey (KeyCode.UpArrow))
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z + speed);

		//move back
		if (Input.GetKey (KeyCode.DownArrow))
			transform.position = new Vector3 (transform.position.x, transform.position.y, transform.position.z - speed);

		//move right
		if (Input.GetKey (KeyCode.RightArrow))
			transform.position = new Vector3 (transform.position.x + speed, transform.position.y, transform.position.z);

		//move left
		if (Input.GetKey (KeyCode.LeftArrow))
			transform.position = new Vector3 (transform.position.x - speed, transform.position.y, transform.position.z);

		//make everyone else jump
		if (Input.GetKeyDown (KeyCode.Space))
			photonView.RPC ("MakeOthersJump", PhotonTargets.All);
			
	}

	[PunRPC]
	void MakeOthersJump(){

		//we find all the players right now
		GameObject[] gos = GameObject.FindGameObjectsWithTag ("Being");

		//we go through all of them and we make each of them jump, except for the one that is us
		for(int i = 0; i < gos.Length; i++){
			
			if(gos[i] != this.gameObject){//if it is not our gameObject...

				//add an upwards force
				gos[i].GetComponent<Rigidbody> ().AddForce (Vector3.up * 8, ForceMode.Impulse);
			}
		}

	}
}
