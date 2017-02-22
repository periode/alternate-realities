using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrowthController : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 forward = GetComponentInChildren<Camera> ().transform.TransformDirection(Vector3.forward);

		RaycastHit whatDidIHit = new RaycastHit();

		if(Physics.Raycast(transform.position, forward, out whatDidIHit, 20)){
			
			if(whatDidIHit.transform.gameObject.tag == "Interactable"){
				GameObject go = whatDidIHit.transform.gameObject;

				if(go.GetComponent<CylinderStretching>() != null)
					go.GetComponent<CylinderStretching> ().isGrowing = false;

				if (go.name == "MyCube"){
					if(go.GetComponent<AudioSource>().isPlaying == false)
						go.GetComponent<AudioSource> ().Play ();
				}
					
					
			}
		}else{
			GameObject[] allInteractable = GameObject.FindGameObjectsWithTag ("Interactable");

			Debug.Log (allInteractable.Length);

			for(int i = 0; i < allInteractable.Length; i = i + 1){
				if(allInteractable [i].GetComponent<CylinderStretching> () != null)
					allInteractable [i].GetComponent<CylinderStretching> ().isGrowing = true;
			}
		}
	}
}
