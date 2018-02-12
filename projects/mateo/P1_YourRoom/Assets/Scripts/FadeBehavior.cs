using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeBehavior : MonoBehaviour {

    Material mat;
    protected float fade;

    public float fadeSpeed = 1;
    public float resetSpeed = 0.1f;
    protected bool bReset = false;

	// Use this for initialization
	void Start () {
        mat = GetComponent<MeshRenderer>().material;
	}
	
	// Update is called once per frame
	protected virtual void Update () {
        Fade();

        if (bReset) {
            fade = Mathf.Lerp(fade, 0, resetSpeed);

            if (fade < 0.01) {
                fade = 0;
                bReset = false;
            }
        }
        else {
            fade += Time.deltaTime * fadeSpeed;
            fade = Mathf.Clamp01(fade);
        }
	}

    public void Reset() {
        bReset = true;
    }

    protected virtual void Fade() {
        mat.SetFloat("_Fade", fade);
    }
}
