//IMPORTANT NOTE! THIS SCRIPT WAS MADE IN VIDEO NUMBER 21! Check out PlayerStats to use the old one.

#pragma strict

var MaxHealth = 100;
var Health : int;

function Start ()
{
	Health = MaxHealth;
}

function ApplyDammage (TheDammage : int)
{
	Health -= TheDammage;
	
	if(Health <= 0)
	{
		Dead();
	}
}

function Dead()
{
	RespawnMenuV2.playerIsDead = true; //VERY IMPORTANT! This line was added in tutorial number 19. If you haven't reached that tutorial yet, go ahead and remove it.
	Debug.Log("Player Died");
}

function RespawnStats ()
{
	Health = MaxHealth;
}