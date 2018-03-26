using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon; // in order to access Photon-specific variables, we include this line

// here, we change MonoBehaviour to Photon.MonoBehaviour
public class BeingController : Photon.MonoBehaviour
{

    float speed = 0.1f;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        // the variable photonView.isMine returns TRUE if the current GameObject belongs to this user
        // i.e. has been instantiated by the player through PhotonNetwork.Instantiate (see MyNetwork.cs, line 41)
        // or if ownership has been transferred to the player (not that important right now)

        if (photonView.isMine) //are you the part of the application that called 'PhotonNetwork.Instantiate()'?
            Movement();

        // putting the Movement() method inside the if statement allows us to make sure that our user's input
        // will only affect one object at a time.

    }

    void Movement()
    {
        //move forward
        if (Input.GetKey(KeyCode.UpArrow))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z + speed);

        //move back
        if (Input.GetKey(KeyCode.DownArrow))
            transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z - speed);

        //move right
        if (Input.GetKey(KeyCode.RightArrow))
            transform.position = new Vector3(transform.position.x + speed, transform.position.y, transform.position.z);

        //move left
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.position = new Vector3(transform.position.x - speed, transform.position.y, transform.position.z);

        //make everyone else jump
        if (Input.GetKeyDown(KeyCode.Space))
            ForceJump(Vector3.up);

    }

    [PunRPC]
    void ForceJump(Vector3 dir)
    {

        GetComponent<Rigidbody>().AddForce(dir * 8, ForceMode.Impulse);

        if (photonView.isMine)
            photonView.RPC("ForceJump", PhotonTargets.Others, dir);
    }
}