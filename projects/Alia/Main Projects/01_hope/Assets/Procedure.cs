using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Procedure : MonoBehaviour {


    public LayerMask layerMask;
    public float raycastDistance;
    public GameObject plane;
    public GameObject tree;
    public GameObject apple;

    GameObject[] all_plane;
    GameObject[] all_trees;
    GameObject[] all_apples;

    public int numPlanes = 10000;

    public int numTrees = 1000;

    public int numRow = 1000;

    public int numApples = 10000;

    

    // Use this for initialization
    void Start () {
        all_plane = new GameObject[numPlanes];
        all_trees = new GameObject[numTrees];

        double height = 100;
        int k = 0;
        int j = 0;

        for (int t=0; t<numTrees; t++)
        {
            int treex = Random.Range(-500,500);
            int treez = Random.Range(15, 95);

            all_trees[j] = GameObject.Instantiate(tree, new Vector3(treex, 0f, treez), Quaternion.identity);

        }



        for (int x = 0; x < numRow; x+=5)
                {

                for (int z = 0; z < height; z+=5)
                        {

               
                    all_plane[k] = GameObject.Instantiate(plane, new Vector3(x, 0f, z), Quaternion.identity);
                    k++;

                    all_plane[k] = GameObject.Instantiate(plane, new Vector3(-x, 0f, z), Quaternion.identity);
                    k++;

                       }
                    }
                
                      

        }


    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("space")) 
            SceneManager.LoadScene("project1");

    }
    }



//


   
 