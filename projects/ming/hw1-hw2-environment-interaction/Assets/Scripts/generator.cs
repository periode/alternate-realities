using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class generator : MonoBehaviour {
	public GameObject block;
	public int numBlocks;
	public float period = 0.05f;//seconds
	public float red = 1f;
	public float green = 0.6f;
	public float blue = 0f;//Random.Range (0.5f, 0.6f);
	float timer = 0;
	float Theta = 0f;
	float degreeChange = 0.2f;
	float radius = 50f;
	int h = 0;
	int layers = 10;
	float center_x = 0;
	float center_z = 0;
	int counter = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        // Draw a circle of pyramids
		if (counter < 2.1 / degreeChange) {
			if (timer >= period) {
				if (h == layers || h == 0) {
					Theta += (degreeChange * Mathf.PI);
					center_x = radius * Mathf.Cos (Theta);
					center_z = radius * Mathf.Sin (Theta);
					h = 0;
					counter++;
				}

				timer = 0;
				h++;
				for (float i = -layers + h + center_x; i < layers + 1 - h + center_x; i++) {
					for (float j = -layers + h + center_z; j < layers + 1 - h + center_z; j++) {
						GameObject obj = Instantiate (block, new Vector3 (i, h, j), Quaternion.identity);
						blue = Random.Range (0f, 0.1f * h);
                        red = 0.1f * h + 0.3f;
						Color col = new Color (red, green, blue);
                        obj.GetComponent<cubeColor> ().baseColor = col;
					}
				}
			}

			timer += Time.deltaTime;
		}
	}
}
