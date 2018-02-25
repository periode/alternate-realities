#pragma strict

var walkSpeed : float = 7; //Regular speed
var crouchSpeed : float = 3; //Speed while crouching
var sprintSpeed : float = 13; // Speed while sprinting

private var charMotor : CharacterMotor;
private var charController : CharacterController;
private var theTransform : Transform;
private var charHeight : float; //Initial height

function Start () 
{
	charMotor = GetComponent(CharacterMotor);
	theTransform = transform;
	charController = GetComponent(CharacterController);
	charHeight = charController.height;
}

function Update () 
{
	var h = charHeight;
	var speed = walkSpeed;
	
	if (charController.isGrounded && Input.GetKey("left shift") || Input.GetKey("right shift"))
	{
		speed = sprintSpeed;
	}
	
	if (Input.GetKey("c"))
	{
		h = charHeight*0.5;
		speed = crouchSpeed;
	}
	
	charMotor.movement.maxForwardSpeed = speed; // Setting the max speed
	var lastHeight = charController.height; //Stand up/crouch smoothly
	charController.height = Mathf.Lerp(charController.height, h, 5*Time.deltaTime);
	theTransform.position.y += (charController.height-lastHeight)/2; //Fix vertical position
}