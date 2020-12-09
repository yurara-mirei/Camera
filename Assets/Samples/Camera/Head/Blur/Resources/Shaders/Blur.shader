Shader "Yurara/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _BlurScale ("BlurScale", float) = 1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        Fog { Mode Off }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 uv1 : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            half4 _MainTex_ST;
            half4 _MainTex_TexelSize;

            float _BlurScale;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);

                o.uv = UnityStereoScreenSpaceUVAdjust(v.uv, _MainTex_ST);

                half2 inc = half2(_MainTex_TexelSize.x * 1.4 * _BlurScale, 0);

                o.uv1 = UnityStereoScreenSpaceUVAdjust(v.uv - inc, _MainTex_ST);
                o.uv2 = UnityStereoScreenSpaceUVAdjust(v.uv + inc, _MainTex_ST);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv) * 0.2 + (tex2D(_MainTex, i.uv1) + tex2D(_MainTex, i.uv2)) * 0.3;
                return col;
            }
            ENDCG
        }
    }
}
