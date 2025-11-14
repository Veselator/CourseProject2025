Shader "Custom/TextOutline"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _FaceColor ("Face Color", Color) = (1,1,1,1)
        _OutlineColor ("Outline Color", Color) = (0,0,0,1)
        _OutlineWidth ("Outline Width", Range(0, 1)) = 0.2
    }

    SubShader
    {
        Tags 
        { 
            "Queue"="Transparent" 
            "RenderType"="Transparent"
            "IgnoreProjector"="True"
        }

        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _FaceColor;
            float4 _OutlineColor;
            float _OutlineWidth;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float4 texCol = tex2D(_MainTex, i.uv);
                
                float outlineSample = 0;
                float samples = 8;
                float angleStep = 6.283185 / samples;
                
                for(float angle = 0; angle < 6.283185; angle += angleStep)
                {
                    float2 offset = float2(cos(angle), sin(angle)) * _OutlineWidth * 0.01;
                    outlineSample += tex2D(_MainTex, i.uv + offset).a;
                }
                
                outlineSample = saturate(outlineSample);
                
                float4 faceColor = _FaceColor * texCol.a;
                float4 outlineColor = _OutlineColor * outlineSample * (1 - texCol.a);
                
                float4 finalColor = faceColor + outlineColor;
                finalColor *= i.color;
                
                return finalColor;
            }
            ENDCG
        }
    }
}