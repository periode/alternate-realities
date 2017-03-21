using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class MyNetworkCommunication : Photon.MonoBehaviour {

	Vector3 targetPosition;
	Quaternion targetRotation;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(!photonView.isMine){
			this.transform.position = Vector3.Lerp (this.transform.position, targetPosition, Time.deltaTime * 5f);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime * 5f);
		}
	}

	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){
		if(stream.isWriting){//if we own this GameObject, send the data to other players

			stream.SendNext (transform.position);
			stream.SendNext (transform.rotation);

		}else{//if we don't own this object, update their data according to what we receive
			
			targetPosition = (Vector3)stream.ReceiveNext();
			targetRotation = (Quaternion)stream.ReceiveNext ();

		}
	}
}
