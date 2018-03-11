//IMPORTANT NOTE: This script was updated last in tutorial number 6. If you haven't watched it yet, go do it before messaging me with errors :)

#pragma strict

var TheDammage : int = 50;
var Distance : float;
var MaxDistance : float = 1.5;
var TheSystem : Transform;

function Update ()
{
	if (Input.GetButtonDown("Fire1")) 
	{
		//Attack animation - IF YOU ARE GETTING ERRORS BECAUSE YOU HAVENT MADE AN ANIMATION AND ATTACHED IT, DELETE THE FOLOWING LINE.
		animation.Play("Attack");

	}
//	if (TheMace.animation.isPlaying == false)
//	{
//		TheMace.animation.CrossFade("Idle");
//	}
//	
//	if (Input.GetKey (KeyCode.LeftShift))
//	{
//		TheMace.animation.CrossFade("Sprint");
//	}
//	
//	if (Input.GetKeyUp(KeyCode.LeftShift))
//	{
//		TheMace.animation.CrossFade("Idle");
//	}
}

function AttackDammage ()
{
		//Attack function
		var hit : RaycastHit;
		if (Physics.Raycast (TheSystem.transform.position, TheSystem.transform.TransformDirection(Vector3.forward), hit))
		{
			Distance = hit.distance;
			if (Distance < MaxDistance)
			{
				hit.transform.SendMessage("ApplyDammage", TheDammage, SendMessageOptions.DontRequireReceiver);
			}
		}
}