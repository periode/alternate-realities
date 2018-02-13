using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadGenerator : MonoBehaviour {

	public GameObject road_block;
	float road_block_length;
	int num_of_blocks = 200;
	int num_of_pos_z_blocks; 
	int new_blocks = 20;
	public float world_limit_z;
//	static GameObject road;

	// Use this for initialization
	void Start () {
		num_of_pos_z_blocks = num_of_blocks / 2;
		road_block_length = road_block.GetComponent<MeshRenderer>().bounds.extents.z;
//		world_limit_z = num_of_blocks *  * road_block_length;
		UpdateWorldLimit(num_of_pos_z_blocks);
		GenerateRoad ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void UpdateWorldLimit(float num){
		world_limit_z = road_block_length * num;
	}

	void GenerateRoad(){
		
		for (int i = 0; i < num_of_blocks; i++) {
			if (i % 2 == 0) {
				GameObject.Instantiate (road_block, new Vector3 (0, 0, road_block_length*(i - num_of_blocks/2)), Quaternion.identity);
			} else {
				GameObject.Instantiate (road_block, new Vector3 (0, 0, -road_block_length*(i + num_of_blocks/2)), Quaternion.identity);
			}
				
		}
	}

	public Vector3 GetMiddleOfTheRoadPosition(){
		float middle = (road_block.GetComponent<MeshRenderer>().bounds.extents.x) - 2.5f;
//		return (middle,0
		return new Vector3(middle,0,0);
	}

	public IEnumerator GenerateNewBlocks(){
		UpdateWorldLimit(num_of_pos_z_blocks + new_blocks);
		for (int i = 0; i < new_blocks; i++) {
			GameObject.Instantiate (road_block, new Vector3 (0, 0, road_block_length*(i + num_of_pos_z_blocks)), Quaternion.identity);
			yield return null;
		}
		num_of_pos_z_blocks += new_blocks;
	}
		
}
