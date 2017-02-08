using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour {

	public GameObject brick;
	public int brickNum;

	// Use this for initialization
	void Start () {

		while(brickNum > 0){
			GameObject.Instantiate ((Object)brick, new Vector3 (Random.Range (-5.0f, 5.0f), Random.Range (-5.0f, 5.0f), Random.Range (-5.0f, 5.0f)), Quaternion.identity);
			brickNum--;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if(Cardboard.SDK.Triggered){
			GameObject[] bricks = GameObject.FindGameObjectsWithTag ("BrickTag");
			for(int i = 0; i < bricks.Length; i++){
				bricks [i].transform.localScale *= Random.Range (0.5f, 1.5f);
			}
		}
	}
}
