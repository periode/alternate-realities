using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EmotionHandInteractions : MonoBehaviour {

    private GameObject left;
    private GameObject right;
    private GameObject Globals;
    private bool isTriggerEmotionGlobal;

    // Use this for initialization
    private void Start()
    {
        // Make sure the object has the VRTK script attached: 
        if (GetComponent<VRTK_InteractableObject>() == null)
        {
            Debug.LogError("This object " + gameObject.name + " has the EmotionHandInteractions script but not the VRTK_InteractableObject script.");
            return;
        }

        // Set the InstanceID as the GameObject's name to be accessed later:
        gameObject.name = GetInstanceID().ToString();

        // Listen for the ObjectGrabbed/Ungrabbed events:
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectUngrabbed);
        Globals = GameObject.FindWithTag("globals");
    }

    private void Update()
    {
        // Check the global space for a triggered emotion:
        isTriggerEmotionGlobal = Globals.GetComponent<GlobalProperties>().isTriggerEmotionGlobal;
    }

    // When this object is grabbed:
    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)
    {
        // Assign the controllers if needed:
        if (left == null)
        {
            left = GameObject.FindWithTag("left");
        }
        if (right == null)
        {
            right = GameObject.FindWithTag("right");
        }

        // Run if no emotions are currently a trigger, set a new one:
        if (!isTriggerEmotionGlobal)
        {
            // Set the object as a trigger, and mark its ID in the global space:

            // Set the objects trigger:
            gameObject.GetComponent<SphereCollider>().isTrigger = true;
            // Set the global trigger:
            Globals.GetComponent<GlobalProperties>().isTriggerEmotionGlobal = true;
            // Set the global ID:
            Globals.GetComponent<GlobalProperties>().triggerEmotionName = gameObject.name;
        }
    }

    private void ObjectUngrabbed(object sender, InteractableObjectEventArgs e)
    {

    }
}

// ISSUE: the Grabbed function triggers AFTER Unclicked, while releasing

