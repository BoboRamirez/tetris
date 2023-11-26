Shader "Unlit/BackGroundTest"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}  
    }
    SubShader
    {
        Tags { "RenderType"="Opaque"}
        Pass
        {

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            sampler2D _MainTex;
            float4 _MainTex_ST;
            struct meshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert (meshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            { 
                fixed4 color = tex2D(_MainTex, i.uv);
                return color;
            }
            ENDCG
        }
    }
}
