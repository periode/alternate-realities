using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRTK;
using UnityEngine.UI;

public class BookCounter : MonoBehaviour {

	VRTK_InteractableObject bookInteraction;
	private bool Grabbed = false;
	private bool pGrabbed = false;
	public Text bookCover;
	public Text bookSpine;

	// Use this for initialization
	void Start () {
		bookInteraction = this.GetComponent<VRTK_InteractableObject>();
		bookCover.text = GetComponent<BookContents> ().bookTitle;
		bookSpine.text = GetComponent<BookContents> ().bookTitle;
	}
	
	// Update is called once per frame
	void Update () {
		 Grabbed = bookInteraction.IsGrabbed();

		if (pGrabbed == false && Grabbed == true) {
			BooksGrabbedCount.booksGrabbed++;
			pGrabbed = true;
		} 

	}
}
