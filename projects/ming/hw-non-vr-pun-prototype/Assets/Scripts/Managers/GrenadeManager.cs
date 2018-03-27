using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

/*
 * Grenade logic:
 * Bounce around for the specified seconds and then die
 */


public class GrenadeManager : Photon.MonoBehaviour {

    public float grenadeDespawnDelay;

    // Position sync 
    Vector3 targetPos;

    // Reference to our grenade launcher
    GrenadeGunManager gun = null;
    // Public API allows it to be set only once
    public GrenadeGunManager Gun {
        get { return gun; }
        set {
            if (gun == null) {
                gun = value;
            }
        }
    }

    // Use this for initialization
    void Start() {
        // Set to kinematic in remote machines - we are lerping the position anyway
        if (!photonView.isMine) {
            GetComponent<Rigidbody>().isKinematic = true;
        } else {
            StartCoroutine(DeathDelay());
        }
    }

    // Update is called once per frame
    void Update() {
        if (!photonView.isMine) {
            transform.position = Vector3.Lerp(transform.position, targetPos, Time.deltaTime * 5f);
        }
    }

    IEnumerator DeathDelay() {
        yield return new WaitForSeconds(grenadeDespawnDelay);

        // Tell our gun that we are despawning
        if (gun) {
            gun.DespawnGrenade();
        }

        PhotonNetwork.Destroy(gameObject);
    }

    void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
        if (stream.isWriting) {
            stream.SendNext(transform.position);
        } else {
            targetPos = (Vector3)stream.ReceiveNext();
        }
    }
}
