using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GenerateText : MonoBehaviour {

	Text gen;
	float startTime, duration;

	// Use this for initialization
	void Start () {
		gen = GetComponent<Text> ();
		startTime = Time.time;
		duration = 5f;
	}
	
	// Update is called once per frame
	void Update () {

		if (Time.time - duration > startTime) {
			//gen

			gen.text = charArrtoStr (randomText());
			startTime = Time.time;
		}
		
	}

	char[] randomText(){
		char[] titleArray = new char[10];
		char title;
		for (int i = 0; i < 10; i++) {
			title= (char) Random.Range(97, 122);
			titleArray[i] = title;
		}
		return titleArray;
	}

	string charArrtoStr(char[] charArr){
		string myStr = "";
		for (int i = 0; i<charArr.Length; i++) {
			myStr += charArr [i].ToString();
		}

		return myStr;
	}
}
