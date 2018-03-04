using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class mergeDetection : MonoBehaviour
{
	//Set the speed number in the Inspector window
	public float moveSpeed;
	public float repelStrength;
	GameObject emotion;
	Rigidbody emotionRB;

	void Start()
	{
		emotion = this.gameObject;
		emotionRB = GetComponent<Rigidbody>();
	}

	void Update()
	{
		//Press right to move the GameObject to the right. Make sure you set the speed high in the Inspector window.
		if (Input.GetKey(KeyCode.RightArrow))
		{
			emotionRB.AddForce(Vector3.right * moveSpeed);
		}

		//Press the left arrow key to move the GameObject to the left
		if (Input.GetKey(KeyCode.LeftArrow))
		{
			emotionRB.AddForce(Vector3.left * moveSpeed);
		}

	}

	//Detect when there is a collision
	void OnCollisionStay(Collision collide)
	{
		//Output the name of the GameObject you collide with
		Debug.Log("I hit the GameObject : " + collide.gameObject.name);
	}

	// Applies an upwards force to all rigidbodies that enter the trigger.
	void OnTriggerStay(Collider otherEmotion)
	{
		// Get distance between the colliding objects:
		float dist = Vector3.Distance (emotion.transform.position, otherEmotion.transform.position);

		// If the object colliding with the emotion running the script has "compatibleEmotion":
		if (otherEmotion.CompareTag("compatibleEmotion"))
		{
			if (dist < 0.15) // TODO: Replace '0.15' with 15% of diameter of the emotion orbs.
			{
				// Debug.Log(emotion.name + " combined with " + otherEmotion.gameObject.name);
				Destroy(otherEmotion.gameObject);
				emotion.GetComponent<Renderer>().material.SetColor("_Color", Color.yellow);
			}
		}

		// If the colliding object is not compatible:
		if (otherEmotion.CompareTag("incompatibleEmotion"))
		{
			
			if (dist < 1) // TODO: Replace '1' with the diameter of the emotion orbs.
			{
				// Debug.Log(emotion.name + " repelled " + otherEmotion.gameObject.name);
				Vector3 repelVector = otherEmotion.transform.position - emotion.transform.position;
				otherEmotion.attachedRigidbody.AddForce(repelVector * (dist / dist) * repelStrength);

			}
		}
	}
	void OnTriggerExit(Collider otherEmotion)
	{
		otherEmotion.attachedRigidbody.velocity = Vector3.zero;
	}
}
