using UnityEngine;
using VRTK;

public class onTouchRevealBookTitle : MonoBehaviour {
	public string bookTitle;
	public VRTK_ControllerEvents controllerEvents;
	public VRTK_InteractTouch touchEvents;
	public GameObject BookTitleFollower;
	// Use this for initialization
	bool titleWindowUp = true;


	void OnEnable(){

		controllerEvents.TriggerReleased += ControllerEvents_TriggerReleased;

	}


	void OnDisable(){

		controllerEvents.TriggerReleased -= ControllerEvents_TriggerReleased;

	}

	void ControllerEvents_TriggerReleased (object sender, ControllerInteractionEventArgs e)
	{
		titleWindowUp = !titleWindowUp;
		BookTitleFollower.SetActive (titleWindowUp);
	}

}
