using UnityEngine;
using VRTK;

public class CustomControllerEvents : MonoBehaviour
{
    public bool triggered;
    private GameObject Globals;
    private GameObject Other;
	private GameObject Held;
	private GameObject OtherHeld;
    public string heldName = "zero";

    private void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

        // Set the other controller
        if (gameObject.CompareTag("left"))
        {
            Other = GameObject.FindWithTag("right");
        }
        else
        {
            Other = GameObject.FindWithTag("left");
        }

        // Find the PlayAreaScripts GameObject to edit the global values:
        Globals = GameObject.FindWithTag("globals");

        //Setup controller event listeners for full trigger clicks:
        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
        GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);
    }

    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
        triggered = true;

		// On full trigger click, the current grabbed emotion will be set:
		heldName = Globals.GetComponent<GlobalProperties>().isGrabbedEmotion;
		Held = GameObject.Find(heldName);
		// If no emotion is already a trigger, set this one:
		if (!Globals.GetComponent<GlobalProperties>().isTriggerEmotionGlobal)
		{
			Held.GetComponent<SphereCollider>().isTrigger = true;
			Globals.GetComponent<GlobalProperties>().isTriggerEmotionGlobal = true;
		}

    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        triggered = false;

        // If this controller was holding something on unclick:
        if (!heldName.Equals("zero"))
        {
			// And the other controller is empty, reset everything:
			if (Other.GetComponent<CustomControllerEvents>().heldName.Equals("zero") || Other.GetComponent<CustomControllerEvents>().heldName == "")
			{

				// Find the specific emotion by reverse-searching for the name-as-ID,
				// This is set in Start() of the EmotionHandInteractions script.
				// If it is set as a trigger, reset the local and global:
				//if (Held.GetComponent<SphereCollider>().isTrigger) {
					// Reset the object's variable along with the global boolean:

					Held.GetComponent<SphereCollider>().isTrigger = false;
					Globals.GetComponent<GlobalProperties>().isTriggerEmotionGlobal = false;
					Globals.GetComponent<GlobalProperties>().isGrabbedEmotion = "zero";
				//}
				heldName = "zero";
			}
			// Otherwise the other controller is also holding something:
			else
            {
				// If the other is not holding the same InstanceID:
				string otherHeldName = Other.GetComponent<CustomControllerEvents>().heldName;
				if (!otherHeldName.Equals(heldName))
				{
					// Set the global variables to that emotion instead:
					Held.GetComponent<SphereCollider>().isTrigger = false;
					Globals.GetComponent<GlobalProperties>().isGrabbedEmotion = otherHeldName;
					// Add trigger to other emotion
					OtherHeld = GameObject.Find(otherHeldName);
					OtherHeld.GetComponent<SphereCollider>().isTrigger = true;
				}
				heldName = "zero";
            }
       }
    }
}
