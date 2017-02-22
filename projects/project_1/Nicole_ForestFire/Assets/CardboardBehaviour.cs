using UnityEngine;
using System.Collections;
public class CardboardBehaviour : MonoBehaviour {

	public GameObject fireContainer;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (GvrViewer.Instance.Triggered) {
			Transform headTransform = GvrViewer.Instance.GetComponentInChildren<GvrHead> ().transform;
			GameObject fire = (GameObject) Instantiate (fireContainer, headTransform.position , headTransform.rotation);
		
		}
	}
}
