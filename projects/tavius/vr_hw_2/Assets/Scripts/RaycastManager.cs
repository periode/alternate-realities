using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour {

    public int raycastDistance;
    public LayerMask layers;
    public float sitTimer = 3.0f;
    public float viewTime = 0.0f;
    public GameObject inView = null;
    public bool sitting = false;

    public string inViewName;

	// Use this for initialization
	void Start () {
        viewTime = Time.time;
    }
	
	// Update is called once per frame
	void Update () {
        Sitcast();
	}

    void Sitcast()
    {
        Vector3 forward = transform.forward;
        RaycastHit hit;

        if(Physics.Raycast(transform.position, forward, out hit, raycastDistance))
        {
            Debug.DrawLine(transform.position, end: hit.point);
            //Debug.Log("Looking at " + hit.collider.gameObject.name);
            if (hit.collider.gameObject.tag == "Sit")
            {
                // Check the timer if object is the same:
 //               if (inViewName == hit.collider.name)
 //               {
                    if (Time.time - viewTime > sitTimer)
                    {
                        Sit();
                    }
 //               }
            }
            // Otherwise reset the variables:
            else
            {
                viewTime = Time.time;
                inView = hit.collider.gameObject;
                inViewName = hit.collider.name;
            }

            // Issue, timer doesn't count. Collider is continually colliding.
            // This is where hit.collider.name comes into account, check it.
            Debug.Log(Time.time - viewTime);
        }
    }

    void Sit()
    {
        if (sitting)
        {   // Stand
            sitting = false;
            // Move camera up, look target down, and unlock movement:
            GameObject.Find("GvrEditorEmulator").transform.Translate(0f, 0.5f, 0f);
            GameObject.Find("GvrEditorEmulator").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            GameObject.Find("Sit Cylinder").transform.Translate(0f, -3f, 0f);
        }
        else
        {   // Sit
            sitting = true;
            // Move camera down, look target up, and lock movement:
            GameObject.Find("GvrEditorEmulator").transform.Translate(0f, -0.5f, 0f);
            GameObject.Find("GvrEditorEmulator").GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            GameObject.Find("Sit Cylinder").transform.Translate(0f, 3f, 0f);
        }
    }

    // On inView collision {
    // This becomes a constant on OBJECT ENTER
    //     viewTime = Time.time;
    // }
    // Every time the raycast enters a 'Sit' tagged object,
    // the viewTime will be reset to "0", Time.time.

    // if (Time.time - viewTime > sitTimer) {
    //     Sit();
    // }
}
