using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookContents : MonoBehaviour {

	public string bookTitle;
	const string glyphs= "abcdefghijklmnopqrstuvwxyz ";


	// Use this for initialization
	void Start () {
		int charAmount = 25; //set those to the minimum and maximum length of your string

		for(int i=0; i<charAmount; i++)
		{
			bookTitle += glyphs[Random.Range(0, glyphs.Length)];
		}
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
