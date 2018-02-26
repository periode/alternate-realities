using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightscript : MonoBehaviour {

   
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        ProceduralGenerator pg = GameObject.Find("Environment").GetComponent<ProceduralGenerator>();

        //Debug.Log("function called, target = " + pg.altnewx);
        float ly = this.transform.position.y;
        float lz = this.transform.position.z;

       
        Vector3 endpos = new Vector3(pg.altnewx, ly, lz);
        this.transform.position = Vector3.Lerp(this.transform.position, endpos, Time.deltaTime);

    }
    
}
