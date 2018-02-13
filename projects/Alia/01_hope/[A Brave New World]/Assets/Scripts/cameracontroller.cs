using UnityEngine;
using System.Collections;

public class cameracontroller : MonoBehaviour
{

    //public float speed;
    float tunnelx;
    public GameObject tunnel;
    float speed = 10;
    void Update()

    {
        int x = 0;
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.Translate(new Vector3(0, 0, -speed * Time.deltaTime));
        }
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.Translate(new Vector3(0, 0, speed * Time.deltaTime));
            x++;
            if (x%10==0)
            {
                speed += 2;
            }
           
           

        }
    }

   
}




