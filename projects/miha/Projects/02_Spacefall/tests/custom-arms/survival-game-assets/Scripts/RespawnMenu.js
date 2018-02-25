//IMPORTANT NOTE! If you are using this script without having watched video number 20 then you must go ahead and remove the "//" from the script.
//The "//" signs were added because the sprintScript was deleted and instead we use the FPSWalkerEnhanced.
//Also note that the variable charMotor was changed to charController because it will then work with both the old and new setup.
//If any of this confuses you, watch tutorial number 20 :)

#pragma strict

var lookAround01 : MouseLook;
var lookAround02 : MouseLook;
var charController : CharacterController;
//var sprintScript : SprintAndCrouch;

static var playerIsDead = false;

function Start () 
{
	lookAround01 = gameObject.GetComponent(MouseLook);
	lookAround02 = GameObject.Find("MainCamera").GetComponent(MouseLook);
	charController = gameObject.GetComponent(CharacterController);
	//sprintScript = gameObject.GetComponent(SprintAndCrouch);
}

function Update ()
{
	if (playerIsDead == true)
	{
		lookAround01.enabled = false;
		lookAround02.enabled = false;
		//sprintScript.enabled = false;
		charController.enabled = false;
	}
}

function OnGUI ()
{
	if (playerIsDead == true)
	{
		if (GUI.Button(Rect(Screen.width*0.5-50, 200-20, 100, 40), "Respawn"))
		{
			RespawnPlayer();
		}
		
		if (GUI.Button(Rect(Screen.width*0.5-50, 240, 100, 40), "Menu"))
		{
			Debug.Log("Return to Menu");
		}
	}
}

function RespawnPlayer ()
{
	Debug.Log("Respawn Player");
}

@script RequireComponent(CharacterController)