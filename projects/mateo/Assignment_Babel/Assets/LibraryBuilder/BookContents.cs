using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookContents : MonoBehaviour {

	public string bookTitle;
    public string ovrText = "";
	const string glyphs= "abdefghijklmnoprstuwxy .,";

	// Use this for initialization
	void Start () {
        if (ovrText == "") {
            int charAmount = 8; //set those to the minimum and maximum length of your string

            for (int i = 0; i < charAmount; i++) {
                bookTitle += glyphs[Random.Range(0, glyphs.Length)];
            }
        } else {
            bookTitle = ovrText;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
