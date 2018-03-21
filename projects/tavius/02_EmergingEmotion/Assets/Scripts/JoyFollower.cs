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

	void Start () {
        joyRB = gameObject.GetComponent<Rigidbody>();
	}

    void Update()
    {
        joyVector = gameObject.transform.position;
        playerVector = VRTK_DeviceFinder.DeviceTransform(VRTK_DeviceFinder.Devices.Headset).position;
		seekVector = playerVector - joyVector;
        dist = Vector3.Distance(joyVector, playerVector);
		// If far away, seek the player at a speed propotional to the distance:
		if (dist > 2f)
		{
			joySpeed = dist / 9f;
			joyRB.AddForce(seekVector * joySpeed);
		}
		// If getting close, slow down:
		if (dist <= 2f && dist > 0.65f)
		{
			joySpeed = dist / 14f;
			joyRB.AddForce(seekVector * joySpeed);
		}
		// If close, float up:
		if (dist <= 0.65f)
		{
			seekVector += Vector3.up;
			joySpeed = dist * 1.2f;
			joyRB.AddForce(seekVector * joySpeed);
		}
    }
}
