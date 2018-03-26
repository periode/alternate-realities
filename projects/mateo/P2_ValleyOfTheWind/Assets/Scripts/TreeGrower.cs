using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreeGrower : MonoBehaviour {

    [Range(0, 0.2f)]
    public float rate = 0.05f;
    float maxSize = 1f;

    // Use this for initialization
    void Start() {
        transform.localScale = Vector3.zero;
        float rAngle = Random.Range(0f, 360f);
        transform.Rotate(0, rAngle, 0);

        maxSize = Random.Range(0.8f, 1.2f);
    }

    // Update is called once per frame
    void Update() {
        Vector3 scale = transform.localScale;
        if (scale.sqrMagnitude < maxSize * maxSize) {
            transform.localScale = Vector3.Lerp(scale, Vector3.one * maxSize, rate);
        }

    }
}
