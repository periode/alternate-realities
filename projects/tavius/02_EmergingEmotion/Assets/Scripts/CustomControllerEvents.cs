using UnityEngine;
using VRTK;

public class CustomControllerEvents : MonoBehaviour
{
    public bool triggered;
    private GameObject Globals;
    public GameObject Other;
    public string holding = null;

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

        holding = Globals.GetComponent<GlobalProperties>().triggerEmotionName;
    }

    private void DoTriggerUnclicked(object sender, ControllerInteractionEventArgs e)
    {
        //triggered = false;

        // If this controller just let go of something:
        if (!holding.Equals(null))
        {

            // If the other controller is also holding something:
            if (!Other.GetComponent<CustomControllerEvents>().holding.Equals(null))
            {
                // If the other is not holding the same InstanceID:
                string otherHold = Other.GetComponent<CustomControllerEvents>().holding;
                if (!otherHold.Equals(holding))
                {
                    // Set the global variables to that emotion instead:
                    Globals.GetComponent<GlobalProperties>().triggerEmotionName = otherHold;
                }
                else
                {
                    Debug.Log("Global emotion should stay the same");
                }
                holding = null;
            }
            // If the other controller is empty, reset everything:
            else
            {

                triggered = false;

                // Find the specific emotion by reverse-searching for the name-as-ID,
                // This is set in Start() of the EmotionHandInteractions script.
                // If it is set as a trigger, reset the local and global:
                string test = holding.ToString();
                Debug.Log("Held name: " + test);
                GameObject toReset = GameObject.Find(holding.ToString());
                if (toReset.GetComponent<SphereCollider>().isTrigger)
                {
                    // Reset the object's variable along with the global boolean:
                    toReset.GetComponent<SphereCollider>().isTrigger = false;
                    Globals.GetComponent<GlobalProperties>().isTriggerEmotionGlobal = false;
                    Globals.GetComponent<GlobalProperties>().triggerEmotionName = null;
                }
                holding = null;
            }
        }
    }
}

// ISSUE: Not untriggered on drop
// Holding isn't being reset to 0 for the controllers
// Any time the trigger is released the Holding needs to be set to 0