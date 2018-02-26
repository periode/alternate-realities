using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class applemovement : MonoBehaviour {

    public GameObject apple;
    
 
    // Use this for initialization
    void Start () {
      
    }
	
	// Update is called once per frame
	void Update () {

        float applex = apple.transform.position.x;
        float applez = apple.transform.position.z;
        

        for (float appley= 30; appley>0; appley-=0.01f) {
           
            apple.transform.position = new Vector3(applex, appley, applez);
           }
	}
}
