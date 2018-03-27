#if !defined(SHADOWS_INC)
#define SHADOWS_INC

#include "UnityCG.cginc"

struct VData {
  float4 vertex : POSITION;
  float3 normal : NORMAL;
};

struct Interpolators {
  float4 position : SV_POSITION;
  #ifdef SHADOWS_CUBE
    float3 lightVec : TEXCOORD0;
  #endif
};

Interpolators ShadowVert(VData v) {
  Interpolators i;

  #ifdef SHADOWS_CUBE
    i.position = UnityObjectToClipPos(v.vertex);
    i.lightVec = mul(unity_ObjectToWorld, v.vertex).xyz - _LightPositionRange.xyz;
  #else
    i.position = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal); // Apply normal bias to reduce artifacts
    i.position = UnityApplyLinearShadowBias(i.position); // Apply linear bias to reduce artifacts
  #endif

  return i;
}

float ShadowFrag(Interpolators i) : SV_TARGET {
  #ifdef SHADOWS_CUBE
    float depth = length(i.lightVec) + unity_LightShadowBias.x;
    depth *= _LightPositionRange.w;
    return UnityEncodeCubeShadowDepth(depth);
  #else
    return 0;
  #endif
}

#endif
