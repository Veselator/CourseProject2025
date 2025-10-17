Shader "Custom/BoxDisappearingShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _Value ("Dissolve Value", Range(0, 1)) = 0
        _BurnSize ("Burn Edge Size", Range(0, 0.3)) = 0.1
        _BurnColor ("Burn Color", Color) = (1, 0.5, 0, 1)
        _BurnColorIntensity ("Burn Intensity", Range(0, 5)) = 2
    }
    
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off
        
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
            };
            
            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
            };
            
            sampler2D _MainTex;
            sampler2D _NoiseTex;
            float _Value;
            float _BurnSize;
            fixed4 _BurnColor;
            float _BurnColorIntensity;
            
            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                float noise = tex2D(_NoiseTex, i.uv).r;
                
                // Порог исчезновения
                float dissolveThreshold = _Value * 1.2 - 0.1;
                float edge = dissolveThreshold + _BurnSize;
                
                // Полностью исчезшие пиксели
                if (noise < dissolveThreshold)
                {
                    discard;
                }
                
                // Край выгорания
                if (noise < edge)
                {
                    float burnLerp = (edge - noise) / _BurnSize;
                    col.rgb = lerp(col.rgb, _BurnColor.rgb * _BurnColorIntensity, burnLerp);
                }
                
                return col;
            }
            ENDCG
        }
    }
}
