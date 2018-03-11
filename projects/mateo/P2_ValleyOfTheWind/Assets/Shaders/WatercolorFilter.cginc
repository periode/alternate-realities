//CGINC

#if !defined(WC_FILTER_INC)
#define WC_FILTER_INC

#include "UnityPBSLighting.cginc" // Physical based lighting stuffz
#include "AutoLight.cginc" // Makes some light stuffs easier

// We redeclare our properties inside our CG program
sampler2D _MainTex;//, _DetailTex; // Access texture
float4 _MainTex_ST;//, _DetailTex_ST; // Access texture tiling + offset
float4 _MainTex_TexelSize;

sampler2D _Watercolor;
float4 _Watercolor_ST;
float4 _Watercolor_TexelSize;

float _Strength;
int _Radius;

// Struct to handle Vertex Data
struct VData {
  float4 vertex : POSITION; // POSITION: Raw position input data
  float4 uv : TEXCOORD0; // TEXCOORD0: UV coordinates / Main texture
};

// Structure to Interpolate between Vertex and Fragment programs
struct Interpolators {
  float4 pos : SV_POSITION; // SV_POSITION: Math'd screen position interpolator
  float4 uv : TEXCOORD0;
  float3 wPos : TEXCOORD1; // World Space Position
  float4 screenPos : TEXCOORD2;
};

// Vertex Program
Interpolators Vert (VData v) {
  Interpolators v2f;

  v2f.pos = UnityObjectToClipPos(v.vertex); // Bult-in Unity object-to-screen projection
  v2f.uv.xy = TRANSFORM_TEX(v.uv, _MainTex); // Built-in sample texture (tex2D) + apply Tile/Offset
  v2f.uv.zw = TRANSFORM_TEX(v.uv, _Watercolor);
  v2f.wPos = mul(unity_ObjectToWorld, v.vertex).xyz; // Matrix multiplication using built-in to transform object coordinates to world coordinates

  return v2f;
}

struct Region {
  int2 v1, v2;
};

float3 WaterColor(float2 uv1, float2 uv2) {
  float3 col = tex2D(_MainTex, uv1).rgb;
  float mult = tex2D(_Watercolor, uv2).r;// - 0.5;
  mult *= _Strength;
  col += mult;

  return col;
}

// Fragment Program
float4 Frag (Interpolators i) : SV_TARGET // SV_TARGET: shader target color data
{
  float3 albedo = WaterColor(i.uv.xy, i.uv.zw);

  float4 uv = i.uv;
  float n = float((_Radius + 1) * (_Radius + 1));

  float3 m[4];
  float3 s[4];

  for (int k = 0; k < 4; ++k) {
    m[k] = float3(0, 0, 0);
    s[k] = float3(0, 0 ,0);
  }

  Region r[4] = {
    {-int2(_Radius, _Radius), int2(      0,       0)},
    {-int2(      0, _Radius), int2(_Radius,       0)},
    { int2(      0,       0), int2(_Radius, _Radius)},
    {-int2(_Radius,       0), int2(      0, _Radius)}
  };

  for (int k = 0; k < 4; ++k) {
    for (int j = r[k].v1.y; j <= r[k].v2.y; ++j) {
      for (int i = r[k].v1.x; i <= r[k].v2.x; ++i) {
        float2 delta1 = float2(i*_MainTex_TexelSize.x, j*_MainTex_TexelSize.y);
        float2 delta2 = float2(i*_Watercolor_TexelSize.x, j*_Watercolor_TexelSize.y);
        float3 c = WaterColor(uv.xy + delta1, uv.zw + delta2);

        // tex2D(_MainTex, uv + (float2(i*_MainTex_TexelSize.x, j*_MainTex_TexelSize.y))).rgb;
        m[k] += c;
        s[k] += c * c;
      }
    }
  }

  float min = 100;
  float s2;
  for (k = 0; k < 4; ++k) {
    m[k] /= n;
    s[k] = abs(s[k] / n - m[k] * m[k]);

    s2 = s[k].r + s[k].g + s[k].b;
    if (s2 < min) {
      min = s2;
      albedo.rgb = m[k].rgb;
    }
  }

  return float4(albedo, 1);
}

#endif
