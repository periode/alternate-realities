using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JourneyScript : MonoBehaviour {

	// 
	Particle p;
	GameObject player;
	Vector3 player_offset_vector = new Vector3 (0, 0, -4);
	float travel_speed = 4.0f;

	public AnimationCurve hover_to_road;

	RoadGenerator my_road;

	bool moved_to_journey_position = false;
	bool travelling = false;
	// Use this for initialization
	void Start () {
		Debug.Log("running journey script!!!!!!");

		my_road = GameObject.Find ("RoadGenerator").gameObject.GetComponent<RoadGenerator> ();
//		travel_speed = p.GetComponent<Particle> ().travel_speed;
		GetComponent<CapsuleCollider> ().enabled = false;
		p = GetComponent<Particle> ();
		player = GameObject.Find ("Player");
		player.gameObject.GetComponent<PlayerInteractionManager>().enabled = false;
		p.hover = false;
		Vector3 journey_pos = player.transform.position + player_offset_vector;
		StartCoroutine (LerpToJourneyPosition (transform.position,journey_pos));
	}
	
	// Update is called once per frame
	void Update () {
		if (travelling) {
			//travel together
//			TravelWithPlayer();
		}
	}

	IEnumerator LerpToJourneyPosition(Vector3 init_pos, Vector3 end_pos){
		float lerp_time = 0f;
		float lerp_duration = 2.0f;
		while (lerp_time < lerp_duration) {
			float lerp_amt = lerp_time / lerp_duration;
			transform.position = Vector3.Lerp (init_pos, end_pos, lerp_amt);
			lerp_time += Time.deltaTime;
			yield return null;
		};
		moved_to_journey_position = true;
		Debug.Log("we have moved to journey position, lets move to the road!");

		Vector3 road_pos = my_road.GetMiddleOfTheRoadPosition ();
		StartCoroutine (LerpToRoad(transform.position, road_pos));
	}

	IEnumerator LerpToRoad(Vector3 init_pos, Vector3 end_pos){

		DisablePlayerCollider ();

		float lerp_time = 0f;
		float lerp_duration = 3.0f;
		Vector3 middle_of_road = new Vector3 (end_pos.x, 4, init_pos.z);
		while (lerp_time < lerp_duration) {
			float lerp_amt = lerp_time / lerp_duration;
			float y_boost = hover_to_road.Evaluate (lerp_amt);
			// own position
			transform.position = Vector3.Lerp (init_pos, middle_of_road, lerp_amt);
			transform.position = transform.position + new Vector3 (0, y_boost, 0);
			//player position
			player.transform.position = transform.position - player_offset_vector;
			lerp_time += Time.deltaTime;
			yield return null;
		};
		moved_to_journey_position = true;
		Debug.Log("we have moved to middle of the road, start travelling!");
		StartCoroutine(TravelWithPlayer ());
	}

	IEnumerator TravelWithPlayer(){
		float time = 0.0f;
		float travelling_duration = 20.0f;
		while(time < travelling_duration){
			transform.Translate (0, 0, -travel_speed);
			player.transform.position = transform.position - player_offset_vector;
//			if (transform.position.z < my_road.world_limit_z + 20) {
//				StartCoroutine (my_road.GenerateNewBlocks ());
//			};
			Debug.Log ("travelling!!!!");
			time += Time.deltaTime;
			yield return null;
		}

	}

	void DisablePlayerCollider(){
		player.GetComponent<BoxCollider> ().enabled = false;
	}
		
		


//	void MoveOnTrack
//	Vector3 CalculatePlayerOffsetVector(GameObject my_player){
//		float distance = 4;
//		Vector3 heading = my_player.transform.position - transform.position;
//		Vector3 normalized_heading = heading / heading.magnitude;
//
//	}


}
