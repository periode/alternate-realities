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
	Destroy (gameObject);
}