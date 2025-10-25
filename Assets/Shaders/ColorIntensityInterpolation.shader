Shader "Custom/ColorIntensityInterpolation"
{
    Properties
    {
        _ColorIn ("Color In", Color) = (1, 0, 0, 1)
        _IntensityIn ("Intensity In", Range(0, 5)) = 1.0
        _ColorOut ("Color Out", Color) = (0, 0, 1, 1)
        _IntensityOut ("Intensity Out", Range(0, 5)) = 1.0
        _InterpolationValue ("Interpolation Value", Range(0, 1)) = 0.5
        
        [Header(Reveal Effect)]
        [Toggle] _UseRevealEffect ("Use Reveal Effect", Float) = 1
        _RevealEdgeWidth ("Reveal Edge Width", Range(0, 0.5)) = 0.1
        _RevealEdgeColor ("Reveal Edge Color", Color) = (0.5, 0.8, 1, 1)
        _RevealEdgeIntensity ("Reveal Edge Intensity", Range(0, 5)) = 2
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
            #pragma multi_compile _ _USEREVEALEFFECT_ON
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
                float3 worldPos : TEXCOORD1;
            };
            
            // Оригинальные свойства
            fixed4 _ColorIn;
            float _IntensityIn;
            fixed4 _ColorOut;
            float _IntensityOut;
            float _InterpolationValue;
            
            // Reveal эффект свойства
            float _UseRevealEffect;
            float _RevealEdgeWidth;
            fixed4 _RevealEdgeColor;
            float _RevealEdgeIntensity;
            
            // Глобальные параметры (устанавливаются через PuzzlesVisibilityManager)
            float _PuzzleRevealProgress;
            float4 _PuzzleRevealCenter;
            float _PuzzleWaveSpeed;
            float _PuzzleNoiseScale;
            int _PuzzleEffectType;
            
            // Простая noise функция
            float hash(float2 p)
            {
                return frac(sin(dot(p, float2(127.1, 311.7))) * 43758.5453);
            }
            
            float noise(float2 p)
            {
                float2 i = floor(p);
                float2 f = frac(p);
                f = f * f * (3.0 - 2.0 * f);
                
                float a = hash(i);
                float b = hash(i + float2(1.0, 0.0));
                float c = hash(i + float2(0.0, 1.0));
                float d = hash(i + float2(1.0, 1.0));
                
                return lerp(lerp(a, b, f.x), lerp(c, d, f.x), f.y);
            }
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Оригинальный код
                fixed4 color = lerp(_ColorIn, _ColorOut, _InterpolationValue);
                float intensity = lerp(_IntensityIn, _IntensityOut, _InterpolationValue);
                color.rgb *= intensity;
                
                #ifdef _USEREVEALEFFECT_ON
                if (_UseRevealEffect > 0.5)
                {
                    float revealMask = 1.0;
                    float edgeFactor = 0.0;
                    
                    // Эффект 0: Fade
                    if (_PuzzleEffectType == 0)
                    {
                        revealMask = _PuzzleRevealProgress;
                    }
                    // Эффект 1: Radial Wave
                    else if (_PuzzleEffectType == 1)
                    {
                        float dist = distance(i.worldPos.xy, _PuzzleRevealCenter.xy);
                        float waveProgress = _PuzzleRevealProgress * 15.0; // Радиус волны
                        
                        revealMask = smoothstep(waveProgress - 1.0, waveProgress + 0.5, dist);
                        revealMask = 1.0 - revealMask;
                        
                        // Edge glow
                        float edge = abs(dist - waveProgress);
                        edgeFactor = smoothstep(1.0, 0.0, edge) * _PuzzleRevealProgress;
                    }
                    // Эффект 2: Pixel Dissolve
                    else if (_PuzzleEffectType == 2)
                    {
                        float n = noise(i.worldPos.xy * _PuzzleNoiseScale);
                        revealMask = smoothstep(_PuzzleRevealProgress - 0.1, _PuzzleRevealProgress + 0.1, n);
                        
                        // Edge glow
                        float edge = abs(n - _PuzzleRevealProgress);
                        edgeFactor = smoothstep(0.15, 0.0, edge) * _PuzzleRevealProgress;
                    }
                    // Эффект 3: From Bottom
                    else if (_PuzzleEffectType == 3)
                    {
                        float worldY = i.worldPos.y;
                        float centerY = _PuzzleRevealCenter.y;
                        float waveHeight = centerY + (_PuzzleRevealProgress - 0.5) * 20.0;
                        
                        revealMask = smoothstep(waveHeight - 1.0, waveHeight + 0.5, worldY);
                        
                        // Edge glow
                        float edge = abs(worldY - waveHeight);
                        edgeFactor = smoothstep(1.0, 0.0, edge) * _PuzzleRevealProgress;
                    }
                    
                    // Применяем маску
                    color.a *= revealMask;
                    
                    // Добавляем свечение на краю
                    if (edgeFactor > 0.01)
                    {
                        fixed4 edgeGlow = _RevealEdgeColor * _RevealEdgeIntensity * edgeFactor;
                        color.rgb += edgeGlow.rgb;
                    }
                }
                #endif
                
                return color;
            }
            ENDCG
        }
    }
    
    FallBack "Sprites/Default"
}