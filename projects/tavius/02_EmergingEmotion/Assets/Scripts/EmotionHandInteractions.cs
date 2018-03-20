using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EmotionHandInteractions : MonoBehaviour {

    private GameObject left;
    private GameObject right;
    private GameObject Globals;

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
        Globals = GameObject.FindWithTag("globals");
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

		// Objects are grabbed before the trigger is fully clicked,
		// Pass the grabbed object up to the GlobalProperties:
		Globals.GetComponent<GlobalProperties>().isGrabbedEmotion = gameObject.name;
    }
<<<<<<< HEAD
}


// Two triggers can be triggered simultenaously
=======
}
>>>>>>> 4abde63831dc1325cbc1ab1abb361ea6f19497c4
