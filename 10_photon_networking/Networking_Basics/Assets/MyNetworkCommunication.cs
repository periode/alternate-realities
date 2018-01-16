using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; // again, we use Photon

public class MyNetworkCommunication : Photon.MonoBehaviour { // and replace MonoBehaviour with Photon.MonoBehaviour

	// we declare two variables to store the position and rotation of our GameObject as received from the network
	Vector3 targetPosition;
	Quaternion targetRotation;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		// if this is not the GameObject we control, we have to deal with data that doesn't update smoothly
		// so we constantly Lerp between the position of the GameObject as we see it
		// and the position that we receive on lines 39-40
		if(!photonView.isMine){
			this.transform.position = Vector3.Lerp (this.transform.position, targetPosition, Time.deltaTime * 5f);
			this.transform.rotation = Quaternion.Lerp (this.transform.rotation, targetRotation, Time.deltaTime * 5f);
		}
	}

	// this is the method that allows us to serialize data and send/receive it over the network
	// this is how we update the position of all our users for everybody
	public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info){

		if(stream.isWriting){//if we own this GameObject, send the data to other players

			stream.SendNext (transform.position); // first we send the position
			stream.SendNext (transform.rotation); // then we send the rotation

		}else{//if we don't own this object, update their data according to what we receive

			targetPosition = (Vector3)stream.ReceiveNext(); // we store the received position in our targetPosition
			targetRotation = (Quaternion)stream.ReceiveNext (); // we store the received rotation in our targetRotation

		}
	}
}
