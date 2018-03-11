using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FairyController : MonoBehaviour {
    /*
     Right so what's a fairy do then?
     If it hasn't been triggered, then it just gets instantiated facing the right way, and flies off
     If it gets triggered, it triggers a coroutine that waits for a certain number of seconds, then instantiates a skull at that location
     */

    bool triggered;
    bool spawnedASkull;
    public float skullSpawnWait;
    public GameObject skull;
    public GameObject aimBox;
    public float moveAmt;

	// Use this for initialization
	void Start () {
        triggered = false;
        aimBox = GameObject.Find("fairy_aim");
        
        transform.LookAt(aimBox.transform);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown("g")) {
            triggerFairy();
        }
        if (triggered) {
            StartCoroutine(spawnASkull());
        }
        else {
            fly();  
        }
	}

    public void triggerFairy() {
        triggered = true;
    }

    IEnumerator spawnASkull() {
        yield return new WaitForSeconds(skullSpawnWait);
        if (!spawnedASkull) {
            Instantiate(skull, transform.position, transform.rotation);
            Destroy(gameObject);
            spawnedASkull = true;
        }
    }

    void fly() {
        float xMovement = Random.Range(moveAmt - 0.1f, moveAmt + 0.1f);
        Vector3 flyAmt = new Vector3(xMovement, 0f, 0f);
        transform.position = transform.position - flyAmt;
        if(transform.position.x < -85) {
            Destroy(gameObject);
        }
    }
}
