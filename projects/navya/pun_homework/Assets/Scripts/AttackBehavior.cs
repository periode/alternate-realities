using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon;

public class AttackBehavior : Photon.MonoBehaviour {

//	public Transform target;
//	public float speed;
	Vector3 newPos;
	GameObject player;
	Rigidbody rb;
    public int attackerID;
    public Color attackerColor;
	float starterTime;
	float curTime;

	void Start() {
		GetComponent<Renderer>().material.color = attackerColor;
		rb = gameObject.GetComponent<Rigidbody>();
        // Needed to be transform.forward, not transform.position.
        // Moved to Start to avoid acceleration.
        rb.AddForce(transform.forward * 500f);
		starterTime = Time.time;
    }

    void Update() {
		curTime = Time.time;
		if (curTime - starterTime > 2f) {
			Destroy (gameObject);
		}
	}

	void OnCollisionEnter(Collision collision)
	{
        if (collision.gameObject.CompareTag("Player")) // Check for a Player.
        {
            // If the attack is not colliding with the Player who sent it, destroy them:
            if (collision.gameObject.GetInstanceID() != attackerID)
                Destroy(collision.gameObject);
            Destroy(gameObject); // Destroy the attack on any collision.
        }
	}

    //[PunRPC] // Used to flag methods as remote-callable.
    //void Attack(Vector3 dir)
    //{

    //    GetComponent<Rigidbody>().AddForce(dir * 8, ForceMode.Impulse);

    //    if (photonView.isMine)
    //        photonView.RPC("ForceJump", PhotonTargets.Others, dir);
    //}

}
