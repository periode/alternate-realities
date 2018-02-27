using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BooksGrabbedCount : MonoBehaviour {
	public static int booksGrabbed = 0;
	public Text countText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		SetCountText ();
		
	}
	void SetCountText ()
	{
		countText.text = "You read " + booksGrabbed.ToString () + " books";

	}		
}
