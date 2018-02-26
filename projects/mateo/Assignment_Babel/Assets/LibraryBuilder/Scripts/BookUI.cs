using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class BookUI : MonoBehaviour {
    VRTK_InteractableObject interactions;
    GameObject frontCanvas;
    GameObject backCanvas;
    GameObject spineCanvas;
    GameObject spineCanvas2;
    Rigidbody rb;

    BookContents cont;

    bool wasGrabbed = false;
    bool bError = false;

    // Use this for initialization
    void Start() {
        cont = GetComponent<BookContents>();

        interactions = GetComponent<VRTK_InteractableObject>();

        frontCanvas = transform.Find("CanvasFront").gameObject;
        backCanvas = transform.Find("CanvasBack").gameObject;
        spineCanvas = transform.Find("CanvasSpine").gameObject;
        spineCanvas2 = transform.Find("CanvasSpine2").gameObject;

        if (frontCanvas) {
            frontCanvas.SetActive(false);
        }
        else {
            bError = true;
        }

        if (backCanvas) {
            backCanvas.SetActive(false);
        }
        else {
            bError = true;
        }

        if (spineCanvas) {
            spineCanvas.SetActive(false);
        }
        else {
            bError = true;
        }

        if (spineCanvas2) {
            spineCanvas2.SetActive(false);
        }
        else {
            bError = true;
        }

        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update() {
        bool bGrabbed = interactions.IsGrabbed();
        bool bTouched = interactions.IsTouched();

        if (wasGrabbed && !bGrabbed) {
            rb.isKinematic = false;
        }

        if (bGrabbed || bTouched) {
            SetLabels();
        }

        spineCanvas.SetActive(bTouched);
        spineCanvas2.SetActive(bTouched);

        frontCanvas.SetActive(bGrabbed);
        backCanvas.SetActive(bGrabbed);

        wasGrabbed = bGrabbed;
    }

    void SetLabels() {
        if (bError) {
            return;
        }

        Text txt;

        txt = frontCanvas.transform.Find("Name").GetComponent<Text>();
        txt.text = cont.bookTitle;

        txt = backCanvas.transform.Find("Name").GetComponent<Text>();
        txt.text = cont.bookTitle;

        txt = spineCanvas.transform.Find("Name").GetComponent<Text>();
        txt.text = cont.bookTitle;

        txt = spineCanvas2.transform.Find("Name").GetComponent<Text>();
        txt.text = cont.bookTitle;
    }
}
