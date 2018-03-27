using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

// Class to manage our network
public class NetworkManager : Photon.PunBehaviour {
    string[] roomNames = { "room", "another", "stupid", "pls work", "fak" };
    int randIndex;
    RoomInfo[] rooms;

    enum AvatarType {
        Demo,
        VR
    }

    [SerializeField]
    AvatarType avatarType;

    string avatarName = "";

    // Use this for initialization
    void Start() {
        switch (avatarType) {
            case AvatarType.Demo:
                avatarName = "Avatar_Demo";
                break;
            case AvatarType.VR:
                avatarName = "Avatar_VR";
                break;
        }

        PhotonNetwork.ConnectUsingSettings("0.1");

        randIndex = Random.Range(0, roomNames.Length);
    }

    // Update is called once per frame
    void Update() {
        
    }

    private void OnGUI() {
        if (!PhotonNetwork.connected) {
            GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString());
        }
        else if (PhotonNetwork.room == null) {
            if (GUI.Button(new Rect(100, 100, 250, 100), "New " + roomNames[randIndex])) {
                PhotonNetwork.CreateRoom(roomNames[randIndex], new RoomOptions() { MaxPlayers = 6, IsVisible = true }, null);
            }

            if (rooms != null) {
                for (int i = 0; i < rooms.Length; ++i) {
                    RoomInfo r = rooms[i];
                    if (GUI.Button(new Rect(100, 250 + 110 * i, 250, 100), "Join " + r.Name)) {
                        PhotonNetwork.JoinRoom(r.Name);
                    }
                }
            }
        }
    }

    public override void OnJoinedRoom() {
        //base.OnJoinedRoom();

        PhotonNetwork.Instantiate(avatarName, Vector3.up * 2, Quaternion.identity, 0);
    }

    public override void OnReceivedRoomListUpdate() {
        Debug.Log("henlo");
        rooms = PhotonNetwork.GetRoomList();
    }

    public override void OnPhotonRandomJoinFailed(object[] codeAndMsg) {
        //base.OnPhotonRandomJoinFailed(codeAndMsg);
        PhotonNetwork.CreateRoom(roomNames[randIndex], new RoomOptions() { MaxPlayers = 8 }, null);
    }

    public override void OnJoinedLobby() {
        //base.OnJoinedLobby();
        Debug.Log("dis lobby");
    }
}
