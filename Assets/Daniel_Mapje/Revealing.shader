Shader "Custom/RevealingShader"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _LightPosition ("Light Position", Vector) = (0, 0, 0, 0)
        _LightDirection ("Light Direction", Vector) = (0, 0, 1, 0)
        _LightAngle ("Light Angle", Range(0, 90)) = 45
        _StrengthScale ("Strength Scale", Float) = 50
    }
    SubShader
    {
        // Belangrijk: Zet de queue op Transparent zodat alpha-fading werkt
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows alpha:fade
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldPos;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float4 _LightPosition;
        float4 _LightDirection;
        float _LightAngle;
        float _StrengthScale;

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // 1. Richting van pixel naar de lichtbron
            float3 lightDir = normalize(_LightPosition.xyz - IN.worldPos);
            float3 spotDir = normalize(_LightDirection.xyz);

            // 2. Dot product om te kijken hoe goed de pixel in lijn ligt met de zaklamp
            // We gebruiken -lightDir omdat de vector vanaf de pixel naar de lamp wijst
            float scale = dot(-lightDir, spotDir);

            // 3. Omzetten van hoek naar cosinus (radialen)
            float cosAngle = cos(radians(_LightAngle));

            // 4. Sterkte berekenen en schalen
            float strength = saturate((scale - cosAngle) / (1.0 - cosAngle)) * _StrengthScale;

            // Albedo en Kleur
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            
            o.Albedo = c.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            
            // Alpha wordt aangestuurd door de zaklamp-straal
            o.Alpha = saturate(strength);
        }
        ENDCG
    }
    FallBack "Diffuse"
}