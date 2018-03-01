using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RaycastManager : MonoBehaviour {

    Camera cam;
    public LayerMask mask;

    public Text label;

    // Use this for initialization
    void Start() {
        cam = GetComponent<Camera>();
        //label = GameObject.Find("CanvasOverlay")
        //    .transform.Find("Label")
        //    .GetComponent<Text>();
        //if (!GameObject.Find("CanvasOverlay")
        //    .transform.Find("Label")) {
        //    Debug.LogWarning("wtf");
        //}
    }

    // Update is called once per frame
    void Update() {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, cam.transform.forward);
        bool bHit = Physics.Raycast(ray, out hit, 10f, mask);

        if (bHit) {
            PanelManager pm = hit.collider.GetComponent<PanelManager>();

            if (pm) {
                label.text = "Now examining Sector " + pm.hexagon;
                //Debug.Log(pm.hexagon);
            }
        }
        else {
            label.text = "Nothing to examine";
        }
    }
}
