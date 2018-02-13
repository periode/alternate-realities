using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonBehavior : MonoBehaviour
{
    public int rayDistance = 500;
    skyboxBehavior s1;
    private float rayHitStart = 0f;    RaycastHit hit;

    public bool trackTimer = false;
    bool canChangeTimer = true;
    float timer = 2f;

    private void Start()
    {
        s1 = GameObject.FindObjectOfType<skyboxBehavior>();
    }

    private void raycasting(){

        Debug.Log("Reached myfunc");
        Vector3 fwd = transform.TransformDirection(Vector3.forward);
        Ray cameraRay = new Ray(transform.position, fwd);
        hit = new RaycastHit();

        if (Physics.Raycast(cameraRay, out hit, rayDistance))
        {
            Debug.Log("Reached myfunc1");
            Debug.Log(hit.collider.name);

            if (hit.collider.tag == "Sky")
            {
                //myTimer(Time.time);
                //canChangeTimer = true;
                StartCoroutine("ChangeSkybox");
                Debug.Log(hit.collider.name);
                Debug.Log("Reached myfunc2");
                //RenderSettings.skybox = s1.myMaterials[Random.Range(0, s1.myMaterials.Length)];
            }
            else{
                canChangeTimer = true;
            }
        }
    }

    void Update()
    {
        raycasting();

       
    }

    IEnumerator ChangeSkybox(){
        
        if(canChangeTimer == true){
            yield return new WaitForSeconds(3.0f);
            RenderSettings.skybox = s1.myMaterials[Random.Range(0, s1.myMaterials.Length)];
            canChangeTimer = false;
            yield return new WaitForSeconds(3.0f);
        }
    }
}