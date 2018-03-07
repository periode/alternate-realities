using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;

public class EmotionProperties : MonoBehaviour {

	public static bool isEmotion = true;
	public string emotionType; // JOY, SAD, FEA, ECS, DES, SUR, TER, ANX, MEL
    float[] diameter =     { 0.25f, 1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f };
    float[] drag =        { 3.3f,  1f, 2f, 3f, 4f, 5, 6f, 7f, 8f };
    float[] angularDrag = { 0.3f,  1f, 2f, 3f, 4f, 5f, 6f, 7f, 8f };
    Color[] color = { joy, fea, sad, ecs, des, sur, ter, anx, mel };

    // Color bank:
    public static Color joy = new Color(1f, 0.933333333f, 0f);
    public static Color fea = new Color(1f, 0f, 0f);
    public static Color sad = new Color(0.31372549019f, 0f, 0.49803921568f);
    public static Color ecs = new Color(0.11764705882f, 0.17647058823f, 0.17647058823f);
    public static Color des = new Color(0f, 0f, 0f);
    public static Color sur = new Color(0f, 0f, 0f);
    public static Color ter = new Color(0f, 0f, 0f);
    public static Color anx = new Color(0f, 0f, 0f);
    public static Color mel = new Color(0f, 0f, 0f);

    //    VRTK_InteractTouch;

    // Add Interact Haptics (AUDIO)


    private void Make(string thisEmotion)
    {
        int e = DetermineEmotion(emotionType);
        if (e == -1)
        {
            Debug.LogError("Invalid input. Check EmotionType of EmotionProperties component.");
        }
        else
        {
            gameObject.transform.localScale = new Vector3(diameter[e], diameter[e], diameter[e]);
            gameObject.GetComponent<Rigidbody>().drag = drag[e];
            gameObject.GetComponent<Rigidbody>().angularDrag = angularDrag[e];
            gameObject.GetComponent<Renderer>().material.SetColor("_Color", color[e]);
        }
    }

    // Quick function to test given Emotion Type passed to this component:
    private int DetermineEmotion(string thisEmotion)
    {
        thisEmotion = emotionType;
        switch (thisEmotion)
        {
            case "JOY":
                return 0;
            case "SAD":
                return 1;
            case "FEA":
                return 2;
            case "ECS":
                return 3;
            case "DES":
                return 4;
            case "SUR":
                return 5;
            case "TER":
                return 6;
            case "ANX":
                return 7;
            case "MEL":
                return 8;
            default:
                return -1;
        }
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