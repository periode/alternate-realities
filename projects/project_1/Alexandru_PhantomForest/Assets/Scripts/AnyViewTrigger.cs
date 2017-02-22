using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class AnyViewTrigger : MonoBehaviour
{
	public enum AnyTriggerType
	{
		MAGIC_TREE_HOWL,
		MAGIC_TREE_LIGHTNING,
		MAGIC_SPIDERS
	}

	public AnyTriggerType triggerType;
	public bool Focused { get; set; }
	public AudioClip audioClip;
	public GameObject lightningSource;
	public GameObject target;

	AudioSource audio;
	float startTime = 0;

	void Start() {
		audio = GetComponentInParent<AudioSource> ();
	}

	void Update() {
		if (triggerType == AnyTriggerType.MAGIC_TREE_HOWL) {
			if (Focused && Time.time - startTime > 20) {
				audio.PlayOneShot (audioClip, 1);
				startTime = Time.time;
				Debug.Log ("Triggered Howl!");
			}
		} else if (triggerType == AnyTriggerType.MAGIC_TREE_LIGHTNING) {
			if (Focused && Time.time - startTime > 20) {
				audio.PlayOneShot (audioClip, 1);
				lightningSource.GetComponent<Lightning> ().show ();
				startTime = Time.time;
				Debug.Log ("Lightning!!!");
			}
		} else if (triggerType == AnyTriggerType.MAGIC_SPIDERS) {
			if (!Focused) {
				int shouldMove = Random.Range (0, 10);
				if (shouldMove == 0) {
					GameObject spider = transform.GetChild(Random.Range (0, transform.childCount)).gameObject;
					spider.GetComponent<Animation> ().Play ();
					spider.transform.position = Vector3.MoveTowards (spider.transform.position, target.transform.position, 0.9f);
				}
			}
		}
	}
	
}



