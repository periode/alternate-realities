using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class PanelManager : MonoBehaviour {

	public GameObject book;

	public int numberOfShelves = 4;
	public int numberOfBooksPerShelf = 20;

	public float bookSpacing = 0.125f;
	public float shelfSpacing = 0.55f;

	public float hexagon;
	public int rowNumber;

    new BoxCollider collider;

	// Use this for initialization
	void Start () {
		for (int i = 0; i < numberOfShelves; i++) {
			for (int j = 0; j < numberOfBooksPerShelf; j++) {
				GameObject b = GameObject.Instantiate(book, new Vector3 (transform.localPosition.x + j*bookSpacing, transform.localPosition.y + i*shelfSpacing, transform.localPosition.z), Quaternion.identity);
				b.transform.SetParent (transform);
				b.transform.localPosition = new Vector3 (b.transform.localPosition.x + rowNumber, b.transform.localPosition.y, b.transform.localPosition.z);
				//b.transform.localRotation = transform.rotation;
			}
		}

		transform.localEulerAngles = new Vector3 (0, hexagon * 60, 0);

        collider = GetComponent<BoxCollider>();

        float width = bookSpacing * numberOfBooksPerShelf;
        float height = shelfSpacing * numberOfShelves;

        collider.size = new Vector3(width, height, 0.1f);
        collider.center = new Vector3(width*0.5f - 0.05f + shelfSpacing*rowNumber*1.8f, height*0.5f - 0.3f, 0);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
