using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Create_Primitives : MonoBehaviour {

    bool gravityUp = true;
    AudioSource upSound, downSound;

    float lightIntensity = 0.85f;

    Vector3 upGravity = new Vector3(0, 1.5f, 0);
    Vector3 downGravity = new Vector3(0, -4f, 0);

    public GameObject upGravityIndicator;
    public GameObject downGravityIndicator;

    public float sleepTime = 5;
    float addedSleep = 0;

    public GameObject sphere;
    public GameObject sphereHolder;
    List<GameObject> spheres;

    int counter = 0;
    public int maxCounter = 10;
    public float max_Range = 2f;
    public float min_Range = 10f;

    Vector3 newSpawnLocation;
	// Use this for initialization
	void Start () {
        spheres = new List<GameObject>();
        StartCoroutine(startSpawning());
        upSound = GameObject.Find("UpSound").GetComponent<AudioSource>();
        downSound = GameObject.Find("DownSound").GetComponent<AudioSource>();
    }

    IEnumerator startSpawning() {
        yield return new WaitForSeconds(3f);
        while (counter++ < maxCounter) {
            newSpawnLocation = new Vector3(Random.insideUnitCircle.x, 0, Random.insideUnitCircle.y) * max_Range;
            yield return new WaitForSeconds(sleepTime + (addedSleep++ / 2));
            GameObject thisSphere = Instantiate(sphere, transform.position + newSpawnLocation, Quaternion.identity, sphereHolder.transform);
            spheres.Add(thisSphere);
            lightIntensity = lightIntensity - (lightIntensity / 20);

            foreach (GameObject sphere in spheres) {
                sphere.GetComponentInChildren<Light>().intensity = lightIntensity;
            }
        }
    }

    IEnumerator ignoreGravity(GameObject sphere) {
            sphere.GetComponent<Rigidbody>().useGravity = false;
            yield return new WaitForSeconds(Random.Range(0f,2f));
            sphere.GetComponent<Rigidbody>().useGravity = true;
    }

    // Update is called once per frame
    void Update () {
        if (Cardboard.SDK.Triggered) {
            gravityUp = !gravityUp;
            Physics.gravity = gravityUp ? upGravity : downGravity;
            upGravityIndicator.SetActive(gravityUp);
            downGravityIndicator.SetActive(!gravityUp);
            upSound.mute = !gravityUp;
            downSound.mute = gravityUp;
            List<GameObject> thisSpheres = new List<GameObject>(spheres);
            foreach (GameObject sphere in thisSpheres) {
                StartCoroutine(ignoreGravity(sphere));
            }
        }

        Touch myTouch = Input.GetTouch(0);

        Touch[] myTouches = Input.touches;
        if(Input.touchCount > 1) {
            SceneManager.LoadScene(0);
        }
    
    }
}
