//CGINC

#if !defined(CELTEX_INC)
#define CELTEX_INC

#include "AutoLight.cginc" // Makes some light stuffs easier
#include "UnityPBSLighting.cginc" // Physical based lighting stuffz

// We redeclare our properties inside our CG program
sampler2D _MainTex, _DetailTex; // Access texture
float4 _MainTex_ST, _DetailTex_ST; // Access texture tiling + offset
sampler2D _NormalMap, _DetailNormal;
float _BumpScale, _DetailBumpScale;
float4 _Color; // Tint
float _Metallic; // Metallic
float _Glossiness; // AKA 1 - Roughness
float _Steps;
float _Bias;

sampler2D _MainAlt;
float4 _MainAlt_ST;
float _Fade;

// Struct to handle Vertex Data
struct VData {
  float4 pos : POSITION; // POSITION: Raw position input data
  float2 uv : TEXCOORD0; // TEXCOORD0: UV coordinates / Main texture
  float3 normal : NORMAL; // NORMAL: Normal map
  float4 tangent : TANGENT; // TANGENT: Tangent vector
};

// Structure to Interpolate between Vertex and Fragment programs
struct Interpolators {
  float4 position : SV_POSITION; // SV_POSITION: Math'd screen position interpolator
  float4 uv : TEXCOORD0;
  //float3 normal : NORMAL;
  // TEXCOORD#: literally anything else lmao
  float3 wPos : TEXCOORD1; // World Space Position
  float3 wNormal : TEXCOORD2; // World Space Normals
  float4 wTangent : TEXCOORD3; // World Space Tangents

  #if defined(VERTEXLIGHT_ON)
  float3 vertexLightColor : TEXCOORD4; // Vertex Lighting
  #endif
};

float Max3(float4 v) {
  return max(max(v.r, v.g), v.b);
}

float Max4(float4 v) {
  return max(max(v.r, v.g), max(v.b, v.a));
}

// Utility function to calculate fragment normals using bump/normal maps
void FragmentNormal(inout Interpolators i) {
  float3 mainNormal = UnpackScaleNormal(tex2D(_NormalMap, i.uv.xy), _BumpScale); // This samples the Normal Map using the proper encoding, and scales them using the Bump Scale
  float3 detNormal = UnpackScaleNormal(tex2D(_DetailNormal, i.uv.zw), _DetailBumpScale);

  float3 tsNormal = BlendNormals(mainNormal, detNormal); // Blend Tangent Space Normals using Whiteout Blending and nromalize

  float3 binormal = cross(i.wNormal, i.wTangent.xyz) * (i.wTangent.w * unity_WorldTransformParams.w); // Calculate Binormal vector and factor in object scale sign

  i.wNormal = normalize(
    tsNormal.x * i.wTangent +
    tsNormal.y * binormal +
    tsNormal.z * i.wNormal
    );
}

// Utility to create a UnityLight struct based on our interpolated data
UnityLight CreateLight (Interpolators i) {
  UnityLight light;
  UNITY_LIGHT_ATTENUATION(attenuation, i, i.wPos);//0, i.wPos); // Built-in attenuation magic
  #if defined(POINT) || defined(POINT_COOKIE) || defined(SPOT)
  light.dir = -normalize(i.wPos - _WorldSpaceLightPos0.xyz ); // Built-in light position - world pos
  #else
  light.dir = _WorldSpaceLightPos0.xyz; // Directional lights only have a direction (stored instead of a position)
  #endif
  light.color = _LightColor0.rgb * attenuation; // Built-in light color
  light.ndotl = DotClamped(i.wNormal, light.dir); // Normal dot Light
  return light;
}

// Utility to create a UnityIndirect struct based on our interpolated data
UnityIndirect CreateIndirect (Interpolators i) {
  UnityIndirect ind;
  ind.diffuse = 0;
  ind.specular = 0;

  #if defined(VERTEXLIGHT_ON)
  ind.diffuse = i.vertexLightColor; // Indirect lighting will be vertex lit
  #endif

  #ifdef FORWARD_BASE_PASS
  ind.diffuse += max(0, ShadeSH9(float4(i.wNormal, 1))); // Spherical Harmonics for weak light approximation in our base pass
  #endif

  return ind;
}

// Utility to compute vertex lighting
void ComputeVertexLightColor (VData v, inout Interpolators i) {
  #if defined(VERTEXLIGHT_ON)
  // Take vertex light data and save it in our vertexLightColor
  i.vertexLightColor = Shade4PointLights(
    unity_4LightPosX0, unity_4LightPosY0, unity_4LightPosZ0,
    unity_LightColor[0].rgb, unity_LightColor[1].rgb,
    unity_LightColor[2].rgb, unity_LightColor[3].rgb,
    unity_4LightAtten0, i.wPos, i.wNormal
    );
  #endif
}

// Vertex Program
Interpolators Vert (VData v) {
  Interpolators v2f;

  v2f.position = UnityObjectToClipPos(v.pos); // Bult-in Unity object-to-screen projection
  v2f.uv.xy = TRANSFORM_TEX(v.uv, _MainTex); // Built-in sample texture (tex2D) + apply Tile/Offset
  v2f.uv.zw = TRANSFORM_TEX(v.uv, _DetailTex);
  v2f.wPos = mul(unity_ObjectToWorld, v.pos).xyz; // Matrix multiplication using built-in to transform object coordinates to world coordinates
  v2f.wNormal = UnityObjectToWorldNormal(v.normal); // Built-in Unity object-to-world normals
  v2f.wNormal = normalize(v2f.wNormal); // Normalize normals just in case
  v2f.wTangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w); // Object-to-world tangents

  return v2f;
}

// Fragment Program
float4 Frag (Interpolators i) : SV_TARGET // SV_TARGET: shader target color data
{
  FragmentNormal(i);

  half3 viewDirection = -normalize(i.wPos - _WorldSpaceCameraPos); // I legit dunno why that minus is needed but light would be ded otherwise

  // sample main and alternate textures
  float3 mTex = tex2D(_MainTex, i.uv).rgb;
  float3 aTex = tex2D(_MainAlt, i.uv).rgb;

  // blend textures and apply tint
  float3 albedo = (mTex + (aTex - mTex)*_Fade)*_Color.rgb;
  albedo *= tex2D(_DetailTex, i.uv.zw) * unity_ColorSpaceDouble; // Factor in detail albedo texture

  float3 specularTint;
  float oneMinusReflectivity;
  // Derive final albedo from diffuse color + metallic property
  albedo = DiffuseAndSpecularFromMetallic(
    albedo, _Metallic, specularTint, oneMinusReflectivity
    );

  // Unity built-in physics magic for Physically Based Lighting
  float4 col = UNITY_BRDF_PBS(
      albedo, specularTint,   // albedo, specularTint
      oneMinusReflectivity, _Glossiness, // oneMinusReflectivity, 1 - roughness
      i.wNormal, viewDirection,   // normal, view direction
      CreateLight(i), CreateIndirect(i)             // direct light, indirect light
    );

  float br = Max3(col);
  float shade = 0.5;
  float step =  1.0/int(_Steps);
  for (int j = 0; j < _Steps; ++j) {
    if (br < j*step) {
      continue;
    } else {
      shade = (j+_Bias)*step;
      //break;
    }
  }

  col *= shade;
  col /= br;

  // return float4(normalize(i.wPos.xyz), 1);
  return saturate(float4(col.rgb, 1));
}

#endif
