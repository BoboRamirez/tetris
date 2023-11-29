Shader "Custom/MinoCubeFresnelGPU"
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
            #pragma multi_compile_instancing	
            #include "UnityCG.cginc"
            //float _fresnelIntensity, _thresh;
            //float3 _color;
            struct meshData
            {
                float4 vertex : POSITION;
                float4 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float4 normal : NORMAL;
                float3 wPos : TEXCOORD1;
                float3 wNor : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

             UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _color)
                UNITY_DEFINE_INSTANCED_PROP(float, _thresh)
                UNITY_DEFINE_INSTANCED_PROP(float, _fresnelIntensity)
            UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (meshData v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                o.vertex = UnityObjectToClipPos(v.vertex);//mvp matrix
                o.normal = v.normal;
                o.wPos = mul (unity_ObjectToWorld, v.vertex);
                o.wNor = UnityObjectToWorldNormal(v.normal);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                UNITY_SETUP_INSTANCE_ID(i);
                float3 N = normalize(i.wNor);
                float3 V = normalize(_WorldSpaceCameraPos - i.wPos);
                float fresnel =  pow(1 - abs(dot(N, V)), UNITY_ACCESS_INSTANCED_PROP(Props, _fresnelIntensity));
                fresnel *= (fresnel > UNITY_ACCESS_INSTANCED_PROP(Props, _thresh));
                fixed4 col = fixed4(UNITY_ACCESS_INSTANCED_PROP(Props, _color) * fresnel);
                return col;
            }
            ENDCG
        }
    }
}
