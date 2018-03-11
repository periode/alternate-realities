using UnityEngine;
using VRTK;

public class CustomControllerEvents : MonoBehaviour
{
<<<<<<< HEAD
=======
    public bool triggered;
    private GameObject Globals;
    private GameObject Other;
	private GameObject Held;
	private GameObject OtherHeld;
    public string heldName = "zero";

>>>>>>> f2da5c8646aa4cf664d9f4456a97a6925e8b340c
    private void Start()
    {
        if (GetComponent<VRTK_ControllerEvents>() == null)
        {
            VRTK_Logger.Error(VRTK_Logger.GetCommonMessage(VRTK_Logger.CommonMessageKeys.REQUIRED_COMPONENT_MISSING_FROM_GAMEOBJECT, "VRTK_ControllerEvents_ListenerExample", "VRTK_ControllerEvents", "the same"));
            return;
        }

<<<<<<< HEAD
        // On object grab check for object with emotion tag

        //Setup controller event listeners
        GetComponent<VRTK_ControllerEvents>().TriggerPressed += new ControllerInteractionEventHandler(DoTriggerPressed);
        GetComponent<VRTK_ControllerEvents>().TriggerReleased += new ControllerInteractionEventHandler(DoTriggerReleased);

        GetComponent<VRTK_ControllerEvents>().TriggerTouchStart += new ControllerInteractionEventHandler(DoTriggerTouchStart);
        GetComponent<VRTK_ControllerEvents>().TriggerTouchEnd += new ControllerInteractionEventHandler(DoTriggerTouchEnd);

        GetComponent<VRTK_ControllerEvents>().TriggerHairlineStart += new ControllerInteractionEventHandler(DoTriggerHairlineStart);
        GetComponent<VRTK_ControllerEvents>().TriggerHairlineEnd += new ControllerInteractionEventHandler(DoTriggerHairlineEnd);

        GetComponent<VRTK_ControllerEvents>().TriggerClicked += new ControllerInteractionEventHandler(DoTriggerClicked);
        GetComponent<VRTK_ControllerEvents>().TriggerUnclicked += new ControllerInteractionEventHandler(DoTriggerUnclicked);

        GetComponent<VRTK_ControllerEvents>().TriggerAxisChanged += new ControllerInteractionEventHandler(DoTriggerAxisChanged);

        GetComponent<VRTK_ControllerEvents>().GripPressed += new ControllerInteractionEventHandler(DoGripPressed);
        GetComponent<VRTK_ControllerEvents>().GripReleased += new ControllerInteractionEventHandler(DoGripReleased);

        GetComponent<VRTK_ControllerEvents>().GripTouchStart += new ControllerInteractionEventHandler(DoGripTouchStart);
        GetComponent<VRTK_ControllerEvents>().GripTouchEnd += new ControllerInteractionEventHandler(DoGripTouchEnd);

        GetComponent<VRTK_ControllerEvents>().GripHairlineStart += new ControllerInteractionEventHandler(DoGripHairlineStart);
        GetComponent<VRTK_ControllerEvents>().GripHairlineEnd += new ControllerInteractionEventHandler(DoGripHairlineEnd);

        GetComponent<VRTK_ControllerEvents>().GripClicked += new ControllerInteractionEventHandler(DoGripClicked);
        GetComponent<VRTK_ControllerEvents>().GripUnclicked += new ControllerInteractionEventHandler(DoGripUnclicked);

        GetComponent<VRTK_ControllerEvents>().GripAxisChanged += new ControllerInteractionEventHandler(DoGripAxisChanged);

        GetComponent<VRTK_ControllerEvents>().TouchpadPressed += new ControllerInteractionEventHandler(DoTouchpadPressed);
        GetComponent<VRTK_ControllerEvents>().TouchpadReleased += new ControllerInteractionEventHandler(DoTouchpadReleased);

        GetComponent<VRTK_ControllerEvents>().TouchpadTouchStart += new ControllerInteractionEventHandler(DoTouchpadTouchStart);
        GetComponent<VRTK_ControllerEvents>().TouchpadTouchEnd += new ControllerInteractionEventHandler(DoTouchpadTouchEnd);

        GetComponent<VRTK_ControllerEvents>().TouchpadAxisChanged += new ControllerInteractionEventHandler(DoTouchpadAxisChanged);

        GetComponent<VRTK_ControllerEvents>().ButtonOnePressed += new ControllerInteractionEventHandler(DoButtonOnePressed);
        GetComponent<VRTK_ControllerEvents>().ButtonOneReleased += new ControllerInteractionEventHandler(DoButtonOneReleased);

        GetComponent<VRTK_ControllerEvents>().ButtonOneTouchStart += new ControllerInteractionEventHandler(DoButtonOneTouchStart);
        GetComponent<VRTK_ControllerEvents>().ButtonOneTouchEnd += new ControllerInteractionEventHandler(DoButtonOneTouchEnd);

        GetComponent<VRTK_ControllerEvents>().ButtonTwoPressed += new ControllerInteractionEventHandler(DoButtonTwoPressed);
        GetComponent<VRTK_ControllerEvents>().ButtonTwoReleased += new ControllerInteractionEventHandler(DoButtonTwoReleased);

        GetComponent<VRTK_ControllerEvents>().ButtonTwoTouchStart += new ControllerInteractionEventHandler(DoButtonTwoTouchStart);
        GetComponent<VRTK_ControllerEvents>().ButtonTwoTouchEnd += new ControllerInteractionEventHandler(DoButtonTwoTouchEnd);

        GetComponent<VRTK_ControllerEvents>().StartMenuPressed += new ControllerInteractionEventHandler(DoStartMenuPressed);
        GetComponent<VRTK_ControllerEvents>().StartMenuReleased += new ControllerInteractionEventHandler(DoStartMenuReleased);

        GetComponent<VRTK_ControllerEvents>().ControllerEnabled += new ControllerInteractionEventHandler(DoControllerEnabled);
        GetComponent<VRTK_ControllerEvents>().ControllerDisabled += new ControllerInteractionEventHandler(DoControllerDisabled);

        GetComponent<VRTK_ControllerEvents>().ControllerIndexChanged += new ControllerInteractionEventHandler(DoControllerIndexChanged);
    }

