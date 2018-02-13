using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class eyeseverywhere : MonoBehaviour {

	public GameObject theBlueEye;
	public int numBlueEyes;

	public GameObject theGreenEye;
	public int numGreenEyes;

	public GameObject theHazelEye;
	public int numHazelEyes;

	public GameObject theBrownEye;
	public int numBrownEyes;

	public Transform target;

	int minPosition = -8;
	int maxPosition = 8;
	GameObject[] ojitosAzules;
	GameObject[] ojitosVerdes;
	GameObject[] ojitosMiel;
	GameObject[] ojitosCafes;


	// Use this for initialization
	void Start () {
		ojitosAzules = new GameObject[numBlueEyes];
		ojitosVerdes = new GameObject[numGreenEyes];
		ojitosMiel = new GameObject[numHazelEyes];
		ojitosCafes = new GameObject[numBrownEyes];

		MakeEyes (theBlueEye, numBlueEyes, ojitosAzules);
		MakeEyes (theBrownEye, numBrownEyes, ojitosCafes);
		MakeEyes (theGreenEye, numGreenEyes, ojitosVerdes);
		MakeEyes (theHazelEye, numHazelEyes, ojitosMiel);


	}
	
	// Update is called once per frame
	void Update () {
	}

	void MakeEyes(GameObject eyeColor,  int numEyes, GameObject[] ojitos){
		for (int i = 0; i < numEyes; i++) {
			float cons_x = Random.Range (minPosition, maxPosition);
			float cons_y = Random.Range (minPosition, maxPosition);
			float cons_z = Random.Range (minPosition, maxPosition);
			ojitos[i] = GameObject.Instantiate (eyeColor, new Vector3 (cons_x, cons_y, cons_z), Quaternion.identity);
			ojitos[i].transform.SetParent (this.transform);
		}
	}
}
