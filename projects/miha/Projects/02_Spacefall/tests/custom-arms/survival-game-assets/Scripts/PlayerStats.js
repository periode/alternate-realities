#pragma strict

var Health = 100;

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
	RespawnMenu.playerIsDead = true; //VERY IMPORTANT! This line was added in tutorial number 19. If you haven't reached that tutorial yet, go ahead and remove it.
	Debug.Log("Player Died");
}