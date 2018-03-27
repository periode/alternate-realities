//CGINC

#ifndef LIGHTSOLID_INC
#define LIGHTSOLID_INC

#include "UnityPBSLighting.cginc" // Physical based lighting stuffz
#include "AutoLight.cginc" // Makes some light stuffs easier

// We redeclare our properties inside our CG program
sampler2D _MainTex; // Access texture
float4 _MainTex_ST; // Access texture tiling + offset

sampler2D _NormalMap;
float _BumpScale;

float4 _Color; // Tint

sampler2D _MetallicMap; // Metallic Map
float _Metallic; // Metallic
float _Glossiness; // AKA 1 - Roughness

float _LightThreshold;
float4 _HiddenColor;

// Struct to handle Vertex Data
struct VData {
  float4 vertex : POSITION; // POSITION: Raw position input data
  float4 uv : TEXCOORD0; // TEXCOORD0: UV coordinates / Main texture
  float3 normal : NORMAL; // NORMAL: Normal vector
  float4 tangent : TANGENT; // TANGENT: Tangent vector
};

// Structure to Interpolate between Vertex and Fragment programs
struct Interpolators {
  float4 pos : SV_POSITION; // SV_POSITION: Math'd screen position interpolator
  float4 uv : TEXCOORD0;
  //float3 normal : NORMAL;
  // TEXCOORD#: literally anything else lmao
  float3 wPos : TEXCOORD1; // World Space Position
  float3 wNormal : TEXCOORD2; // World Space Normals
  float4 wTangent : TEXCOORD3; // World Space Tangents

  SHADOW_COORDS(4) // Macro for defining shadow coordinates

  #if defined(VERTEXLIGHT_ON)
  float3 vertexLightColor : TEXCOORD5; // Vertex Lighting
  #endif
};

// Utility to calculate brightness
float brightness(float3 col) {
    return 0.2126*col.r + 0.7152*col.g + 0.0722*col.b;
}

// Utility to get the max color value
float max3(float3 v) {
  return max(v.r, max(v.g, v.b));
}

// Utility function to calculate fragment normals using bump/normal maps
void FragmentNormal(inout Interpolators i) {
  float3 mainNormal = UnpackScaleNormal(tex2D(_NormalMap, i.uv.xy), _BumpScale); // This samples the Normal Map using the proper encoding, and scales them using the Bump Scale

  float3 tsNormal = mainNormal; // Blend Tangent Space Normals using Whiteout Blending and nromalize

  float3 binormal = cross(i.wNormal, i.wTangent.xyz) * (i.wTangent.w * unity_WorldTransformParams.w); // Calculate Binormal vector and factor in object scale sign

  #ifdef FORWARD_BASE_PASS
    i.wNormal = normalize(i.wNormal);
  #else
    i.wNormal = normalize(
      tsNormal.x * i.wTangent +
      tsNormal.y * binormal +
      tsNormal.z * i.wNormal
      );
  #endif
}

// Utility to compute a box projection
float3 BoxProjection(
  float3 dir, float3 pos,
  float4 cubemapPos, float3 boxMin, float3 boxMax
  ) {
    // Check if box projection is supported
    #if UNITY_SPECCUBE_BOX_PROJECTION

    // Check if we should use box projection
    if (cubemapPos.w > 0) {
      //find intersection with box bounds
      float3 factors = ((dir > 0 ? boxMax : boxMin) - pos) / dir;

      // correct scalar is the closest / smallest one
      float scalar = min(min(factors.x, factors.y), factors.z);
      dir = dir * scalar + (pos - cubemapPos);
    }
    #endif

    return dir;
  }

// Utility to combine unifom and mapped metallic values - change as needed
float GetMetallic (Interpolators i) {
  #ifdef _METALLIC_MAP
  return tex2D(_MetallicMap, i.uv.xy).r;
  #else
  return _Metallic;
  #endif
}

