//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//
//public class Controllers : MonoBehaviour {
//
//	public GameObject leftEmotion;
//	public GameObject rightEmotion;
//	public bool leftHolding;
//	public bool rightHolding;
//	public float repelStrength;
//
//	//Set the speed number in the Inspector window
//	public float moveSpeed;
//	Rigidbody rightEmotionRB;
//
//	void Start()
//	{
//		rightEmotionRB = rightEmotion.GetComponent<Rigidbody>();
//	}
//
//	void Update()
//	{
//		//Press right to move the GameObject to the right. Make sure you set the speed high in the Inspector window.
//		if (Input.GetKey(KeyCode.RightArrow))
//		{
//			rightEmotionRB.AddForce(Vector3.right * moveSpeed);
//		}
//		//Press the left arrow key to move the GameObject to the left
//		if (Input.GetKey(KeyCode.LeftArrow))
//		{
//			rightEmotionRB.AddForce(Vector3.left * moveSpeed);
//		}
//	}
//
//
//	void RightTriggerDown() {
//		if(this.CompareTag("emotion")){
//			rightEmotion = this.raycast;
//			rightEmotion.AddComponent<EmotionCombos>();
//			//Attach rightEmotion to rightContactPoint
//			rightHolding = true;
//		}
//	}
//
//	void RightTriggerUp() {
//		if(rightHolding){
//			rightEmotion.RemoveComponent<EmotionCombos>();
//			rightEmotion = null; // Detach rightContactPoint?
//			rightHolding = false;
//		}
//	}
//
//	void LeftTriggerDown() {
//		if(this.CompareTag("emotion")){
//			leftEmotion = this;
//			//Attach to leftEmotion to leftContactPoint
//			leftHolding = true;
//		}
//	}
//
//	void LeftTriggerUp() {
//		if(leftHolding){
//			leftEmotion = null; // Detach leftContactPoint?
//			leftHolding = false;
//		}
//	}
//
//	void ActivateEmotions() {
//		if (leftHolding && rightHolding) {
//			rightEmotion.GetComponent<Collider>().isTrigger = true;
//		}
//	}
//
//
//
//	// Automatically triggered by the trigger when it 
//	void OnTriggerStay(Collider otherEmotion)
//	{
//		float dist = Vector3.Distance(rightEmotion.transform.position, otherEmotion.transform.position);
//
//		// Lifting main orbs to face:
//		if (rightEmotion.name.Equals("JOY") || rightEmotion.name.Equals("SAD") || rightEmotion.name.Equals("FEA")) {
//			if (otherEmotion.CompareTag("goggles")) {
//				//if (dist < otherEmotion.GetComponent<GoggleProperties>().length * 0.3) {
//					GameObject.Find("Ground").GetComponent<Renderer>().material.SetColor ("_Color", Color.yellow);
//					//GameObject newEmotion = Instantiate(groundPrefab, rightEmotion.transform.position, rightEmotion.transform.rotation);
//			}
//		}
//
//		// Make sure only the left orb reacts with the right:
//		int leftTest = leftEmotion.GetInstanceID(); 
//		Debug.Log(leftTest);
//		Debug.Log(otherEmotion.GetInstanceID());
//		if (leftEmotion.getInstanceID() != otherEmotion.getinstanceID()) {
//			Debug.Log ("Try to combine objects you are holding");
//		} else {
//			// Get distance between the colliding objects:
//			// my_texture = GameObject.Find("World").gameObject.GetComponent<TexturePainting>();
//			string typeOfEmotion = tryCombo(otherEmotion, rightEmotion);
//			// If the emotions are not compatible, repel:
//			if (typeOfEmotion.Equals("nope")) {
//				if (dist < otherEmotion.GetComponent<EmotionProperties>().diameter) {
//					// Debug.Log(rightEmotion.name + " repelled " + otherEmotion.gameObject.name);
//					Vector3 repelVector = otherEmotion.transform.position - rightEmotion.transform.position;
//					otherEmotion.attachedRigidbody.AddForce (repelVector * (dist / dist) * repelStrength);
//				}
//			}
//
//			// If the colliding emotion IS compatible:
//			else {
//				if (dist < otherEmotion.GetComponent<EmotionProperties>().diameter * 0.15) {
//					// Debug.Log(rightEmotion.name + " combined with " + otherEmotion.gameObject.name);
//					Destroy(otherEmotion.gameObject);
//					GameObject newEmotion = Instantiate(typeOfEmotion, rightEmotion.transform.position, rightEmotion.transform.rotation);
//					Destroy(rightEmotion);
//					//emotion.GetComponent<Renderer> ().material.SetColor ("_Color", Color.yellow);
//				}
//			}
//		}
//	}
//	void OnTriggerExit(Collider otherEmotion)
//	{
//		if (otherEmotion.CompareTag ("incompatibleEmotion"))
//		{
//			otherEmotion.attachedRigidbody.velocity = Vector3.zero;
//		}
//	}
//
//}
