#pragma strict

var currentWeapon = 0;
var maxWeapons = 2;
var theAnimator : Animator;

function Awake () 
{
	SelectWeapon(0);
}

function Update () 
{
	if(Input.GetAxis("Mouse ScrollWheel") > 0)
	{
		if(currentWeapon + 1 <= maxWeapons)
		{
			currentWeapon++;
		}
		else
		{
			currentWeapon = 0;
		}
		SelectWeapon(currentWeapon);
	}
	else if (Input.GetAxis("Mouse ScrollWheel") < 0)
	{
		if(currentWeapon - 1 >= 0)
		{
			currentWeapon--;
		}
		else
		{
			currentWeapon = maxWeapons;
		}
		SelectWeapon(currentWeapon);
	}
	
	if(currentWeapon == maxWeapons + 1)
	{
		currentWeapon = 0;
	}
	if(currentWeapon == -1)
	{
		currentWeapon = maxWeapons;
	}
	
	if(Input.GetKeyDown(KeyCode.Alpha1))
	{
		currentWeapon = 0;
		SelectWeapon(currentWeapon);
	}
	if(Input.GetKeyDown(KeyCode.Alpha2) && maxWeapons >= 1)
	{
		currentWeapon = 1;
		SelectWeapon(currentWeapon);
	}
	if(Input.GetKeyDown(KeyCode.Alpha3) && maxWeapons >= 2)
	{
		currentWeapon = 2;
		SelectWeapon(currentWeapon);
	}

}

function SelectWeapon (index : int)
{
	for (var i = 0; i < transform.childCount; i++)
	{
		//Activate the selected weapon
		if (i == index)
		{
			if (transform.GetChild(i).name == "Fists")
			{
				theAnimator.SetBool("WeaponIsOn", false);
			}
			else
			{
				theAnimator.SetBool("WeaponIsOn", true);
			}
			transform.GetChild(i).gameObject.SetActive(true);
		}
		else
		{
			transform.GetChild(i).gameObject.SetActive(false);
		}
	}
}