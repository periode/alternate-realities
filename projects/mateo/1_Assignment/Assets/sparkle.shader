// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/sparkle" {
	Properties {
		/*
		Basically, ShaderLab's uniform declarations
		_Property ("Name", Type) = [Default]
		*/
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_NoiseTex ("Noise Texture", 2D) = "white" {}
		_Scale ("Scale", Float) = 1
		_Intensity ("Intensity", Float) = 50
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		[Gamma] _Metallic ("Metallic", Range(0,1)) = 0.0 // Metallic works in gamma space, we need to tag that
	}
	SubShader {
		Pass {
			Tags {
				"LightMode" = "ForwardBase" // Base pass: Blend One Zero
			}

			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			#pragma multi_compile _ VERTEXLIGHT_ON // Allow vertex lighting for point lights

			#pragma vertex Vert
			#pragma fragment Frag

			#define FORWARD_BASE_PASS

			#include "sparkle.cginc"

			ENDCG
		}

		Pass {
			Tags {
				"LightMode" = "ForwardAdd" // Additive passes
			}

			Blend One One // One One = Additive, One Zero = no blending
			ZWrite Off // Turn off z-buffer writing because it's our second+ pass

			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			#pragma multi_compile fwdadd // Shorthand for: DIRECTIONAL DIRECTIONAL_COOKIE POINT POINT_COOKIE SPOT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "sparkle.cginc"

			ENDCG
		}
	}
}
