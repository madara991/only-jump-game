Shader "Custom/UnlitSurfacePLatform"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1, 1, 1, 1)
        _ShadowIntensity("Shadow Intensity", Range(0, 1)) = 0.5
    }
        SubShader
        {
            Tags { "RenderType" = "Opaque" }
            LOD 200

            Pass
            {
                Tags { "LightMode" = "ForwardBase" }

                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma multi_compile_fwdbase
                #include "UnityCG.cginc"
                #include "AutoLight.cginc"

                struct v2f
                {
                    float4 pos : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    SHADOW_COORDS(1)
                };

                sampler2D _MainTex;
                float4 _MainTex_ST;
                fixed4 _Color;
                float _ShadowIntensity;

                v2f vert(appdata_base v)
                {
                    v2f o;
                    o.pos = UnityObjectToClipPos(v.vertex);

                    // Calculate texture coordinates with tiling and offset
                    o.uv = TRANSFORM_TEX(v.texcoord, _MainTex);

                    // Transfer shadow coordinates if needed
                    TRANSFER_SHADOW(o);

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Sample texture and apply color
                    fixed4 col = tex2D(_MainTex, i.uv) * _Color;

                // Apply shadow intensity
                float shadow = SHADOW_ATTENUATION(i);
                col.rgb *= lerp(1.0, shadow, _ShadowIntensity);

                return col;
            }
            ENDCG
        }
        }

            // Fallback to a basic shader if needed
                FallBack "Diffuse"
}
