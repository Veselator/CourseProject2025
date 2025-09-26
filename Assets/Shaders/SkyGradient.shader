Shader "Unlit/SkyGradient2D"
{
    Properties
    {
        [Header(Colors)]
        _TopColor ("Top Color", Color) = (0.4, 0.7, 1.0, 1.0)
        _MiddleColor ("Middle Color", Color) = (0.8, 0.9, 1.0, 1.0)
        _BottomColor ("Bottom Color", Color) = (1.0, 0.95, 0.8, 1.0)
        
        [Header(Gradient Settings)]
        _MiddlePosition ("Middle Position", Range(0, 1)) = 0.5
        _TopBlend ("Top Blend", Range(0, 1)) = 0.3
        _BottomBlend ("Bottom Blend", Range(0, 1)) = 0.3
        
        [Header(Additional Effects)]
        _Brightness ("Brightness", Range(0, 2)) = 1.0
        _Contrast ("Contrast", Range(0, 2)) = 1.0
        _Saturation ("Saturation", Range(0, 2)) = 1.0
        
        [Header(Animation)]
        _ScrollSpeed ("Scroll Speed", Range(-2, 2)) = 0.0
        _WaveAmplitude ("Wave Amplitude", Range(0, 0.1)) = 0.0
        _WaveFrequency ("Wave Frequency", Range(0, 10)) = 1.0
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Background"
            "IgnoreProjector"="True"
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
            
            // Properties
            fixed4 _TopColor;
            fixed4 _MiddleColor;
            fixed4 _BottomColor;
            
            float _MiddlePosition;
            float _TopBlend;
            float _BottomBlend;
            
            float _Brightness;
            float _Contrast;
            float _Saturation;
            
            float _ScrollSpeed;
            float _WaveAmplitude;
            float _WaveFrequency;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            // Функция для регулировки насыщенности
            fixed3 adjustSaturation(fixed3 color, float saturation)
            {
                fixed3 gray = dot(color, fixed3(0.299, 0.587, 0.114));
                return lerp(gray, color, saturation);
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Получаем UV координаты с учётом скроллинга
                float2 uv = i.uv;
                uv.y += _Time.y * _ScrollSpeed;
                
                // Добавляем волновой эффект
                if (_WaveAmplitude > 0)
                {
                    float wave = sin(uv.y * _WaveFrequency + _Time.y * 2.0) * _WaveAmplitude;
                    uv.x += wave;
                }
                
                // Нормализуем Y координату (0 = низ, 1 = верх)
                float gradientPos = uv.y;
                
                fixed4 finalColor;
                
                // Создаём трёхцветный градиент
                if (gradientPos <= _MiddlePosition)
                {
                    // Нижняя часть: от Bottom к Middle
                    float t = gradientPos / _MiddlePosition;
                    t = smoothstep(0, _BottomBlend, t);
                    finalColor = lerp(_BottomColor, _MiddleColor, t);
                }
                else
                {
                    // Верхняя часть: от Middle к Top
                    float t = (gradientPos - _MiddlePosition) / (1.0 - _MiddlePosition);
                    t = smoothstep(1.0 - _TopBlend, 1.0, t);
                    finalColor = lerp(_MiddleColor, _TopColor, t);
                }
                
                // Применяем эффекты
                finalColor.rgb = adjustSaturation(finalColor.rgb, _Saturation);
                finalColor.rgb = ((finalColor.rgb - 0.5) * _Contrast + 0.5) * _Brightness;
                
                // Ограничиваем значения
                finalColor.rgb = saturate(finalColor.rgb);
                
                return finalColor;
            }
            ENDCG
        }
    }
    
    Fallback "Sprites/Default"
}