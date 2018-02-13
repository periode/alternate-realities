using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeBehavior : MonoBehaviour {

    float timer = 0;
    public float period = 1f;
    Vector3 vel = new Vector3();
    [Range(0, 5)]
    public float speed = 1;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        float t = (timer % period) / period;

        GetComponent<MeshRenderer>().material.color = new Color(1, 0, t);

        vel.x = speed*Mathf.Cos(Mathf.PI * 2 * t);
        vel.z = speed*Mathf.Sin(Mathf.PI * 2 * t);

        transform.position += vel * Time.deltaTime;

        timer += Time.deltaTime;
	}
}
