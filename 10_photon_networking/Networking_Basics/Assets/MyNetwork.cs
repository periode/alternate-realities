using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; //we need to add that line to use Photon-specific methods

// here, we change MonoBehaviour to Photon.PunBehaviour
// so that we can override some methods offered by Photon (lines 27-48)
public class MyNetwork : Photon.PunBehaviour {

	// Use this for initialization
	void Start () {
		//this line allows us to connect to the network. The only argument is the version of the application.
		PhotonNetwork.ConnectUsingSettings ("0.1");
	}

	// Update is called once per frame
	void Update () {

	}

	void OnGUI(){
		//this is a simple way to display the connection state on the screen itself, instead of in the Console
		GUILayout.Label (PhotonNetwork.connectionStateDetailed.ToString ());
	}

	//by default, we join a lobby. once that's done, we want to join our room right away
	public override void OnJoinedLobby () {
		// once we've joined the lobby (make sure you check the Auto-Join Lobby setting in Window>Photon Networking Settings)
		// we tell Photon to join a random room inside our application
		// essentially, that means that, if there is a room available, we will join it
		// if that fails, Photon will call OnPhotonRandomJoinFailed() (line 43)
		PhotonNetwork.JoinRandomRoom ();

	}

	// once we've joined our room, we want to instantiate an object for our player to control
	public override void OnJoinedRoom ()
	{
		// here, we use PhotonNetwork.Instantiate instead of GameObject.Instantiate to make sure that the GameObject we Instantiate
		// will be kept track of by Photon in order to update its information in MyNetworkCommunication.cs
		GameObject being = PhotonNetwork.Instantiate ("Being", new Vector3(0, 2, 0), Quaternion.identity, 0);

		// in order for this prefab to be instantiated, it should be located in Assets/Resources folder
	}

	public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
	{
		// if no room is available, then we create a new one (which means that at least one room will be available for future users to join)
		PhotonNetwork.CreateRoom ("AlternateRealities", new RoomOptions(){MaxPlayers = 8}, null);
	}
}
