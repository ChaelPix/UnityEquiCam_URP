// Shader: Bodhi Donselaar/TinyPlanet
Shader "Bodhi Donselaar/TinyPlanet"
{
    Properties
    {
        _Angle("Angle", Range(0, 360)) = 180
        _Pow("Pow", Range(0, 4)) = 1
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

            Varyings vert (Attributes IN)
            {
                Varyings OUT;
                OUT.positionCS = TransformObjectToHClip(IN.positionOS.xyz);
                OUT.uv = (IN.uv - 0.5);
                OUT.uv.x *= _ScreenParams.x / _ScreenParams.y;
                return OUT;
            }

            TEXTURECUBE(_MainTex);
            SAMPLER(sampler_MainTex);

            half _Angle, _Pow;

            half4 frag (Varyings IN) : SV_Target
            {
                float angle = _Angle * (PI / 180.0);
                
                float y = -1 + distance(IN.uv, float2(0,0)) * 2.0;
                float x = atan2(IN.uv.x, IN.uv.y);

                float3 dir = normalize(float3(cos(x) * (1-y*y), y, sin(x) * (1-y*y)));
                
                return SAMPLE_TEXTURECUBE(_MainTex, sampler_MainTex, dir);
            }
            ENDHLSL
        }
    }
}