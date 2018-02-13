#if !defined(SHADOWS_INC)
#define SHADOWS_INC

#include "UnityCG.cginc"

struct VData {
  float4 vertex : POSITION;
  float3 normal : NORMAL;
};

float4 ShadowVert(VData v) : SV_POSITION {
  float4 pos = UnityClipSpaceShadowCasterPos(v.vertex.xyz, v.normal); // Apply normal bias to reduce artifacts
  return UnityApplyLinearShadowBias(pos); // Apply linear bias to reduce artifacts
}

float ShadowFrag() : SV_TARGET {
  return 0;
}

#endif
