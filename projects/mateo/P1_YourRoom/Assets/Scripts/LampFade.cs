using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampFade : FadeBehavior {

    Color startColor;
    public Color endColor;
    Light lampLight;

    [Range(0, 1)]
    public float flickerChance = 0.5f;
    public float period = 1f;
    float timer = 0f;
    bool bFlicker = false;

	// Use this for initialization
	void Start () {
        lampLight = GetComponentInChildren<Light>();
        startColor = lampLight.color;
	}
	
	// Update is called once per frame
	protected override void Update () {
        // base fading behavior
        base.Update();

        // at fixed intervals
        if (timer >= period) {
            // randomly select
            float dart = Random.Range(0f, 1f);
            if (dart < fade*flickerChance) {
                // whether the lamp flickers
                StartCoroutine(Flicker(Random.Range(1, 6)));
            }

            // restart timer
            timer = 0;
        }

        timer += Time.deltaTime;
	}

    protected override void Fade() {
        // lamp fades light color, not texture
        Color lCol = Color.Lerp(startColor, endColor, fade);

        lampLight.color = lCol;
    }

    IEnumerator Flicker(int n) {
        // make sure we only start one flicker at a time
        if (bFlicker) {
            yield break;
        }

        bFlicker = true;

        // cache original light intensity
        float intensity = lampLight.intensity;

        // flicker flicker
        for (int i = 0; i < n; ++i) {
            lampLight.intensity = Random.Range(0f, intensity*0.5f);

            yield return new WaitForEndOfFrame();// WaitForSeconds(0.05f);

            lampLight.intensity = Random.Range(intensity*0.5f, intensity);

            yield return new WaitForEndOfFrame();//  WaitForSeconds(0.05f);
        }

        // we done
        lampLight.intensity = intensity;

        bFlicker = false;
    }
}
