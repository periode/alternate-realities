using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour {

    public GameObject fren;

    [Range(1, 5)]
    public float period = 1;
    float timer = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (timer >= period) {
            Instantiate(fren, new Vector3(Random.Range(-3f, 3f), 0, Random.Range(-3f, 3f)), Quaternion.identity);
            timer = 0;
        }

        timer += Time.deltaTime;
	}
}
