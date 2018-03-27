Shader "Custom/LightClear" {
	Properties {

		/* Basically, ShaderLab's uniform declarations
		_Property ("Name", Type) = [Default]
		*/

		// Albedo and Tint
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}

		// Normal Map
		[NoScaleOffset] _NormalMap ("Normal Map", 2D) = "bump" {}
		_BumpScale ("Bump Scale", Float) = 1

		// Smoothness and Metallic (Map)
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		[NoScaleOffset] _MetallicMap ("Metallic", 2D) = "white" {}
		[Gamma] _Metallic ("Metallic", Range(0,1)) = 0.0 // Metallic works in gamma space, we need to tag that

		// // Emission (Map)
		// [NoScaleOffset] _EmissionMap ("Emission", 2D) = "black" {}
		// _Emission ("Emission", Color) = (0, 0, 0)

		// // Detail Albedo and Normals
		// _DetailTex ("Detail Albedo", 2D) = "gray" {}
		// [NoScaleOffset] _DetailNormal ("Detail Normals", 2D) = "bump" {}
		// _DetailBumpScale ("Detail Bump Scale", Float) = 1

		// Custom properties
		_LightThreshold ("Light Threshold", Range(0, 1)) = 0.5
	}

	// Uncomment to add a custom Editor GUI
	// CustomEditor "Namespace.EditorGUIClass"

	SubShader {
		Pass {
			Tags {
				"LightMode" = "ForwardBase" // Base pass: Blend One Zero
			}

			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			//#pragma multi_compile_fwdadd_fullshadows// DIRECTIONAL DIRECTIONAL_COOKIE POINT POINT_COOKIE SPOT
			#pragma multi_compile _ SHADOWS_SCREEN // Allow shadow casting
			#pragma multi_compile _ VERTEXLIGHT_ON // Allow vertex lighting for point lights

			// shader_feature compiles ONLY if it is needed at build time
			#pragma shader_feature _ _METALLIC_MAP // Custom keyword directive to enable metallic mapping
			#pragma shader_feature _ _SMOOTHNESS_ALBEDO _SMOOTHNESS_METALLIC // Custom keywords to determine smoothness map source

			#pragma vertex Vert
			#pragma fragment Frag

			#define FORWARD_BASE_PASS

			#include "LightClear.cginc"

			ENDCG
		}

		Pass {
			Tags {
				"LightMode" = "ForwardAdd" // Additive passes
			}

			Blend SrcAlpha OneMinusSrcAlpha // One One = Additive, One Zero = no blending
			//ZWrite Off // Turn off z-buffer writing because it's our second+ pass

			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			#pragma multi_compile_fwdadd_fullshadows// DIRECTIONAL DIRECTIONAL_COOKIE POINT POINT_COOKIE SPOT

			// shader_feature compiles ONLY if it is needed at build time
			#pragma shader_feature _ _METALLIC_MAP // Custom keyword directive to enable metallic mapping
			#pragma shader_feature _ _SMOOTHNESS_ALBEDO _SMOOTHNESS_METALLIC // Custom keywords to determine smoothness map source

			#pragma vertex Vert
			#pragma fragment Frag

			#include "LightClear.cginc"

			ENDCG
		}
	}
}
