using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastManager : MonoBehaviour
{

    public int rayDistance;
    public LayerMask layers;
    public float unlockTimer = 2.0f;
    public float viewTime = 0.0f;
    public GameObject inView = null;

    public int unlockedCount = 0;

    //public float fog = RenderSettings.fogDensity; // 0.0 to 1.0;
    public bool unlocked = false;
    public bool canHitRocks = false;
    public bool canHitGround = false;

    // Need layers 8, 9, 10
    //int layerUnlock = 8;
    //int layerRock = 9;
    //int layerGround = 10;
    //private LayerMask = 1 << layerUnlock;
    //public LayerMask canHit = 

    public string inViewName;

    void Start()
    {
        viewTime = Time.time;
        //fog = 0.6f;
    }

    void Update()
    {
        Cast();
    }

    void Cast()
    {
        Vector3 forward = transform.forward;
        RaycastHit hit;

        if (Physics.Raycast(transform.position, forward, out hit, rayDistance))
        {
            if (hit.collider.gameObject.tag == "Unlock")
            {
                // Check the timer if object is the same:
                if (inViewName == hit.collider.name)
                {
                    if (Time.time - viewTime > unlockTimer)
                    {
                        unlocked = true;
                        unlockedCount++;
                        canHitRocks = true;
                        rayDistance += 5;
                        //fog = 0.5f;
                        Debug.Log("Unlocked");
                    }
                }
            }

            if (hit.collider.gameObject.tag == "Rock" && canHitRocks)
            {
                // Check the timer if object is the same:
                if (inViewName == hit.collider.name)
                {
                    if (Time.time - viewTime > unlockTimer)
                    {
                        unlockedCount++;
                        rayDistance++;
                        Debug.Log(unlockedCount);
                    }
                    if (unlockedCount > 5)
                    {
                        canHitGround = true;
                        Debug.Log("Unlocked Ground");
                    }
                }
            }

            //if (hit.collider.gameObject.tag == "Ground")
            //{
            //    // Check the timer if object is the same:
            //    if (inViewName == hit.collider.name)
            //    {
            //        if (Time.time - viewTime > unlockTimer)
            //        {
            //            unlocked = true;
            //            unlockedCount++;
            //            canHitRocks = true;
            //        }
            //    }
            //}

            // Otherwise reset the variables:
            else
            {
                viewTime = Time.time;
                inView = hit.collider.gameObject;
                inViewName = hit.collider.name;
            }

            Debug.Log(Time.time - viewTime);
        }
    }
}
