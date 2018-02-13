using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickonFace : MonoBehaviour {
	public Vector3 delta;

	void OnMouseOver() {
		// If the left mouse button is pressed
		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Left click!");
			Destroy(this.transform.parent.gameObject);
		}
		// If the right mouse button is pressed
		if (Input.GetMouseButtonDown(1)) {
			Debug.Log("Right click!");
			// Call method from WorldGenerator class
			ProceduralGenerator.CloneAndPlace(this.transform.parent.transform.position + delta, // N = C + delta
				this.transform.parent.gameObject); // The parent GameObject
		}
	}
}