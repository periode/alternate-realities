#pragma strict

var walkSpeed : float = 7; // Regular speed
var sprintSpeed : float = 13; // Run speed

private var charMotor : CharacterMotor;
private var charController : CharacterController;

function Start () 
{
	charMotor = GetComponent(CharacterMotor);
	charController = GetComponent(CharacterController);
}

function Update () 
{
	//Making the actual speed var
	var speed = walkSpeed;
	
	//Checking for oppertunity to sprint
	if ( charController.isGrounded && Input.GetKey("left shift") || Input.GetKey("right shift"))
	{
		//changing the speed to sprint
		speed = sprintSpeed;
	}
	
	charMotor.movement.maxForwardSpeed = speed; //Setting the speed
}