using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InClassRaycast : MonoBehaviour {

    public int rcd;
    public LayerMask mask;
    Vector3 destPos; //...cito, Quiero respirar tu cuello despacito / Deja que te diga cosas al oído

    // Use this for initialization
    void Start () {
        Vector3 destPos;
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 fwd = transform.forward;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, fwd, out hit, rcd, mask)) {
            Debug.Log("Did a thing!");
        }
        else {
            Debug.ClearDeveloperConsole();
        }
	}
}
