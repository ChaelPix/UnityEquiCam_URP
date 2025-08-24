// Shader: Hidden/Bodhi Donselaar/EquiCam
Shader "Hidden/Bodhi Donselaar/EquiCam"
{
    Properties
    {
        // cubemap will be set by the script
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" "RenderPipeline"="UniversalPipeline" }
        LOD 100
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS   : POSITION;
                float2 uv           : TEXCOORD0;
            };

            struct Varyings
            {
                float4 positionCS   : SV_POSITION;
                float2 uv           : TEXCOORD0;
            };

            float FORWARD;

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = (IN.uv - 0.5) * float2(TWO_PI, PI) + float2(FORWARD, 0);
                return OUT;
            }

            TEXTURECUBE(_MainTex);
            SAMPLER(sampler_MainTex);

            half4 frag (Varyings IN) : SV_Target
            {
                float cy = cos(IN.uv.y);
                float3 dir = float3(sin(IN.uv.x) * cy, sin(IN.uv.y), cos(IN.uv.x) * cy);
                
                return SAMPLE_TEXTURECUBE(_MainTex, sampler_MainTex, dir);
            }
            ENDHLSL
        }
    }
}