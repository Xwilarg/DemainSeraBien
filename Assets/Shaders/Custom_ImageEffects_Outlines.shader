Shader "Custom/Image Effects/Outlines"
{
    Properties
    {
        _SampleDistance ("Sample Distance", Range(0, 20)) = 1.0
        _Threshold ("Threshold", Range(0.001,1)) = 0.01
    }
    SubShader
    {
        ZWrite Off ZTest Always

        Pass
        {
            HLSLPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            TEXTURE2D(_SourceTex); SAMPLER(sampler_SourceTex);
            float4 _SourceTex_ST; float4 _SourceTex_TexelSize;

            TEXTURE2D(_ObjectsToOutlineTexture); SAMPLER(sampler_ObjectsToOutlineTexture);
            float4 _ObjectsToOutlineTexture_ST; float4 _ObjectsToOutlineTexture_TexelSize;

            float _SampleDistance;
            float _Threshold;

            struct Attributes
            {
                float4 positionOS : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

                UNITY_VERTEX_OUTPUT_STEREO
            };

            Varyings vert(Attributes input)
            {
                Varyings output = (Varyings)0;
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.vertex = vertexInput.positionCS;
                output.uv = TRANSFORM_TEX(input.uv, _SourceTex);

                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

                half3 albedo = SAMPLE_TEXTURE2D(_SourceTex, sampler_SourceTex, input.uv).rgb;

                half pixel = SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv).r;
                // pixel = step(0.5, pixel);

                // return half4(pixel, pixel, pixel, 1);

                const float2 pixelOffset = float2(_SourceTex_TexelSize.x / _SourceTex_TexelSize.y, 1) * 0.001 * _SampleDistance;

                float neighbours = 0;
                neighbours += SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv + float2(0, pixelOffset.y)).r;
                neighbours += SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv + float2(0, -pixelOffset.y)).r;
                neighbours += SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv + float2(-pixelOffset.x, 0)).r;
                neighbours += SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv + float2(pixelOffset.x, 0)).r;
                neighbours += SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv + float2(pixelOffset.x, pixelOffset.y) * 0.62).r;
                neighbours += SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv + float2(pixelOffset.x, -pixelOffset.y) * 0.62).r;
                neighbours += SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv + float2(-pixelOffset.x, pixelOffset.y) * 0.62).r;
                neighbours += SAMPLE_TEXTURE2D(_ObjectsToOutlineTexture, sampler_ObjectsToOutlineTexture, input.uv + float2(-pixelOffset.x, -pixelOffset.y) * 0.62).r;
                neighbours /= 8;

                const float outlineIntensity = step(_Threshold, length(pixel - neighbours));

                // return half4(outlineIntensity, outlineIntensity, outlineIntensity, 1);

                albedo = lerp(albedo, half3(0,0,0), outlineIntensity);

                return half4(albedo, 1);
            }
            ENDHLSL
        }
    }
}
