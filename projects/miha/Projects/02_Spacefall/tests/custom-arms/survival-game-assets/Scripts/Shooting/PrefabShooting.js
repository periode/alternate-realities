#pragma strict

var theBullet : Rigidbody;
var Speed = 20;

function Update () {
	if (Input.GetMouseButtonDown(0))
	{
		var clone = Instantiate(theBullet, transform.position, transform.rotation);
		clone.velocity = transform.TransformDirection(Vector3(0, 0, Speed));
		
		Destroy (clone.gameObject, 3);
	}
}