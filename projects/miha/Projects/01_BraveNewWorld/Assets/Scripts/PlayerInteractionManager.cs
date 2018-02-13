using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteractionManager : MonoBehaviour {

	public float speed = 10.0F;
	public float rotationSpeed = 100.0F;
	public Rigidbody rb;
	bool can_connect_w_particle = false;
	public float connection_trigger_limit = 0.5f;
	Particles particles; 
	bool player_is_interacting = false;
	bool player_can_move = true;
	public bool is_on_journey = false;
	// Use this for initialization

	void Start () {
		// variable setups
		rb = this.GetComponent<Rigidbody> ();
		particles = GameObject.Find("ParticlesManager").GetComponent<Particles>();
		// function calls
		StartCoroutine (SeekConnectionWithParticle());
	}

	//update

	void Update(){
		CheckWorldBounds ();

		MovePlayer ();

//		MovePlayer ();
	}

	void CheckWorldBounds(){
		if(Mathf.Abs(transform.position.z) > 100){
			float z_mag = transform.position.z;
			float x_mag = transform.position.x;
			if (z_mag > 0) {
				transform.Translate(0, 0, -5);
			} else {
				transform.Translate(0,0,5);
			}


		}else if(Mathf.Abs(transform.position.x) > 100){
				transform.Translate(0,0,5);
	
		}
		
	}
	// this method is responsible for player movement based on user input

	void MovePlayer(){
		
		float movement_forward = Input.GetAxis("Vertical") * speed;
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		movement_forward *= Time.deltaTime;
		rotation *= Time.deltaTime;
		transform.Translate(0, 0, movement_forward);
		transform.Rotate(0, rotation, 0);

	}


	// this method *checks* if the player can seek connection with the particle

	IEnumerator SeekConnectionWithParticle(){
		while (player_is_interacting == false) {
//			Debug.Log ("trying to connect!");
			if (can_connect_w_particle) {
//				Debug.Log ("I can finally connect!!");
				float random_val = Random.value;
				if (random_val > connection_trigger_limit) {
					bool detected_active_particle = particles.DetectInteractableParticles ();
					if (!detected_active_particle) {
						particles.SetInteractableParticle ();
						//	Debug.Log ("new interactable particles!");
					}
				}
			}
			;
			yield return new WaitForSeconds (0.1f);
		}

	}


	// this method *determines* whether the player can start seeking connection with the particle

	public void StageConnectionWithParticle(bool tf){
		
		can_connect_w_particle = tf;

	}


}
