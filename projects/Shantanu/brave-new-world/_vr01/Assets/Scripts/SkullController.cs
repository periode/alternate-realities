using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullController : MonoBehaviour {
    /*
     * So what's a skull do?
     * It faces the player, it rushes the player, and when it reaches the player, it turns all the lights off and loads a new scene
     */
    // Use this for initialization
    public GameObject player;
    public float speed;
	void Start () {
        player = GameObject.Find("player");
        transform.LookAt(player.transform);
	}
	
	// Update is called once per frame
	void Update () {
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, player.transform.position, step);
	}

}
