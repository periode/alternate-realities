using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; //we need to add that line to use Photon-specific methods

//here, we change MonoBehaviour to Photon.PunBehaviour
public class MyNetwork : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}

	//by default, we join a lobby. once that's done, we want to join our room right away
	public override void OnJoinedLobby () {
		
		PhotonNetwork.JoinRandomRoom ();

	}

	//once we've joined our room, we want to instantiate an object for our player to control
	public override void OnJoinedRoom ()
	{
		GameObject being = PhotonNetwork.Instantiate ("Being", new Vector3(0, 2, 0), Quaternion.identity, 0);
		being.GetComponent<BeingController> ().enabled = true;

		//TODO change that so that only one player can control the instatiated being
	}

	public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
	{
		PhotonNetwork.CreateRoom ("AlternateRealities", new RoomOptions(){MaxPlayers = 4}, null);
	}
}
