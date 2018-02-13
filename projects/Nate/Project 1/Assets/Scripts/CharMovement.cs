using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharMovement : MonoBehaviour {

    public float speed = 10f;

    public LayerMask layerMask;
    public float raycastDistance;

    bool start;
    bool left;
    bool right;

    float factor;
    float[,] data = new float[,] { {3.598f,5.724f,8.296f,10.328f,13.795f,17.764f,21.57f,25.656f},
                                   {0.4548f, 0.752f, 0.7918f, 0.6318f, 1.73764f, 1.44736f, 1.26072f, 1.22392f},
                                   { 150.8f, 300f, 34f, 40f, 21f, 23f, 27f, 26f },
                                   { 2.07f, 1.62f, 1f, 0.49f, 0.09f, 0.04f, 0.01f, 0.004f }};

    GameObject sun;
    GameObject mercury;
    GameObject venus;
    GameObject earth;
    GameObject mars;
    GameObject jupiter;
    GameObject saturn;
    GameObject uranus;
    GameObject neptune;

    // Use this for initialization
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        sun = GameObject.FindGameObjectWithTag("Sun");
        mercury = GameObject.FindGameObjectWithTag("Mercury");
        venus = GameObject.FindGameObjectWithTag("Venus");
        earth = GameObject.FindGameObjectWithTag("Earth");
        mars = GameObject.FindGameObjectWithTag("Mars");
        jupiter = GameObject.FindGameObjectWithTag("Jupiter");
        saturn = GameObject.FindGameObjectWithTag("Saturn");
        uranus = GameObject.FindGameObjectWithTag("Uranus");
        neptune = GameObject.FindGameObjectWithTag("Neptune");

        GameObject[] planets = new GameObject[] { mercury, venus, earth, mars, jupiter, saturn, uranus, neptune };

        Raycasting();

        if (start == true)
        {
            if (left == true)
            {
                factor = 0.1f;
            }
            else if (right == true)
            {
                factor = 5f;
            }
            else
            {
                factor = 1f;
            }
        }
       
        for (int i = 0; i < planets.Length; i++)
        {
            planets[i].transform.Rotate(0, (data[2, i] / 10), 0);
            planets[i].transform.RotateAround(sun.transform.position, Vector3.up, data[3, i] * factor);
        }


        float translation = Input.GetAxis("Vertical") * speed;
        translation *= Time.deltaTime;

        transform.Translate(0, translation, 0);

        if (Input.GetKeyDown("escape"))
        {
            Cursor.lockState = CursorLockMode.None;
        }


    }

    void Raycasting()
    {
        Vector3 fwd = transform.TransformDirection(Vector3.forward); //what is the direction in front of us
        RaycastHit hit = new RaycastHit();

        if (Physics.Raycast(transform.position, fwd, out hit, raycastDistance, layerMask))
        {
            //Debug.Log("hit object: " + hit.collider.gameObject.name);

            if (hit.collider.gameObject.name == "Sun")
            {
                start = true;
                if (Input.GetKeyDown(KeyCode.LeftArrow))
                {
                    if (right != true)
                    {
                        left = true;
                    }
                    right = false;
                }
                else if (Input.GetKeyDown(KeyCode.RightArrow))
                {
                    if (left != true)
                    {
                        right = true;
                    }
                    left = false;
                } 
            }
        }
    }
}