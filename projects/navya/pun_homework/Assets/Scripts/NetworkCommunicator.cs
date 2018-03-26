using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

// NOTE: MonoBehaviour -> Photon.MonoBehaviour
public class NetworkCommunicator : Photon.MonoBehaviour
{ 
    // To store the position and rotation of our GameObject as received from the network:
    Vector3 targetPosition;
    Quaternion targetRotation;

    void Start()
    {

    }

    void Update()
    {
        // Check if we control this GameObject. If not, deal with data that doesn't update smoothly
        // by constantly Lerping between the position of the GameObject as we see it,
        // and the position that we get from stream.RecieveNext of OnPhotonSerializeView.
        if (!photonView.isMine)
        {
            this.transform.position = Vector3.Lerp(this.transform.position, targetPosition, Time.deltaTime * 5f);
            this.transform.rotation = Quaternion.Lerp(this.transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // This method allows us to serialize data and send/receive it over the network,
    // This is what updates the position of this object for everybody:
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.isWriting)
        { // If we own this GameObject, send the data to other players (float/ints/vectors/quaternions)
            stream.SendNext(transform.position); // Send the position.
            stream.SendNext(transform.rotation); // Send the rotation.
        }
        else
        { // If we don't own this object, update its data according to what we receive:
            targetPosition = (Vector3)stream.ReceiveNext(); // Store the received position in our targetPosition.
            targetRotation = (Quaternion)stream.ReceiveNext(); // Store the received rotation in our targetRotation.
        }
    }
}
