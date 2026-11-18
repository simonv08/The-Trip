Shader "Custom/URP_RevealingShader"
{
    Properties
    {
        _BaseColor ("Color", Color) = (1,1,1,1)
        _BaseMap ("Albedo (RGB)", 2D) = "white" {}
        _LightPosition ("Light Position", Vector) = (0, 0, 0, 0)
        _LightDirection ("Light Direction", Vector) = (0, 0, 1, 0)
        _LightAngle ("Light Angle", Range(0, 90)) = 45
        _StrengthScale ("Strength Scale", Float) = 50
    }

    SubShader
    {
        Tags 
        { 
            "RenderType" = "Transparent" 
            "Queue" = "Transparent" 
            "RenderPipeline" = "UniversalPipeline" 
        }

        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off

        Pass
        {
            Name "UnlitPass"
            Tags { "LightMode" = "UniversalForward" }

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
                float3 worldPos     : TEXCOORD1;
            };

            Texture2D _BaseMap;
            SamplerState sampler_BaseMap;

            CBUFFER_START(UnityPerMaterial)
                float4 _BaseMap_ST;
                float4 _BaseColor;
                float4 _LightPosition;
                float4 _LightDirection;
                float _LightAngle;
                float _StrengthScale;
            CBUFFER_END

            Varyings vert(Attributes input)
            {
                Varyings output;
                VertexPositionInputs vertexInput = GetVertexPositionInputs(input.positionOS.xyz);
                output.positionCS = vertexInput.positionCS;
                output.worldPos = vertexInput.positionWS;
                output.uv = TRANSFORM_TEX(input.uv, _BaseMap);
                return output;
            }

            half4 frag(Varyings input) : SV_Target
            {
                // 1. Richting van pixel naar de zaklamp
                float3 lightDir = normalize(_LightPosition.xyz - input.worldPos);
                float3 spotDir = normalize(_LightDirection.xyz);

                // 2. Dot product berekenen
                float scale = dot(-lightDir, spotDir);

                // 3. Hoek omzetten naar cosinus (radialen)
                float cosAngle = cos(radians(_LightAngle));

                // 4. Sterkte en transparantie berekenen
                float strength = saturate((scale - cosAngle) / (1.0 - cosAngle)) * _StrengthScale;

                // Texture en kleur ophalen
                half4 texColor = _BaseMap.Sample(sampler_BaseMap, input.uv) * _BaseColor;
                
                // Pas de alpha aan zodat het object alleen zichtbaar wordt in het licht
                texColor.a *= saturate(strength);

                return texColor;
            }
            ENDHLSL
        }
    }
    FallBack "Hidden/Universal Render Pipeline/FallbackError"
}