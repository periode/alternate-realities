using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EmotionProperties : MonoBehaviour {

	public static bool isEmotion = true;
	public string emotionType; // JOY, SAD, FEA, ECS, DES, SUR, TER, ANX, MEL
    float[] diameter = { 0.25f, 1, 2, 3, 4, 5, 6, 7, 8 };
    float[] drags = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    float[] angularDrags = { 0, 1, 2, 3, 4, 5, 6, 7, 8 };
    VRTK_InteractTouch
     
    // Add Interact Haptics (AUDIO)

    private void Start() {
        gameObject.transform.localScale = new Vector3(diameter[0], diameter[0], diameter[0]);
    }
}

// Example:

//public class EnvObjQualities : MonoBehaviour
//{
//	public float alpha;
//	public bool seen = false;
//
//	void Start () {
//		this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
//	}
//
//	void Update()
//	{
//		if (seen)
//		{
//			this.GetComponent<Renderer>().material.color = new Color(1, 1, 1, alpha);
//			this.tag = "Non-Interactable";
//		}
//	}
//}