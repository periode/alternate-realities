#pragma strict

var TheDammage : int = 50;
private var Distance : float;
var MaxDistance : float = 1.5;
var TheAnimator : Animator;
var DammageDelay : float = 0.6;

private var Hit01Streak = 0;
private var Hit02Streak = 0;

function Update ()
{
	if (Input.GetButtonDown("Fire1"))
	{
		AttackDammage();
	}
}

function AttackDammage ()
{
	if (Random.value >= 0.5 && Hit01Streak <= 2)
	{
		TheAnimator.SetBool("Hit01", true);
		Hit01Streak += 1;
		Hit02Streak = 0;
	}
	else
	{
		if (Hit02Streak <= 2)
		{
			TheAnimator.SetBool("Hit02", true);
			Hit01Streak = 0;
			Hit02Streak += 1;
		}
		else
		{
			TheAnimator.SetBool("Hit01", true);
			Hit01Streak += 1;
			Hit02Streak = 0;
		}
	}

	yield WaitForSeconds(DammageDelay);
	//Actual attacking
	var hit : RaycastHit;
	var ray = Camera.main.ScreenPointToRay(Vector3(Screen.width/2, Screen.height/2, 0));
	if (Physics.Raycast (ray, hit))
	{
		Distance = hit.distance;
		if (Distance < MaxDistance)
		{
			hit.transform.SendMessage("ApplyDammage", TheDammage, SendMessageOptions.DontRequireReceiver);
		}
	}
	
	TheAnimator.SetBool("Hit01", false);
	TheAnimator.SetBool("Hit02", false);
}