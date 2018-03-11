using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamerShake : MonoBehaviour {

	private Vector3 originalPos;
	public static CamerShake instance;

	void Start(){
		originalPos = transform.localPosition;
		instance = this;
	}

	void Update(){
		if (Input.GetMouseButtonDown (0)) {
			CamerShake.Shake (1.0f, 0.4f);
		}
	}
		

	public static void Shake (float duration, float amount) {
		instance.StopAllCoroutines();
		instance.StartCoroutine(instance.cShake(duration, amount));
	}

	public IEnumerator cShake (float duration, float amount) {
		float endTime = Time.time + duration;

		while (Time.time < endTime) {
			transform.localPosition = originalPos + Random.insideUnitSphere * amount;

			duration -= Time.deltaTime;

			yield return null;
		}

		transform.localPosition = originalPos;
	}
}

