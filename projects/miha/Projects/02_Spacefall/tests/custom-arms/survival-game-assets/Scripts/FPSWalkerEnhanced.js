var walkSpeed = 6.0;
var runSpeed = 11.0;
var crouchSpeed = 3.0;
 
// If true, diagonal speed (when strafing + moving forward or back) can't exceed normal move speed; otherwise it's about 1.4 times faster
var limitDiagonalSpeed = true;
 
// If checked, the run key toggles between running and walking. Otherwise player runs if the key is held down and walks otherwise
// There must be a button set up in the Input Manager called "Run"

var enableRun = true;
var enableCrouch = true;
 
var jumpSpeed = 8.0;
var gravity = 20.0;
 
var enableFallingDamage = true;
// Units that player can fall before a falling damage function is run. To disable, type "infinity" in the inspector
var fallingDamageThreshold = 10.0;
var fallingDamageMultiplier = 2;
 
// If the player ends up on a slope which is at least the Slope Limit as set on the character controller, then he will slide down
var slideWhenOverSlopeLimit = false;
 
// If checked and the player is on an object tagged "Slide", he will slide down it regardless of the slope limit
var slideOnTaggedObjects = false;
 
var slideSpeed = 12.0;
 
// If checked, then the player can change direction while in the air
var airControl = false;
 
// Small amounts of this results in bumping when walking down slopes, but large amounts results in falling too fast
var antiBumpFactor = .75;
 
// Player must be grounded for at least this many physics frames before being able to jump again; set to 0 to allow bunny hopping 
var antiBunnyHopFactor = 1;
 
private var moveDirection = Vector3.zero;
private var grounded = false;
private var controller : CharacterController;
private var myTransform : Transform;
private var speed : float;
private var hit : RaycastHit;
private var fallStartLevel : float;
private var falling = false;
private var slideLimit : float;
private var rayDistance : float;
private var contactPoint : Vector3;
private var playerControl = false;
private var jumpTimer : int;

private var charHeight : float; //Initial height
 
function Start () {
	controller = GetComponent(CharacterController);
	myTransform = transform;
	speed = walkSpeed;
	rayDistance = controller.height * .5 + controller.radius;
	slideLimit = controller.slopeLimit - .1;
	jumpTimer = antiBunnyHopFactor;
	oldPos = transform.position;
}
 
function FixedUpdate() {
	var inputX = Input.GetAxis("Horizontal");
	var inputY = Input.GetAxis("Vertical");
	// If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
	var inputModifyFactor = (inputX != 0.0 && inputY != 0.0 && limitDiagonalSpeed)? .7071 : 1.0;
 
	if (grounded) {
		var sliding = false;
		// See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
		// because that interferes with step climbing amongst other annoyances
		if (Physics.Raycast(myTransform.position, -Vector3.up, hit, rayDistance)) {
			if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
				sliding = true;
		}
		// However, just raycasting straight down from the center can fail when on steep slopes
		// So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
		else {
			Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, hit);
			if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit)
				sliding = true;
		}
 
		// If we were falling, and we fell a vertical distance greater than the threshold, run a falling damage routine
		if (falling) {
			falling = false;
			if (myTransform.position.y < fallStartLevel - fallingDamageThreshold && enableFallingDamage == true)
				ApplyFallingDamage (fallStartLevel - myTransform.position.y);
		}
 
		// If running is enabled, change to run speed when left shift is pressed:
		if (Input.GetKey(KeyCode.LeftShift) && enableRun == true)
		{
			speed = runSpeed;
		}
		// If crouching is enabled, change to crouch speed when "c" is pressed:
		else if (Input.GetKey("c") && enableCrouch == true)
		{
			speed = crouchSpeed;
		}
		// If nothing is pressed, use walkSpeed:
		else
		{
			speed = walkSpeed;
		}
 
		// If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
		if ( (sliding && slideWhenOverSlopeLimit) || (slideOnTaggedObjects && hit.collider.tag == "Slide") ) {
			var hitNormal = hit.normal;
			moveDirection = Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
			Vector3.OrthoNormalize (hitNormal, moveDirection);
			moveDirection *= slideSpeed;
			playerControl = false;
		}
		// Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
		else {
			moveDirection = Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputY * inputModifyFactor);
			moveDirection = myTransform.TransformDirection(moveDirection) * speed;
			playerControl = true;
		}
 
		// Jump! But only if the jump button has been released and player has been grounded for a given number of frames
		if (!Input.GetButton("Jump"))
			jumpTimer++;
		else if (jumpTimer >= antiBunnyHopFactor) {
			moveDirection.y = jumpSpeed;
			jumpTimer = 0;
		}
	}
	else {
		// If we stepped over a cliff or something, set the height at which we started falling
		if (!falling) {
			falling = true;
			fallStartLevel = myTransform.position.y;
		}
 
		// If air control is allowed, check movement but don't touch the y component
		if (airControl && playerControl) {
			moveDirection.x = inputX * speed * inputModifyFactor;
			moveDirection.z = inputY * speed * inputModifyFactor;
			moveDirection = myTransform.TransformDirection(moveDirection);
		}
	}
 
	// Apply gravity
	moveDirection.y -= gravity * Time.deltaTime;
 
	// Move the controller, and set grounded true or false depending on whether we're standing on something
	grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
}

function Update ()
{
//	var h = charHeight;
//	
//	if (Input.GetKey("c") && enableCrouch == true)
//	{
//		h = charHeight*0.5;
//	}
//	
//	var lastHeight = controller.height; //Stand up/crouch smoothly
//	controller.height = Mathf.Lerp(controller.height, h, 5*Time.deltaTime);
//	myTransform.position.y += (controller.height-lastHeight)/2; //Fix vertical position
}

// Store point that we're in contact with for use in FixedUpdate if needed
function OnControllerColliderHit (hit : ControllerColliderHit) {
	contactPoint = hit.point;
}
 
// If falling damage occured, this is the place to do something about it. You can make the player
// have hitpoints and remove some of them based on the distance fallen, add sound effects, etc.
function ApplyFallingDamage (fallDistance : float) {
	gameObject.SendMessage("ApplyDammage", fallDistance*fallingDamageMultiplier);
	Debug.Log ("Ouch! Fell " + fallDistance + " units!");	
}
 
@script RequireComponent(CharacterController)