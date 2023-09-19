Shader "Custom/TextureOverlayShader" {
	Properties{
		_BaseTex("Base Texture", 2D) = "white" {}
		_OverlayTex1("Overlay Texture 1", 2D) = "white" {}
		_OverlayTex2("Overlay Texture 2", 2D) = "white" {}
		_OverlayTex3("Overlay Texture 3", 2D) = "white" {}
		_Overlay1Offset("Overlay 1 Offset", Vector) = (0, 0, 0, 0)
		_Overlay2Offset("Overlay 2 Offset", Vector) = (0, 0, 0, 0)
		_Overlay3Offset("Overlay 3 Offset", Vector) = (0, 0, 0, 0)
		_Overlay1Scale("Overlay 1 Scale", Vector) = (1, 1, 1, 1)
		_Overlay2Scale("Overlay 2 Scale", Vector) = (1, 1, 1, 1)
		_Overlay3Scale("Overlay 3 Scale", Vector) = (1, 1, 1, 1)
		_NormalMap1("Normal Map 1", 2D) = "bump" {}
		_NormalMap2("Normal Map 2", 2D) = "bump" {}
		_NormalMap3("Normal Map 3", 2D) = "bump" {}
		_OverlayAlpha("Overlay Alpha", Float) = 1.0
	}

		SubShader{
			Tags { "RenderType" = "Opaque" }
			LOD 200

			Pass {
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag
				#pragma target 3.0

				#include "UnityCG.cginc"

				sampler2D _BaseTex;
				sampler2D _OverlayTex1;
				sampler2D _OverlayTex2;
				sampler2D _OverlayTex3;
				sampler2D _NormalMap1;
				sampler2D _NormalMap2;
				sampler2D _NormalMap3;

				float4 _Overlay1Offset;
				float4 _Overlay2Offset;
				float4 _Overlay3Offset;

				float _OverlayAlpha;

				float4  _Overlay1Scale;
				float4  _Overlay2Scale;
				float4  _Overlay3Scale;

				struct appdata {
					float4 vertex : POSITION;
					float2 uv_BaseTex : TEXCOORD0;
					float2 uv_OverlayTex1 : TEXCOORD1;
					float2 uv_OverlayTex2 : TEXCOORD2;
					float2 uv_OverlayTex3 : TEXCOORD3;
					float2 uv_NormalMap1 : TEXCOORD4;
					float2 uv_NormalMap2 : TEXCOORD5;
					float2 uv_NormalMap3 : TEXCOORD6;
				};

				struct v2f {
					float2 uv_BaseTex : TEXCOORD0;
					float2 uv_OverlayTex1 : TEXCOORD1;
					float2 uv_OverlayTex2 : TEXCOORD2;
					float2 uv_OverlayTex3 : TEXCOORD3;
					float2 uv_NormalMap1 : TEXCOORD4;
					float2 uv_NormalMap2 : TEXCOORD5;
					float2 uv_NormalMap3 : TEXCOORD6;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v) {
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv_BaseTex = v.uv_BaseTex;
					o.uv_OverlayTex1 = v.uv_OverlayTex1;
					o.uv_OverlayTex2 = v.uv_OverlayTex2;
					o.uv_OverlayTex3 = v.uv_OverlayTex3;
					o.uv_NormalMap1 = v.uv_NormalMap1;
					o.uv_NormalMap2 = v.uv_NormalMap2;
					o.uv_NormalMap3 = v.uv_NormalMap3;
					return o;
				}

				half4 frag(v2f IN) : SV_Target{
					// Retrieve base color and normal
					fixed4 baseColor = tex2D(_BaseTex, IN.uv_BaseTex);
					fixed3 baseNormal = UnpackNormal(tex2D(_NormalMap1, IN.uv_NormalMap1)).xyz;

					// Apply offsets and scales to overlay texture UV coordinates
					float2 overlayUV1 = (IN.uv_OverlayTex1 + _Overlay1Offset.xy) * _Overlay1Scale.xy;
					float2 overlayUV2 = (IN.uv_OverlayTex2 + _Overlay2Offset.xy) * _Overlay2Scale.xy;
					float2 overlayUV3 = (IN.uv_OverlayTex3 + _Overlay3Offset.xy) * _Overlay3Scale.xy;

					// Retrieve overlay colors
					fixed4 overlayColor1 = tex2D(_OverlayTex1, overlayUV1);
					fixed4 overlayColor2 = tex2D(_OverlayTex2, overlayUV2);
					fixed4 overlayColor3 = tex2D(_OverlayTex3, overlayUV3);

					// Retrieve normals
					fixed3 overlayNormal1 = UnpackNormal(tex2D(_NormalMap2, IN.uv_NormalMap2));
					fixed3 overlayNormal2 = UnpackNormal(tex2D(_NormalMap3, IN.uv_NormalMap3));
					fixed3 overlayNormal3 = baseNormal;

					// Apply alpha
					overlayColor1.a *= _OverlayAlpha;
					overlayColor2.a *= _OverlayAlpha;
					overlayColor3.a *= _OverlayAlpha;

					// Calculate multiply factors
					fixed4 multiplyFactors = (1 - overlayColor1.a * _OverlayAlpha) *
						(1 - overlayColor2.a * _OverlayAlpha) *
						(1 - overlayColor3.a * _OverlayAlpha);

					// Combine colors and normals with blending
					fixed4 finalColor = baseColor * multiplyFactors +
						(overlayColor1 * overlayColor1.a * _OverlayAlpha) +
						(overlayColor2 * overlayColor2.a * _OverlayAlpha) +
						(overlayColor3 * overlayColor3.a * _OverlayAlpha);

					fixed3 finalNormal = baseNormal +
						(overlayNormal1 * overlayColor1.a * _OverlayAlpha) +
						(overlayNormal2 * overlayColor2.a * _OverlayAlpha) +
						(overlayNormal3 * overlayColor3.a * _OverlayAlpha);

					return half4(finalColor.rgb, finalColor.a);
				}
				ENDCG
			}
		}

	Fallback "Diffuse"
}
