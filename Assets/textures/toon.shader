Shader "Custom/ToonShader"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Main Color", Color) = (1,1,1,1)
        _OutlineColor("Outline Color", Color) = (0,0,0,1)
        _Outline("Outline Width", Range(0.0, 0.1)) = 0.05
    }

        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 100

            CGPROGRAM
            #pragma surface surf Lambert

            struct Input
            {
                float2 uv_MainTex;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _OutlineColor;
            half _Outline;

            void surf(Input IN, inout SurfaceOutput o)
            {
                fixed4 texColor = tex2D(_MainTex, IN.uv_MainTex) * _Color;
                o.Albedo = texColor.rgb;
                o.Alpha = texColor.a;

                // Example: Apply a toon/cell shading effect
                float lighting = dot(normalize(float3(0.5, 0.5, 1)), o.Normal); // Example light direction
                o.Albedo *= smoothstep(0.2, 1, lighting);

                // Example: Add a rim light effect for outlining
                float rim = 1 - saturate(dot(normalize(float3(0, 0, 1)), o.Normal));
                o.Emission = rim * _OutlineColor.rgb * _Outline;
            }
            ENDCG
        }

            Fallback "Diffuse"
}
