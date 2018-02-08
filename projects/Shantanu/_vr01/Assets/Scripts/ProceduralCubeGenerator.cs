using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralCubeGenerator : MonoBehaviour {

    public GameObject myCube;
    public int cubeFieldSize;
    // Use this for initialization
    void Start() {
        for (int i = 0; i < cubeFieldSize; i++) {
            Vector3 pos = new Vector3(Random.Range(0, 100), Random.Range(0, 100));
            GameObject.Instantiate(myCube, pos, Quaternion.identity);
        }
    }


    void Update() {
       //nothing 
    }
}