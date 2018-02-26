using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class raycastmanager : MonoBehaviour
{
    public GameObject player;
    public int raycastDistance;
    public LayerMask layers;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Raycast();

    }

    void Raycast()
    {
         float speed = 10;
        int x = 0;
        Vector3 forward = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, raycastDistance, layers))
        {
            //Debug.Log("raycast hit!");
            player.transform.Translate(new Vector3(speed * Time.deltaTime, 0,0));
            x++;
            if (x % 10 == 0)
            {
                speed += 2;
            }


        }
    }
}