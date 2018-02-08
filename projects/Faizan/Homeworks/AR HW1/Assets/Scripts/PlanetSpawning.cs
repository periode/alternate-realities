using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetSpawning : MonoBehaviour {

	public Vector3 newPos;
	public GameObject myPrefab;
	public int count;
	public float distance;
	// Use this for initialization

	void Start () {
//		this.GetComponent<PlanetBehavior>().radius = ;
//		GameObject.Instantiate(Object, newPos, Quaternion.identity);
		newPos = new Vector3 (Random.Range (5.0f, 100.0f), Random.Range (5.0f, 100.0f), Random.Range (5.0f, 100.0f));
		count = 0;
		distance = 0f;
	}
	
	// Update is called once per frame
	void Update () {
//		for (int i = 0; i < 10; i++) {
		if (count <1){
//			newPos = new Vector3 (Random.Range (5.0f, 10.0f), Random.Range (5.0f, 10.0f), Random.Range (5.0f, 10.0f));
//			newPos = new Vector3 (distance+Random.Range (5.0f, 100.0f), distance+Random.Range (5.0f, 100.0f), distance+Random.Range (5.0f, 100.0f));
			newPos = transform.position;
//			newPos.x+= (distance + Random.Range(5.0f, 100.0f));
//			newPos.y+= (distance + Random.Range(5.0f, 100.0f));
//			newPos.z+= (distance + Random.Range(5.0f, 100.0f));
			newPos.x+= 50.0f;
			newPos.y+= 50.0f;
			transform.position = newPos;
//			this.GetComponent<PlanetBehavior> ().radius += .04f;
			GameObject.Instantiate (myPrefab as Object, newPos, Quaternion.identity);
		}
		count++;
		distance += 10f;
	}
}
