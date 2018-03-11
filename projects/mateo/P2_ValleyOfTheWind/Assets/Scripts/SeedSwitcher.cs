using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using VRTK;

public class SeedSwitcher : MonoBehaviour {

    public GameObject[] seeds;
    int seedIndex = 0;

    SeedShooter shooter;
    Image seedImg;
    VRTK_ControllerEvents controller;
    bool bEventAttached = false;

	// Use this for initialization
	void Start () {
        shooter = GetComponent<SeedShooter>();
        controller = GetComponent<VRTK_ControllerEvents>();

        if (!shooter) {
            Debug.LogError("No SeedShooter found! Please attach a SeedShooter script.");
        }
        //else {
        //    controller = shooter.Controller; // .TriggerClicked += new ControllerInteractionEventHandler(OnGripPress);
        //}

        Image[] imgs = GetComponentsInChildren<Image>();
        seedImg = imgs[imgs.Length - 1];
        if (!seedImg) {
            Debug.LogError("Couldn't find Seed UI Image!");
        }

        SetSeed();
        bEventAttached = BindEvent();
	}
	
	// Update is called once per frame
	void Update () {
		if (!bEventAttached) {
            bEventAttached = BindEvent();
        }
	}

    bool BindEvent() {
        controller = shooter.Controller;

        if (!controller) {
            Debug.LogWarning("Binding Failed, retrying...");
            return false;
        }

        Debug.Log("YAy bound!");
        controller.TouchpadPressed += new ControllerInteractionEventHandler(OnTouchpadPress);

        return true;
    }

    void SetSeed() {
        Debug.Log("Switching seeds...");

        shooter.seed = seeds[seedIndex];
        seedIndex = (++seedIndex) % seeds.Length;

        if (seedImg) {
            seedImg.sprite = shooter.seed.GetComponent<SeedBehavior>().seedSprite;
        }
    }

    void OnTouchpadPress(object sender, ControllerInteractionEventArgs e) {
        Debug.Log("Grip Press");
        SetSeed();
    }
}