// Ditto for Smoothness - stored in metallic map's ALPHA
float GetSmoothness(Interpolators i) {
  float smoothness = 1;

  #if defined(_SMOOTHNESS_ALBEDO)
  smoothness = tex2D(_MainTex, i.uv.xy).a; // Smoothness from albedo alpha?
  #elif defined(_SMOOTHNESS_METALLIC) && defined(_METALLIC_MAP)
  return tex2D(_MetallicMap, i.uv.xy).a * _Glossiness;
  #endif

  return smoothness * _Glossiness;
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
UnityIndirect CreateIndirect (Interpolators i, float3 viewDirection) {
  UnityIndirect ind;
  ind.diffuse = 0;
  ind.specular = 0;

  #if defined(VERTEXLIGHT_ON)
  ind.diffuse = i.vertexLightColor; // Indirect lighting will be vertex lit
  #endif

  #ifdef FORWARD_BASE_PASS
  ind.diffuse += max(0, ShadeSH9(float4(i.wNormal, 1))); // Spherical Harmonics for weak light approximation in our base pass
  float3 reflectionDir = reflect(-viewDirection, i.wNormal); // Sample based on where we are looking from

  // Sample Reflections
  Unity_GlossyEnvironmentData data;
  data.roughness = 1 - GetSmoothness(i);
  data.reflUVW = BoxProjection(reflectionDir, i.wPos,
    unity_SpecCube0_ProbePosition,
    unity_SpecCube0_BoxMin, unity_SpecCube0_BoxMax); // Use box projection to determine proper reflection, probe 0
  float3 probe0 = Unity_GlossyEnvironment(
    UNITY_PASS_TEXCUBE(unity_SpecCube0), unity_SpecCube0_HDR, data
    ); // Sample primary reflection probe

  // Is reflection blending supported?
    #if UNITY_SPECCUBE_BLENDING

    // Is sampling the second probe worth it?
    float t = unity_SpecCube0_BoxMin.w;
    UNITY_BRANCH
    if (t < 0.99999) {
      data.reflUVW = BoxProjection(reflectionDir, i.wPos,
        unity_SpecCube1_ProbePosition,
        unity_SpecCube1_BoxMin, unity_SpecCube1_BoxMax); // Use box projection to determine proper reflection, probe 1
      float3 probe1 = Unity_GlossyEnvironment(
        UNITY_PASS_TEXCUBE_SAMPLER(unity_SpecCube1, unity_SpecCube0), unity_SpecCube0_HDR, data
        ); // Sample secondary reflection probe

      // Blend both probe samples - amount is stored in probe 0's BoxMin.w
      ind.specular = lerp(probe1, probe0, t);
    } else {
      ind.specular = probe0;
    }
    #else
    ind.specular = probe0;
    #endif

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

  v2f.pos = UnityObjectToClipPos(v.vertex); // Bult-in Unity object-to-screen projection
  v2f.uv.xy = TRANSFORM_TEX(v.uv, _MainTex); // Built-in sample texture (tex2D) + apply Tile/Offset
  v2f.uv.zw = TRANSFORM_TEX(v.uv, _MainTex);
  v2f.wPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Matrix multiplication using built-in to transform object coordinates to world coordinates
  v2f.wNormal = UnityObjectToWorldNormal(v.normal); // Built-in Unity object-to-world normals
  v2f.wNormal = normalize(v2f.wNormal); // Normalize normals just in case
  v2f.wTangent = float4(UnityObjectToWorldDir(v.tangent.xyz), v.tangent.w); // Object-to-world tangents

  TRANSFER_SHADOW(v2f); // Correctly calculate and transfer shadows

  ComputeVertexLightColor(v, v2f);

  return v2f;
}

// Fragment Program
float4 Frag (Interpolators i) : SV_TARGET // SV_TARGET: shader target color data
{
  FragmentNormal(i); // Calculate the fragment's normal

  half3 viewDirection = -normalize(i.wPos - _WorldSpaceCameraPos); // I legit dunno why that minus is needed but light would be ded otherwise

  // we'll need the light info before PBS
  UnityLight light = CreateLight(i);

  #ifdef FORWARD_BASE_PASS
    // base pass renders the fake color
    float3 albedo = _HiddenColor;
  #else
    // additive pass reveals shit
    // get light intensity
    float br = max3(light.color.rgb);
    // cache light's original color
    float4 rimColor = float4(light.color, 1);
    // change light to white to not tint the revealed stuff
    light.color = float3(1, 1, 1);

    // above threshold, full alpha; below, we fade out
    float alpha = br > _LightThreshold ? 1 : br/_LightThreshold;
    // make alpha zero for faces facing away (reduces artifacts)
    alpha = light.ndotl > 0 ? alpha : 0;
    float3 albedo = tex2D(_MainTex, i.uv.xy).rgb*_Color.rgb; // Sample texture and apply tint
  #endif

  float3 specularTint;
  float oneMinusReflectivity;
  // Derive final albedo from diffuse color + metallic property
  albedo = DiffuseAndSpecularFromMetallic(
    albedo, GetMetallic(i), specularTint, oneMinusReflectivity
    );

  // Unity built-in physics magic for Physically Based Lighting
  float4 col = UNITY_BRDF_PBS(
      albedo, specularTint,   // albedo, specularTint
      oneMinusReflectivity, GetSmoothness(i), // oneMinusReflectivity, 1 - roughness
      i.wNormal, viewDirection,   // normal, view direction
      light, CreateIndirect(i, viewDirection)             // direct light, indirect light
    );

  // our additive pass does some magic!
  #ifndef FORWARD_BASE_PASS
    // determine rim color blending from alpha
    float blend = max(alpha - 0.5, 0)*2;
    // lerp revealed color and rim color
    col = alpha < 1 ? lerp(rimColor, col, pow(blend, 3)) : col;
    // fade our special effect!
    col.a = alpha*alpha;
  #endif

  return col;
}

#endif
