Shader "Custom/celMobile" {
	Properties {
		/*
		Basically, ShaderLab's uniform declarations
		_Property ("Name", Type) = [Default]
		*/
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		// [NoScaleOffset] _NormalMap ("Normal Map", 2D) = "bump" {}
		// _BumpScale ("Bump Scale", Float) = 1
		_Glossiness ("Smoothness", Range(0,1)) = 0.5
		[Gamma] _Metallic ("Metallic", Range(0,1)) = 0.0 // Metallic works in gamma space, we need to tag that
		// _DetailTex ("Detail Albedo", 2D) = "gray" {}
		// [NoScaleOffset] _DetailNormal ("Detail Normals", 2D) = "bump" {}
		// _DetailBumpScale ("Detail Bump Scale", Float) = 1
		_Steps ("Steps", Range(1, 5)) = 3
		_Bias ("Bias", Range(0, 1)) = 0

		_MainAlt ("Albedo Alt", 2D) = "white" {}
		_Fade ("Fade Amount", Range(0, 1)) = 0
	}
	SubShader {
		Pass {
			Tags {
				"LightMode" = "ForwardBase" // Base pass: Blend One Zero
			}

			CGPROGRAM
			// Use shader model 3.0 target, to get nicer looking lighting
			#pragma target 3.0

			//#pragma multi_compile_fwdbase// DIRECTIONAL DIRECTIONAL_COOKIE POINT POINT_COOKIE SPOT
			#pragma multi_compile _ SHADOWS_SCREEN // Allow directional light shadow casting
			#pragma multi_compile _ VERTEXLIGHT_ON // Allow vertex lighting for point lights

			#pragma vertex Vert
			#pragma fragment Frag

			#define FORWARD_BASE_PASS

			#include "celMobile.cginc"

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

			#pragma multi_compile_fwdadd_fullshadows // SHADOWS_DEPTH SHADOWS_CUBE SHADOWS_SOFT SHADOWS_SCREEN//_fullshadows //DIRECTIONAL DIRECTIONAL_COOKIE POINT POINT_COOKIE SPOT

			#pragma vertex Vert
			#pragma fragment Frag

			#include "celMobile.cginc"

			ENDCG
		}

		Pass {
			Tags {
				"LightMode" = "ShadowCaster" // Cast shadows
			}

			CGPROGRAM

			#pragma target 3.0

			#pragma multi_compile_shadowcaster

			#pragma vertex ShadowVert
			#pragma fragment ShadowFrag

			#include "shadows.cginc"

			ENDCG
		}
	}
}
