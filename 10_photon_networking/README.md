## Networking Cheatsheet

#### Setting up PUN
- Download Photon Unity Networking from the Asset Store
- Open the PUN Wizard (Window > Photon Unity Networking)
- Register to get your AppID
- Copy that appID into the related field in the Inspector

#### NetworkManager

- The `NetworkManager.cs` script should be attached to an empty GameObject
- In `Start()`, call `PhotonNetwork.ConnectUsingSettings("0.1")`, in which the string represents whatever version is. Leave it at 0.1 for now.
- Here is the code for connecting to a room
```
string room_name = "Atrium";
RoomInfo[] all_rooms;
string networkedObject_name = "MyNetworkedAvatar";

void OnGUI(){	//This is called to easily display GUI elements
	if(!PhotonNetwork.connected){	//if the player isn't currently connected to the Photon Cloud
		GUILayout.Label(PhotonNetwork.connectionStateDetailed.ToString()); //display log messages
	}else if(PhotonNetwork.room == null){ //else, if you're connected and not yet in a room
		if(GUI.Button(new Rect(100, 100, 250, 100, "Create a Room"){ //display a clickable button to create a room
			PhotonNetwork.CreateRoom(room_name + Guid.NewGuid().ToString("N"), true, true, 5);
		}

		if(all_rooms != null){ //if we have some rooms to display
			for(int i = 0; i < all_rooms.Length; i++){
				if(GUI.Button(new Rect(100, 250 + (110*i), 250, 100), "Join "+all_rooms[i].name)) //create buttons for each available room
					PhotonNetwork.JoinRoom(all_rooms[i].name); //join the room that the user clicked on!
			}
		}
	}
}

void OnReceiveRoomListUpdate(){ //this function is automatically called when you get new rooms (e.g. when a room is created or removed
	all_rooms = PhotonNetwork.GetRoomList();
}

void OnJoinedRoom(){ //this is automatically called when you join a room
	Debug.Log("Joined new room!")
	PhotonNetwork.Instantiate(networkedObject_name, Vector3.zero, Quaternion.identity) //this is where you want to create your avatar
}
```

#### NetworkedObject

- Any object that is going to be networked (i.e. seen across clients), needs to have a `PhotonView.cs` component attached to it.
- These prefabs are created at runtime, not through `GameObject.Instantiate`, but through `PhotonNetwork.Instantiate`, so that it happens on all clients
- These prefabs need to be located in a `Resources` folder in your `Assets` folder.

#### NetworkedObjectController

- Attach a script (e.g. `NetworkController.cs`)to the object you want to control over the network.
- At the top of the script, replace `Monobehaviour` by `Photon.Monobehaviour`, which will give you access to additional variables/namespaces
- Now, you can check whether the current object is indeed your to control (i.e. you are the one who has called `PhotonNetwork.Instantiate()`
- To control it, you would use the variable `photonView.isMine`:
```
void Update(){
	if(photonView.isMine)
		HandleMovement()
}
```

#### Networked Communication 1: State synchronization
- State synchronization just sends data in a constant stream between similar objects over the network (i.e. the object i've instantiated, and the object instantiated on another client's machine)
- We can have access to the stream of data being sent and received through the `OnPhotonSerializeView()` function. It is called everytime data is ready to be sent or received.
- First, drag the script inside which you will have the `OnPhotonSerializeView()` function. In our case, it's the script where we've written the code above, `NetworkController.cs`
- Now, this script is being "observed" by the PhotonView, and it will pass serial data to and from it.
- In order to synchronize, say, the position of the NetworkedObject, we need to update it as such:
```
void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo msg){
	if(stream.isWriting)
		stream.SendNext(rigidbody.position);
	else
		rigidbody.position = (Vector3)stream.ReceiveNext();
}
```

#### Networked Communcation 2: Remote Procedure Calls
- Remote Procedural Calls (RPC) is a way to define a function so that it can be called over the network.
- The arguments can be floats, integers, strings, vectors or quaternions
- A required argument is the `PhotonTargets`, either `Server`, `All` or `Others`. The last two can be buffered so that the RPC is called when a new user joins.
- To do so, you need to have `[RPC]` in front of the first line of your function, such as:
```
[RPC] void ForceJump(Vector3 dir){
	rigidbody.addForce(dir);

	if(photonView.isMine)
		photonView.RPC("ForceJump", PhotonTargets.Others, dir);
}
```
- And then you would just call it as a regular function:
```
void Update(){
	if(Input.GetKeyDown(KeyCode.SPACE))
		ForceJump()
}
```
