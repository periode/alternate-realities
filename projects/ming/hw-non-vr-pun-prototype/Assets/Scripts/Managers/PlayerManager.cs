using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

// Class to sync player to network
public class PlayerManager : Photon.MonoBehaviour {

    Vector3 targetPos;
    Quaternion targetRot;

    [Range(1, 10)]
    public float interpolationFactor = 5f;

    [SerializeField]
    Material visible;
    [SerializeField]
    Material invisible;

    // Use this for initialization
    void Start() {
        List<MeshRenderer> meshes = new List<MeshRenderer>();

        GetComponentsInChildren<MeshRenderer>(meshes);

        foreach (MeshRenderer mr in meshes) {
            mr.material = photonView.isMine ? visible : invisible;
        }
    }

    // Update is called once per frame
    void Update() {
        // update gameobject of we don't own it
        if (!photonView.isMine) {
            float factor = Time.deltaTime * interpolationFactor;
            transform.position = Vector3.Lerp(transform.position, targetPos, factor);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, factor);
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) { // write to network
            stream.SendNext(transform.position);
            stream.SendNext(transform.rotation);
        }
        else { // read from network
            targetPos = (Vector3)stream.ReceiveNext();
            targetRot = (Quaternion)stream.ReceiveNext();
        }
    }
}
