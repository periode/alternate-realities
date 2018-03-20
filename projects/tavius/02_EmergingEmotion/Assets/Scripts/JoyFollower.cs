using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class JoyFollower : MonoBehaviour {

    private Vector3 joyVector;
    private Vector3 playerVector;
    private Vector3 seekVector;
    private Rigidbody joyRB;
    private float joySpeed;
    private float dist;
//    private GameObject player;

	// Use this for initialization
	void Start () {
        joyRB = gameObject.GetComponent<Rigidbody>();
	}

    // Update is called once per frame
    void Update()
    {
        joyVector = gameObject.transform.position;
        playerVector = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset).position;
        dist = Vector3.Distance(joyVector, playerVector);
        if (dist < 0.5f)
        {
			joyRB.velocity = joyRB.velocity * 0f;
			joySpeed = 0;
        }
        if (dist > 2f)
        {
            joySpeed = dist / 5f;
        }
        seekVector = playerVector - joyVector;
        joyRB.AddForce(seekVector * joySpeed);
    }
}
