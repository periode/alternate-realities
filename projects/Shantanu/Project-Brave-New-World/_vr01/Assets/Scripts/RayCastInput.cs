using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RayCastInput : MonoBehaviour {

    Camera cam;
    public ExplosionController exp;
    bool exploded;
    public int explosionCount;
    public float palaceExplosionDelay;
    GameObject LightGO;
    public bool stopSpawning;

	void Start () {
        exploded = false;
        stopSpawning = false;
        cam = Camera.main;
        
	}
	

	void Update () {
        RaycastHit hit;
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if(Physics.Raycast(ray, out hit)) {
            if("palace" == hit.transform.name) {
                stopSpawning = true;
                GameObject[] fairies = GameObject.FindGameObjectsWithTag("fairy");
                for (int i = 0; i < fairies.Length; i++) {
                    fairies[i].GetComponent<FairyController>().triggerFairy();
                }
                if (!exploded) {
                    StartCoroutine(ExplosionsAfterDelay(0.2f, hit));
                    
                }
                exploded = true;
                //Debug.Log("boom boom brrrrr boom!");                   
            }
        }
    }

    // Coroutine that waits for a certain amount of time and then makes explosion effects + destruction of the palace happen
    IEnumerator ExplosionsAfterDelay(float delayTime, RaycastHit hit) {
        yield return new WaitForSeconds(delayTime);

        // Turn off the lights so that the explosion looks more spectacular
        LightGO = GameObject.Find("DirLight");
        Light mainLight = LightGO.GetComponent<Light>();
        mainLight.intensity = 0f;
        Destroy(hit.transform.gameObject, palaceExplosionDelay);
        for (int i = 0; i < explosionCount; i++) {
            //pick a location for your explosion
            Vector3 explosionPos = hit.transform.GetComponent<PalaceController>().GetPositionInsideObject();

            // Instantiate an explosion there and give it a clean name
            ExplosionController new_exp = (ExplosionController)Instantiate(exp, explosionPos, Quaternion.identity);
            new_exp.transform.name = "explosion_" + i;

            // Make it go boom
            new_exp.Explode();
        }
        

    }
}
