using System.Collections.Generic;

using UnityEngine;
using System.Collections;


public class triggerenter : MonoBehaviour
{
    public GameObject tunnel;

    //GameObject[] all_tunnels; 
    //float tunnelx = 46;
    

    void OnTriggerEnter(Collider other)
    {
        float thisx = this.transform.position.x; //-10.79.. 39.346
       
        if (other.gameObject.tag == "collider")
        {
            GameObject.Find("Environment").GetComponent<ProceduralGenerator>().triggered(other, thisx); 
        }

    }


}