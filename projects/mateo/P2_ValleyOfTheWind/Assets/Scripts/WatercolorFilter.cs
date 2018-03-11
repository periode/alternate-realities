using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class WatercolorFilter : MonoBehaviour {

    [Range(0, 1)]
    public float strength = 1f;

    public Texture watercolorTexture;
    public Vector2 scale = Vector2.one;
    public Vector2 offset = Vector2.zero;

    [Range(1, 5)]
    public int radius = 2;

    Material mat;

	// Use this for initialization
	void Start () {
        mat = new Material(Shader.Find("Filter/Watercolor"));
        mat.SetTexture("_Watercolor", watercolorTexture);
	}

    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        mat.SetFloat("_Strength", strength);
        mat.SetTexture("_Watercolor", watercolorTexture);
        mat.SetTextureScale("_Watercolor", scale);
        mat.SetTextureOffset("_Watercolor", offset);
        mat.SetInt("_Radius", radius);

        Graphics.Blit(source, destination, mat);
    }
}
