using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairySpawnerController : MonoBehaviour {
    /*
     * This script will spawn a fairy inside the collider once every X seconds
     */

    public GameObject fairy;
    public float fairySpawnDelay;
    RayCastInput rci; 

	// Use this for initialization
	void Start () {
        InvokeRepeating("spawnFairy", fairySpawnDelay, fairySpawnDelay);
        rci = Camera.main.GetComponent<RayCastInput>();
	}
	
	// Update is called once per frame
	void Update () {
        if (rci.stopSpawning) {
            CancelInvoke();
        }
	}

    void spawnFairy() {
        Instantiate(fairy, GetPositionInsideObject(), Quaternion.identity);
    }

    public Vector3 GetPositionInsideObject() {

        float xRange = GetComponent<BoxCollider>().size.x;
        xRange /= 2f;
        float yRange = GetComponent<BoxCollider>().size.y;
        yRange /= 2f;
        float zRange = GetComponent<BoxCollider>().size.z;
        zRange /= 2f;

        return new Vector3(transform.position.x + Random.Range(-xRange, xRange), 
                            transform.position.y + Random.Range(-yRange, yRange), 
                            transform.position.z + Random.Range(-zRange, zRange)
                            
                            );
    }
}
