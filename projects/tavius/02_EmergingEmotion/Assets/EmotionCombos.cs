using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EmotionCombos : MonoBehaviour {

	public static string tryCombo(GameObject emotionL, GameObject emotionR) {
		if (!emotionL.GetComponent<EmotionProperties>().isEmotion || !emotionR.GetComponent<EmotionProperties>().isEmotion) {
			Debug.Log ("One (or more) of those isn't an emotion, dude...");
		} else {
			string L = emotionL.GetComponent<EmotionProperties>().emotionType;
			string R = emotionR.GetComponent<EmotionProperties>().emotionType;
			string result;
			switch (R) {
			// JOY &
			case "JOY":
				switch (L) {
			// Unique
				case "JOY":
					result = "ECS";
					return result;
					break;
				case "SAD":
					result = "MEL";
					return result;
					break;
				case "FEA":
					result = "SUR";
					return result;
					break;
				default:
					result = "nope";
					return result;
					break;
				}
				break;
			// SADNESS &
			case "SAD":
				switch (L) {
				case "JOY":
					result = "MEL";
					return result;
					break;
			// Unique
				case "SAD":
					result = "DES";
					return result;
					break;
				case "FEA":
					result = "ANX";
					return result;
					break;
				default:
					result = "nope";
					return result;
					break;
				}
				break;
			// FEAR &
			case "FEA":
				switch (L) {
				case "JOY":
					result = "SUR";
					return result;
					break;
				case "SAD":
					result = "ANX";
					return result;
					break;
			// Unique
				case "FEA":
					result = "TER";
					return result;
					break;
				default:
					result = "nope";
					return result;
					break;
				}
				break;
			default:
				result = "nope";
				return result;
				break;
			}
				//      JOY
			    //      SAD
			    //      FEA
				//		J+J = Ecstasy    ECS
				//		S+S = Despair    DES
				//		J+F = Surprise   SUR
				//		F+F = Terror     TER
				//		F+S = Anxiety    ANX
				//		J+S = Melancholy MEL
		}
	}
}
