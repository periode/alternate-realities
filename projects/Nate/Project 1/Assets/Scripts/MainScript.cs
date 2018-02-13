using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainScript : MonoBehaviour
{
    GameObject mercury;
    GameObject venus;
    GameObject earth;
    GameObject mars;
    GameObject jupiter;
    GameObject saturn;
    GameObject uranus;
    GameObject neptune;

    float[,] data = new float[,] { {3.598f,5.724f,8.296f,10.328f,13.795f,17.764f,21.57f,25.656f},
                                   {0.4548f, 0.752f, 0.7918f, 0.6318f, 1.73764f, 1.44736f, 1.26072f, 1.22392f},
                                   { 150.8f, 300f, 34f, 40f, 21f, 23f, 27f, 26f },
                                   { 2.07f, 1.62f, 1f, 0.49f, 0.09f, 0.04f, 0.01f, 0.004f }};
    // Use this for initialization
    void Awake()
    {
        transform.position = new Vector3(0, 0, 0);
        transform.localScale = new Vector3(4.32288f, 4.32288f, 4.32288f);
        
        mercury = GameObject.FindGameObjectWithTag("Mercury");
        venus = GameObject.FindGameObjectWithTag("Venus");
        earth = GameObject.FindGameObjectWithTag("Earth");
        mars = GameObject.FindGameObjectWithTag("Mars");
        jupiter = GameObject.FindGameObjectWithTag("Jupiter");
        saturn = GameObject.FindGameObjectWithTag("Saturn");
        uranus = GameObject.FindGameObjectWithTag("Uranus");
        neptune = GameObject.FindGameObjectWithTag("Neptune");

        GameObject[] planets = new GameObject[] { mercury, venus, earth, mars, jupiter, saturn, uranus, neptune };

        for (int i=0; i<planets.Length; i++)
        {
            planets[i].transform.position = new Vector3(data[0, i], 0, 0);
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }

}
