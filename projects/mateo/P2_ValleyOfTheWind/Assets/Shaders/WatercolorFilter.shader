Shader "Filter/Watercolor" {
	Properties {

		/* Basically, ShaderLab's uniform declarations
		_Property ("Name", Type) = [Default]
		*/

		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		// Custom Properties
		_Watercolor ("Watercolor Texture", 2D) = "gray" {}
		_Strength ("Strength", Range(0, 1)) = 1
		_Radius ("Radius", Int) = 2
	}

	// Uncomment to add a custom Editor GUI
	// CustomEditor "Namespace.EditorGUIClass"

	SubShader {
		Pass {
			Tags {
				//"LightMode" = "ForwardBase" // Base pass: Blend One Zero
			}

			Cull Off ZWrite Off ZTest Always
			Blend One Zero

			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			#pragma vertex Vert
			#pragma fragment Frag

			#include "WatercolorFilter.cginc"

			ENDCG
		}
	}
}
