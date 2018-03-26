using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; // To use Photon-specific methods

// MonoBehaviour -> Photon.PunBehaviour
// Now we can override Photon methods.
public class Network : Photon.PunBehaviour
{
    string room_name = "Atrium";
    RoomInfo[] all_rooms;
    string Player = "Player";

    void Start()
    {
        // Allows us to connect to the network. The only argument is the version of this application.
        PhotonNetwork.ConnectUsingSettings("0.1");
    }

    void Update()
    {

    }

    // This is a simple way to display the connection state on the screen itself, instead of in the Console:
    void OnGUI()
    {
        if (!PhotonNetwork.connected)
        { // If the player isn't currently connected to the Photon Cloud:
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString()); // Display log messages.
        }
        else if (PhotonNetwork.room == null)
        { // Else, if you're connected and not yet in a room, display a clickable button to create a room:
            if (GUI.Button(new Rect(100, 100, 250, 100), "Create a Room")) // This line creates a GUI Button.
                PhotonNetwork.CreateRoom(room_name, new RoomOptions() { MaxPlayers = 6, IsVisible = true }, null); // This line is called on click.

            if (all_rooms != null)
            { // If we have some rooms to display:
                for (int i = 0; i < all_rooms.Length; i++)
                { // Loop and create buttons for each available room:
                    if (GUI.Button(new Rect(100, 250 + (110 * i), 250, 100), "Join " + all_rooms[i].Name))
                        PhotonNetwork.JoinRoom(all_rooms[i].Name); // Join the room that the user clicked on!
                }
            }
        }
    }

    // Once we've joined our room, we want to instantiate an object for our player to control:
    public override void OnJoinedRoom()
    {
        // Use PhotonNetwork.Instantiate instead of GameObject.Instantiate to make sure that the GameObject we Instantiate
        // will be kept track of by Photon in order to update its information in NetworkCommunicator.cs, attached to the object.
        PhotonNetwork.Instantiate(Player, new Vector3(0, 0.5f, 0), Quaternion.identity, 0);

        // NOTE: Prefab should be located in the Assets/Resources folder.
    }

    // Photon automatically calls this function when a room is created or removed:
    public override void OnReceivedRoomListUpdate()
    {
        all_rooms = PhotonNetwork.GetRoomList();
    }

    // By Photon default, we join a lobby. This will join a room right away:
    public override void OnJoinedLobby()
    {
        // Once we've joined the lobby, (tick the Auto-Join Lobby setting in Assets>Resources>PhotonServerSettings)
        // tell Photon to join a random room inside our application.
        // Essentially, that means that if a room is available, we will join it.
        // If that fails, Photon will call OnPhotonRandomJoinFailed() below, creating a room.

        PhotonNetwork.JoinRandomRoom();
    }

    // If no room is available, then create a new one (so at least one room will be available for future users to join):
    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg)
    {
        PhotonNetwork.CreateRoom(room_name, new RoomOptions() { MaxPlayers = 8 }, null);
    }
}
