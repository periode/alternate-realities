using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour {

	public bool is_travelling = false;
	public float speed = 20.0f;
	public float collision_time = 0.0f;
	public float connection_time = 0.0f;
	float attention_threshold = 6.0f;
	public bool is_interacting = false;

	public bool seek_interaction = false;
	public bool hover = false;
	float curr_time = 0f;
	float prev_time = 0f;
	public float travel_speed = 6.0f;

	Color init_color;
	Color end_color = Color.green;

	Renderer rend;
	JourneyScript journey;


	GameObject player;

	// animation curves
	public AnimationCurve movement_curve;
	public AnimationCurve easing;

	bool take_player_on_journey = false;


	// Use this for initialization
	void Start () {
//		is_travelling = false;
		player = GameObject.Find("Player");
		rend = GetComponent<Renderer>();
		journey = GetComponent<JourneyScript>();
		init_color = rend.material.color;
	}
	
	// Update is called once per frame
	void Update () {
		CheckBounds ();
		UpdatePosition ();
		if (connection_time > 0) {
			
		}

		// interact
		if(seek_interaction && hover){
			MeasurePlayerAttention ();
		}else if(seek_interaction){
			DetectProximity (player);
		}

	}

	public bool CheckTravelStatus(){
		return is_travelling;
	}

	private void UpdatePosition(){
		if (is_travelling && !is_interacting) {
			transform.Translate (0, 0, -6.0f);
		}
	}

//	void UpdatePosition(){
//		if (is_travelling && !is_interacting) {
//			transform.Translate (0, 0, -6.0f);
//		}
//	}

//	void UpdatePositionWithPlayer(){
//		if (is_travelling && !is_interacting) {
//			transform.Translate (0, 0, -6.0f);
//		}
//	}

	public void ResetPosition(Vector3 new_pos){
		this.transform.position = new_pos;
	}


	public void CheckBounds(){
		
		if (this.transform.position.z < -1200 && !is_interacting) {
			// reset positions
			is_travelling = false;
		} 
	}

	public void ConnectionTimer(){
		// trigger timer
//		connection_time = 0.1;
	}

	void DetectProximity(GameObject p){
		Vector3 player_position = p.transform.position;
		Vector3 self_position = this.transform.position;
		float dist = Vector3.Distance (self_position, player_position);
//		Debug.Log ("the distance is:" + dist);
		if (dist < 50 && !is_interacting) {
			is_interacting = true;
			transform.Translate (0, 0, 0);
			StartCoroutine(ApproachPlayer (p));
		}
	}

	IEnumerator ApproachPlayer(GameObject p){
		Debug.Log ("Beginning our approach!");
		is_interacting = true;

		float lerpSpeed = 0.01f;
		float lerpAmt = 0f;
		float lerpDuration = 5.0f;

		float curve_time = 0f;
		float curveAmt = 0f;

		while(lerpAmt < 1.0f)
		{
			float easeAmt = easing.Evaluate(lerpAmt);
			Vector3 heading = (transform.position - p.transform.position);
			Vector3 normalized_heading = heading / heading.magnitude;

			Vector3 final_particle_pos = p.transform.position + normalized_heading*4;
			Vector3 lerp_vector = Vector3.Lerp(transform.position, final_particle_pos, easeAmt);

			float y_boost = movement_curve.Evaluate (lerpAmt);
//			Debug.Log (y_boost);
//			Debug.Log (lerp_vector.y);
			transform.position = new Vector3 (lerp_vector.x, lerp_vector.y, lerp_vector.z);

			lerpAmt += lerpSpeed;
//			lerpAmt += Time.deltaTime*lerpSpeed;

			if (lerpAmt > 0.3f && !hover) {
				hover = true;
				StartCoroutine (HoverInFrontOfPlayer (transform.position));
			}
		
//
			yield return null;
			
		}
			
//		measure_attention = true;




	}

	IEnumerator HoverInFrontOfPlayer(Vector3 pos){
		float amt = 0f;
		float speed = 0.08f;
//		float original_y_pos = pos.y;

		while(hover == true){

			// Oscillate
			float y_offset = OscillationOffset(amt)/4;
			transform.position = transform.position + new Vector3 (0, y_offset*0.01f, 0);
			amt += speed;

			// Poll for attention
//			float count = playerAttentionCount();

			yield return null;
		}


	} 

	float OscillationOffset(float time){
		return Mathf.Sin (time)*1.5f;
	}

	public void AttentionCountUpdate(float new_time,float time_increment){
		curr_time = new_time;
		if (curr_time - prev_time < 0.5f) {
			//
			connection_time += time_increment;
//			Debug.Log ("incrementing connection time:" + curr_time);
			
		} else if (curr_time - prev_time > 0.8f) {
			// reset!
			connection_time = 0;
		}
		prev_time = curr_time;
	}

	void MeasurePlayerAttention(){
		
//		Debug.Log ("attention time is:" + connection_time);
		if (connection_time < attention_threshold) {
			float color_lerp_amt = connection_time / attention_threshold;
			Color lerp_color = Color.Lerp (init_color, end_color,color_lerp_amt);
			ChangeColor (lerp_color);
		}else if(connection_time >= attention_threshold){
			Debug.Log ("take player on journey!!!!!");
			journey.enabled = true;
		}
	}

	void ChangeColor(Color new_color){
		rend.material.color = new_color;
		rend.material.SetColor ("_EmissionColor", new_color);
		GetComponentInChildren<Light> ().color = new_color;
	}



}