    private void DebugLogger(uint index, string button, string action, ControllerInteractionEventArgs e)
    {
        VRTK_Logger.Info("Controller on index '" + index + "' " + button + " has been " + action
                + " with a pressure of " + e.buttonPressure + " / trackpad axis at: " + e.touchpadAxis + " (" + e.touchpadAngle + " degrees)");
    }

    private void DoTriggerPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "pressed", e);
    }

    private void DoTriggerReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "released", e);
    }

    private void DoTriggerTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "touched", e);
    }

    private void DoTriggerTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "untouched", e);
    }

    private void DoTriggerHairlineStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "hairline start", e);
    }

    private void DoTriggerHairlineEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "hairline end", e);
=======
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
>>>>>>> f2da5c8646aa4cf664d9f4456a97a6925e8b340c
    }

    private void DoTriggerClicked(object sender, ControllerInteractionEventArgs e)
    {
<<<<<<< HEAD
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "clicked", e);
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "unclicked", e);
    }

    private void DoTriggerAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TRIGGER", "axis changed", e);
    }

    private void DoGripPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "pressed", e);
    }

    private void DoGripReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "released", e);
    }

    private void DoGripTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "touched", e);
    }

    private void DoGripTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "untouched", e);
    }

    private void DoGripHairlineStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "hairline start", e);
    }

    private void DoGripHairlineEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "hairline end", e);
    }

    private void DoGripClicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "clicked", e);
    }

    private void DoGripUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "unclicked", e);
    }

    private void DoGripAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "GRIP", "axis changed", e);
    }

    private void DoTouchpadPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "pressed down", e);
    }

    private void DoTouchpadReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "released", e);
    }

    private void DoTouchpadTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "touched", e);
    }

    private void DoTouchpadTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "untouched", e);
    }

    private void DoTouchpadAxisChanged(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "TOUCHPAD", "axis changed", e);
    }

    private void DoButtonOnePressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON ONE", "pressed down", e);
    }

    private void DoButtonOneReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON ONE", "released", e);
    }

    private void DoButtonOneTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON ONE", "touched", e);
    }

    private void DoButtonOneTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON ONE", "untouched", e);
    }

    private void DoButtonTwoPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON TWO", "pressed down", e);
    }

    private void DoButtonTwoReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON TWO", "released", e);
    }

    private void DoButtonTwoTouchStart(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON TWO", "touched", e);
    }

    private void DoButtonTwoTouchEnd(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "BUTTON TWO", "untouched", e);
    }

    private void DoStartMenuPressed(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "START MENU", "pressed down", e);
    }

    private void DoStartMenuReleased(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "START MENU", "released", e);
    }

    private void DoControllerEnabled(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "CONTROLLER STATE", "ENABLED", e);
    }

    private void DoControllerDisabled(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "CONTROLLER STATE", "DISABLED", e);
    }

    private void DoControllerIndexChanged(object sender, ControllerInteractionEventArgs e)
    {
        DebugLogger(VRTK_ControllerReference.GetRealIndex(e.controllerReference), "CONTROLLER STATE", "INDEX CHANGED", e);
=======
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
>>>>>>> f2da5c8646aa4cf664d9f4456a97a6925e8b340c
    }
}