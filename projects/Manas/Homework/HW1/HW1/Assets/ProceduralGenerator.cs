using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {
    
    public List<GameObject> points = new List<GameObject>();

    public GameObject myPrefab;
    public GameObject myPrefab1;
    public GameObject myPrefab2;


	// Use this for initialization
	void Start () {
        points.Add(myPrefab as GameObject);
        points.Add(myPrefab1 as GameObject);
        points.Add(myPrefab2 as GameObject);


	}
	
	// Update is called once per frame
	void Update () {
        int index;
        index = Random.Range(0, 3);
        
        Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f), Random.Range(-10.0f, 10.0f));
        GameObject.Instantiate(points[index], position, Quaternion.identity);

	}
}
