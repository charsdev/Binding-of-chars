Shader "Custom/BrightnessShader" {
    Properties{
        _MainTex("Sprite Texture", 2D) = "white" {}
        _Color("Tint Color", Color) = (1, 1, 1, 1)
       _Brightness("Brightness", Range(0, 2)) = 1.0
    }

    SubShader{
        Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
        LOD 200

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        Cull Off

        Pass {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float _Brightness;
            fixed4 _Color;

            v2f vert(appdata v) {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target {
                fixed4 color = tex2D(_MainTex, i.uv);
                color.rgb += (1.0 - color.rgb) * _Brightness * _Color.rgb;
                color.a *= _Color.a;
                return color;
            }
            ENDCG
        }
    }
}
