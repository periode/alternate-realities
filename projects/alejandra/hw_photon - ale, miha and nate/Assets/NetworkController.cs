using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkController : Photon.MonoBehaviour {

	private bool hiding = false;

	// Use this for initialization
	void Start () {
		
		
	}
	
	// Update is called once per frame
	void Update () {
//		if (photonView.isMine)
//			HandleMovement ();
		if (Input.GetKeyDown (KeyCode.Space)) {
			Debug.Log ("Space pressed.");
			ForceJump (new Vector3(3,3,3));
		}
		if (Input.GetKeyDown (KeyCode.H)) {
			Debug.Log ("H pressed.");
			HideMe ();
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo msg){
		if (stream.isWriting)
			stream.SendNext(GetComponent<Rigidbody>().position);
		else
			GetComponent<Rigidbody>().position = (Vector3)stream.ReceiveNext();
	}

	[PunRPC] void ForceJump(Vector3 dir){
		Debug.Log (dir);
		GetComponent<PhotonView> ().RequestOwnership ();
		transform.position = dir;
		//GetComponent<Rigidbody>().addForce(dir);

		if(photonView.isMine)
			photonView.RPC("ForceJump", PhotonTargets.Others, dir);
	}

	[PunRPC] void HideMe(){
		GetComponent<PhotonView> ().RequestOwnership ();
		if (hiding == false) {
			gameObject.SetActive (false);
		} else {
			gameObject.SetActive (true);
		}
		if(photonView.isMine)
			photonView.RPC("HideMe", PhotonTargets.Others);
	}
}
