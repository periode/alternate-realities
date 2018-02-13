using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particles : MonoBehaviour {

	public GameObject particle;

	// particle array
	int num_of_particles = 10;
	GameObject[] particle_array; 
	Vector3 start_position = new Vector3(0,1,600);

	// timer
	Timer timer;

	// update particles bool
	bool release_new_particle = false;

	// Use this for initialization
	void Start () {
//		particle = gameObject.Find("Particle").
		particle_array = new GameObject[num_of_particles];

		// set up timer
		timer = GameObject.Find("Timer").GetComponent<Timer>();

		for (int i = 0; i < num_of_particles; i++) {
			particle_array [i] = GameObject.Instantiate (particle, start_position, Quaternion.identity);
			particle_array [i].SetActive(false);
		}
	}
	
	// Update is called once per frame
	void Update () {

		release_new_particle = timer.CheckForUpdate();

		if (release_new_particle) {
			// check for update
			for (int i = 0; i < particle_array.Length; i++) {
				bool particle_travels = particle_array [i].GetComponent<Particle> ().CheckTravelStatus ();

				if (!particle_travels) { 
					//					Debug.Log ("discharging particle number" + i);
					particle_array [i].SetActive (true);
					particle_array [i].GetComponent<Particle> ().ResetPosition (start_position);
					particle_array [i].GetComponent<Particle> ().is_travelling = true;
					//					particle_array [i].GetComponent<Renderer> ().material.color = Color.white;
					break;
				}
			}
		}
		
	}

		public bool DetectInteractableParticles(){
		for (int i = 0; i < particle_array.Length; i++) {
			bool particle_is_active = particle_array [i].GetComponent<Particle> ().seek_interaction;
			if (particle_is_active) {
					//one is active don't do anything
				return true;
			}
		};
		return false;

		}


//	public bool CheckInteractableParticles(){
//		GameObject[] interactable_particles = GameObject.FindGameObjectsWithTag("Interactable");
//		if (interactable_particles.Length == 0) {
////			Debug.Log ("there are no particles currently interacting with the player - fire away!");
//			Debug.Log ("there are" + interactable_particles.Length + "interactable particles right now");
//			return true;
//		} else {
//			return false;
//			Debug.Log ("there are" + interactable_particles.Length + "interactable particles right now");
//		}
//
//	}

	public void SetInteractableParticle(){
		for (int i = 0; i < particle_array.Length; i++) {
			bool particle_travels = particle_array [i].GetComponent<Particle> ().CheckTravelStatus ();
			if (particle_travels == false) {
				particle_array [i].tag = "Interactable";
				particle_array [i].GetComponent<Particle> ().seek_interaction = true;
				Debug.Log ("We have a new interactable particle!");
				break;
			}
		}
	}

}
