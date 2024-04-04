Shader "Custom/SimpleDepth"
{
     Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FogColor ("Fog Color", Color) = (0.5, 0.5, 0.5, 1)
        _FogDensity ("Fog Density", Range(0, 1)) = 0.1
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float depth : TEXCOORD0;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.depth = o.vertex.z; // Pass depth as texture coordinate
                return o;
            }

            sampler2D _MainTex;
            fixed4 _FogColor;
            float _FogDensity;

            fixed4 frag (v2f i) : SV_Target
            {
                // Encode depth as grayscale color
                float depth = i.depth / i.vertex.w; // Linearize depth

                // Apply fog effect
                float fogFactor = exp(-_FogDensity * _FogDensity * i.depth * i.depth);
                fixed4 fogColor = _FogColor * fogFactor;

                // Mix fog color with depth color
                fixed4 depthColor = float4(depth, depth, depth, 1);
                return lerp(fogColor, depthColor, fogFactor);
            }
            ENDCG
        }
    }
}
