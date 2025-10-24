Shader "Custom/LaserPlatformShader"
{
    Properties
    {
        _EdgeColor ("Edge Color", Color) = (1, 0, 0, 1)
        _CenterColor ("Center Color", Color) = (1, 1, 1, 1)
        _EdgeWidth ("Edge Width", Range(0, 0.5)) = 0.3
        _Smoothness ("Smoothness", Range(0.01, 0.5)) = 0.1
        _Brightness ("Brightness", Range(0, 5)) = 1.5
        
        // Параметры анимации
        _ScrollSpeed ("Scroll Speed", Float) = 1.0
        _PulseSpeed ("Pulse Speed", Float) = 2.0
        _PulseAmount ("Pulse Amount", Range(0, 1)) = 0.3
        _NoiseScale ("Noise Scale", Float) = 10.0
        _NoiseAmount ("Noise Amount", Range(0, 0.5)) = 0.1
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
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
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            fixed4 _EdgeColor;
            fixed4 _CenterColor;
            float _EdgeWidth;
            float _Smoothness;
            float _Brightness;
            float _ScrollSpeed;
            float _PulseSpeed;
            float _PulseAmount;
            float _NoiseScale;
            float _NoiseAmount;

            // Простая функция шума
            float noise(float2 uv)
            {
                return frac(sin(dot(uv, float2(12.9898, 78.233))) * 43758.5453);
            }

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Анимация прокрутки по горизонтали
                float2 scrolledUV = i.uv;
                scrolledUV.x += _Time.y * _ScrollSpeed;
                
                // Пульсация яркости
                float pulse = sin(_Time.y * _PulseSpeed) * _PulseAmount + 1.0;
                
                // Шум для эффекта "живого" лазера
                float noiseValue = noise(scrolledUV * _NoiseScale);
                float distFromCenter = abs(i.uv.y - 0.5) * 2.0;
                distFromCenter += noiseValue * _NoiseAmount;
                
                // Плавный переход от центра к краям
                float t = smoothstep(_EdgeWidth - _Smoothness, _EdgeWidth + _Smoothness, distFromCenter);
                
                // Смешиваем цвета
                fixed4 color = lerp(_CenterColor, _EdgeColor, t);
                
                // Применяем яркость и пульсацию
                color.rgb *= _Brightness * pulse;
                
                // Затухание на краях для мягкости
                float alpha = 1.0 - smoothstep(0.8, 1.0, distFromCenter);
                color.a *= alpha;
                
                return color;
            }
            ENDCG
        }
    }
    
    FallBack "Sprites/Default"
}