using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EmotionHandInteractions : MonoBehaviour {

    private uint controllerIndex;

    // Use this for initialization
    void Start()
    {
        //make sure the object has the VRTK script attached... 
        if (GetComponent<VRTK_InteractableObject>() == null)
        {
            Debug.LogError("This object " + gameObject.name + " has the EmotionHandInteractions script but not the VRTK_InteractableObject script.");
            return;
        }

        // Listen for the "ObjectGrabbed" event.
        GetComponent<VRTK_InteractableObject>().InteractableObjectGrabbed += new InteractableObjectEventHandler(ObjectGrabbed);
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);
    }

    // This object has been grabbed.. so do what ever is in the code..

    private void ObjectGrabbed(object sender, InteractableObjectEventArgs e)

    {
        //controllerIndex = DoTriggerPressed(sender);
        //GameObject.Find("LeftController");
        Debug.Log(controllerIndex + " grabbed " + gameObject.GetComponent<EmotionProperties>().emotionType);
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        controllerIndex = VRTK_ControllerReference.GetRealIndex(e.controllerReference);
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        controllerIndex = 0;
    }

}

