Shader "Custom/MinoCubeFresnel"
{
    Properties
    {
        _fresnelIntensity ("Fresnel", Range(0.0, 5)) = 1.0
        _thresh ("Threshold", Range(0.0, 1.0)) = 0
        _color ("MainColor", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        //Tags { "RenderType"="Opaque" }
        Pass
        {
            Cull Off
            ZWrite Off
            Blend One One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            float _fresnelIntensity, _thresh;
            float3 _color;
            struct meshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 normal : NORMAL;
                float3 wPos : TEXCOORD1;
                float3 wNor : TEXCOORD2;
            };

            v2f vert (meshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);//mvp matrix
                o.uv = v.uv;
                o.normal = v.normal;
                o.wPos = mul (unity_ObjectToWorld, v.vertex);
                o.wNor = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                float3 N = normalize(i.wNor);
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float fresnel =  pow(1 - abs(dot(N, V)), _fresnelIntensity);
                fresnel *= (fresnel > _thresh);
                fixed4 col = fixed4(_color * fresnel, 0);
                return col;
            }
            ENDCG
        }
    }
}
