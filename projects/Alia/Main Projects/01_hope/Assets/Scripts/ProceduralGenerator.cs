using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour {

    
    public GameObject plane;
    public GameObject tunnel;
    public GameObject pipe;

    GameObject[] planes;
    public GameObject endlight;

    public int numPlanes = 50;
	// Use this for initialization
	void Start () {
        planes  = new GameObject[numPlanes];
         float k = 20;

      
             for (int i = 0; i < numPlanes; i++)
             {
                 planes[i] = GameObject.Instantiate(plane, new Vector3(k, -4f, 1.3f), Quaternion.identity);
                 k += 28;
             }

    }
  
    

	// Update is called once per frame
	void  Update () {

       
    }

    private int x = 1;
    public float newx = -12;
    public float altnewx;
    public void triggered(Collider other, float thisx)
    {

        float rvalue = Mathf.Abs(x * 0.04f);

        float gvalue = Mathf.Abs(x * 0.02f);
        float bvalue = Mathf.Abs(x * 0.01f);

        pipe.GetComponent<MeshRenderer>().material.color = new Color(rvalue, gvalue, bvalue, 1);

        Debug.Log("r: " + rvalue + "g" + gvalue + "b" + bvalue);

        Debug.Log("x: " + x + ", thisx:" + thisx + ", newx: " + newx);

        if ((thisx + 10) > (newx))
        {
            if (x == 1)
            {
                newx = thisx + 55f;
            }

            else
            {
                int xminus = x - 1;
                newx = thisx + (45f * xminus); //50.79.. 89.346
            }



            for (int j = 0; j < x; j++)
            {
                GameObject.Instantiate(tunnel, new Vector3(newx + (45 * j), -10.5f, 12.844f), Quaternion.identity);

                

                Debug.Log("instantiated j=" + j + " newx: " + newx);
            }

            x++;
                        
            
        }

        altnewx = newx + 30*x;

        //else
        //{

        //    altnewx = thisx + 45f * x;
        //    Debug.Log("skipped collider");
            

        //}

    }



}
