#pragma strict

var Weapon01 : GameObject;
var Weapon02 : GameObject;

function Update () {
	if (Input.GetKeyDown(KeyCode.Q))
	{
		SwapWeapons();
	}
}

function SwapWeapons()
{
	if (Weapon01.active == true)
	{
		Weapon01.SetActiveRecursively(false);
		Weapon02.SetActiveRecursively(true);
	}
	else 
	{
		Weapon01.SetActiveRecursively(true);
		Weapon02.SetActiveRecursively(false);
	}
}