//CGINC

#if !defined(SPARKLE_INC)
#define SPARKLE_INC

#include "AutoLight.cginc" // Makes some light stuffs easier
#include "UnityPBSLighting.cginc" // Physical based lighting stuffz

// We redeclare our properties inside our CG program
sampler2D _MainTex; // Access texture
sampler2D _MainTex_ST; // Access texture tiling + offset
sampler2D _NoiseTex; // Noise texture
float _Scale;
float _Intensity;
float4 _Color;
float _Metallic;
float _Glossiness;

struct VData {
  float4 pos : POSITION; // POSITION: Raw position input data
  float2 uv : TEXCOORD0; // TEXCOORD0: UV coordinates / Main texture
  float3 normal : NORMAL; // NORMAL: Normal map
};

struct Vert2Frag {
  float4 position : SV_POSITION; // SV_POSITION: Math'd position interpolator
  float2 uv : TEXCOORD0;
  //float3 normal : NORMAL;
  float3 wPos : TEXCOORD1; // TEXCOORD#: literally anything else lmao
  float3 wNormal : TEXCOORD2;

  #if defined(VERTEXLIGHT_ON)
  float3 vertexLightColor : TEXCOORD3;
  #endif
};

UnityLight CreateLight (Vert2Frag v) {
  UnityLight light;
  UNITY_LIGHT_ATTENUATION(attenuation, 0, v.wPos); // Built-in attenuation magic
  #if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
  light.dir = normalize(_WorldSpaceLightPos0.xyz - v.wPos); // Built-in light position - world pos
  #else
  light.dir = _WorldSpaceLightPos0.xyz; // Directional lights only have a direction (stored instead of a position)
  #endif
  light.color = _LightColor0.rgb * attenuation; // Built-in light color
  light.ndotl = DotClamped(v.wNormal, light.dir); // Normal dot Light
  return light;
}

UnityIndirect CreateIndirect (Vert2Frag v) {
  UnityIndirect i;
  i.diffuse = 0;
  i.specular = 0;

  #if defined(VERTEXLIGHT_ON)
  i.diffuse = i.vertexLightColor; // Indirect lighting will be vertex lit
  #endif

  #ifdef FORWARD_BASE_PASS
  i.diffuse += max(0, ShadeSH9(float4(v.wNormal, 1))); // Spherical Harmonics for weak light approximation in our base pass
  #endif

  return i;
}

void ComputeVertexLightColor (VData vert, inout Vert2Frag v) {
  #if defined(VERTEXLIGHT_ON)
  // float3 lightPos = float3(
  //   unity_4LightPosX0.x, unity_4LightPosY0.x, unity_4LightPosZ0.x
  //   );
  //
  // float3 lightVec = lightPos - v.wPos;
  // float3 lightDir = normalize(lightVec);
  // float ndotl = DotClamped(v.wNormal, lightDir);
  // float attenuation = 1 / (1+ dot(lightDir, lightDir) * unity_4LightAtten0.x);
  //
  // v.vertexLightColor = unity_LightColor[0].rgb * ndotl * attenuation;
  // Built in version of above: take vertex light data and save it in our vertexLightColor
  v.vertexLightColor = Shade4PointLights(
    unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
    unity_LightColor[0].rgb, unity_LightColor[1].rgb,
    unity_LightColor[2].rgb, unity_LightColor[3].rgb,
    unity_4LightAtten0, v.wPos, v.wNormal
    );
  #endif
}

// Vertex Program
Vert2Frag Vert (VData v) {
  Vert2Frag v2f;
  v2f.position = UnityObjectToClipPos(v.pos); // Bult-in Unity world-to-screen projection
  v2f.uv = v.uv;//TRANSFORM_TEX(v.uv, _MainTex); // Built-in sample texture (tex2D) + apply Tile/Offset
  v2f.wPos = mul(unity_ObjectToWorld, v.pos).xyz; // Matrix multiplication using built-in to transform object coordinates to world coordinates
  v2f.wNormal = UnityObjectToWorldNormal(v.normal); // Built-in Unity object-to-world normals
  v2f.wNormal = normalize(v2f.wNormal);

  return v2f;
}

// Fragment Program
float4 Frag (Vert2Frag v) : SV_TARGET // SV_TARGET: shader target color data
{
  v.wNormal = normalize(v.wNormal);

  half3 viewDirection = normalize(v.wPos - _WorldSpaceCameraPos);

  float3 albedo = tex2D(_MainTex, v.uv).rgb*_Color.rgb;

  // Sample random texture for sparklz
  float3 rand = tex2D(_NoiseTex, v.uv*_Scale);
  rand -= float3(0.5, 0.5, 0.5);
  rand  = normalize(normalize(rand) + v.wNormal);

  float4 col = UNITY_BRDF_PBS(    // Unity built-in physics magic
      albedo, albedo*_Metallic,   // albedo, specularTint
      1 - _Metallic, _Glossiness, // oneMinusReflectivity, 1 - roughness
      v.wNormal, viewDirection,   // normal, view direction
      CreateLight(v), CreateIndirect(v)             // direct light, indirect light
    );

  // Sparkle uses the random texture and view direction to fake bright spots
  half sparkle = dot(-viewDirection, rand);
  sparkle = pow(saturate(sparkle), _Intensity);

  col += half4(sparkle, sparkle, sparkle, 0);

  return col;
}

#endif
