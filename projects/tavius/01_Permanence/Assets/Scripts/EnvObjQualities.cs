using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvObjQualities : MonoBehaviour
{
    public float alpha;
    public bool seen = false;

    void Start () {
        this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
    }

    void Update()
    {
        if (seen)
        {
            this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
            this.tag = "Non-Interactable";
        }
    }
}