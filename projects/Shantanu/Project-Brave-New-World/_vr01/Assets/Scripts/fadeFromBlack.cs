using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fadeFromBlack : MonoBehaviour {

    public float fadePerSecond;
    Renderer _renderer;
    Color _color;

    private void Awake() {
        _renderer = GetComponent<Renderer>();
        _color = _renderer.material.color;
    }

    // Use this for initialization
    void Start () {
        //StartCoroutine(fadeIn());
	}
	
	// Update is called once per frame
	void Update () {
        _color = new Color(_color.r, _color.g, _color.b, _color.a - (fadePerSecond * Time.deltaTime));
        Debug.Log(_color);
        _renderer.material.color = _color;
    }

    IEnumerator fadeIn() {
        while (_color.a > 0f) {
            _color = new Color(_color.r, _color.g, _color.b, _color.a - (fadePerSecond * Time.deltaTime));
            Debug.Log(_color);
            _renderer.material.color = _color;
            yield return null;
        }
    }
}
