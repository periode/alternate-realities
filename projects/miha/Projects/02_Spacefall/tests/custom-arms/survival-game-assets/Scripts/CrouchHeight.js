#pragma strict

private var charController : CharacterController;
private var theTransform : Transform;
private var charHeight : float; //Initial height

function Start () 
{
	theTransform = transform;
	charController = GetComponent(CharacterController);
	charHeight = charController.height;
}

function Update () 
{
	var h = charHeight;
	
	if (Input.GetKey("c"))
	{
		h = charHeight*0.5;
	}
	
	var lastHeight = charController.height; //Stand up/crouch smoothly
	charController.height = Mathf.Lerp(charController.height, h, 5*Time.deltaTime);
	theTransform.position.y += (charController.height-lastHeight)/2; //Fix vertical position
}