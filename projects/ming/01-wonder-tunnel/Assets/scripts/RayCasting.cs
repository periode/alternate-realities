using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCasting : MonoBehaviour
{
    Camera cam;
    public GameObject[] players;
    // Use this for initialization
    void Start()
    {
        cam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        Ray aRay = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(aRay, out hit))
        {
            if (hit.transform.name.Contains("Star")){
                hit.transform.GetComponent<MeshRenderer>().material.color = new Color(1, 1, 1);
                hit.transform.parent.GetComponentInParent<Star>().Fade();
                if (!hit.transform.GetComponent<AudioSource>().isPlaying)
                {
                    hit.transform.GetComponent<AudioSource>().Play();
                }
            }

        }
    }

}