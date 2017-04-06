using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; //we need to add that line to use Photon-specific methods

// here, we change MonoBehaviour to Photon.PunBehaviour
// so that we can override some methods offered by Photon (lines 27-48)
public class MyNetwork : Photon.PunBehaviour {


	public GameObject playerPrefab;

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
		// here, Instantiate to make sure that the GameObject we Instantiate
		// will NOT be kept track of by Photon
		GameObject player = GameObject.Instantiate (playerPrefab, new Vector3(0, 0, 0), Quaternion.identity);

		// however, we do not want to synchronize the whole camera rig, or that would cause maddening problems of who controls what.
		// instead, we are going to instantiate simple prefabs over the network and attach them to the camerarig to represent their position to other users

		// using PhotonNetwork.Instantiate the Head prefab to keep track of it over the network
		GameObject playerHead = PhotonNetwork.Instantiate ("Head", transform.position, transform.rotation, 0);
		// attaching it to the Camera (head) object -it is the 3rd child of our player GameObject
		playerHead.transform.parent = player.transform.GetChild (2).transform;

		// for the hands, we attach prefabs to each controllers
		// using PhotonNetwork.Instantiate the Head prefab to keep track of it over the network
		GameObject handLeft = PhotonNetwork.Instantiate ("Hand", transform.position, transform.rotation, 0);
		// attaching it to the Camera (head) object -it is the 1st child of our player GameObject
		handLeft.transform.parent = player.transform.GetChild (0).transform;

		GameObject handRight = PhotonNetwork.Instantiate ("Hand", transform.position, transform.rotation, 0);
		// attaching it to the Camera (head) object -it is the 2nd child of our player GameObject
		handRight.transform.parent = player.transform.GetChild (1).transform;


		// in order for this prefab to be instantiated, it should be located in Assets/Resources folder
		// and each of these prefabs have a PhotonView component and a MyNetworkCommunication component
	}

	public override void OnPhotonRandomJoinFailed (object[] codeAndMsg)
	{
		// if no room is available, then we create a new one (which means that at least one room will be available for future users to join)
		PhotonNetwork.CreateRoom ("AlternateRealities", new RoomOptions(){MaxPlayers = 8}, null);
	}
}
