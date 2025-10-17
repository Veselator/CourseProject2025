Shader "Custom/LaserShader"
{
    Properties
    {
        [Header(Core Settings)]
        _CoreColor ("Core Color", Color) = (1, 1, 1, 1)
        _CoreIntensity ("Core Intensity", Range(0, 10)) = 2.0
        _CoreWidth ("Core Width", Range(0, 1)) = 0.5
        
        [Header(Border Settings)]
        _BorderColor ("Border Color", Color) = (0, 0.5, 1, 1)
        _BorderIntensity ("Border Intensity", Range(0, 10)) = 3.0
        _BorderThickness ("Border Thickness", Range(0, 0.5)) = 0.15
        _BorderDistance ("Border Distance from Edge", Range(0, 0.5)) = 0.05
        
        [Header(Bloom Control)]
        _EmissionStrength ("Emission Strength", Range(0, 20)) = 5.0
        _BloomThreshold ("Bloom Threshold", Range(0, 10)) = 1.0
        [Toggle(USE_HDR_COLORS)] _UseHDR ("Use HDR Colors for Bloom", Float) = 1
        
        [Header(Animation)]
        _ScrollSpeed ("Scroll Speed", Range(-5, 5)) = 1.0
        _NoiseScale ("Noise Scale", Range(0, 10)) = 2.0
        _NoiseIntensity ("Noise Intensity", Range(0, 1)) = 0.2
        
        [Header(Texture)]
        _MainTex ("Texture (Optional)", 2D) = "white" {}
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        
        [Header(Blending)]
        _Alpha ("Overall Alpha", Range(0, 1)) = 1.0
        _SoftParticlesFactor ("Soft Particles Factor", Range(0, 10)) = 1.0
    }
    
    SubShader
    {
        Tags { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="Transparent" 
            "PreviewType"="Plane"
        }
        
        Blend SrcAlpha One
        Cull Off 
        ZWrite Off
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #pragma shader_feature SOFTPARTICLES_ON
            #pragma shader_feature USE_HDR_COLORS
            
            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float4 color : COLOR;
                UNITY_FOG_COORDS(1)
                #ifdef SOFTPARTICLES_ON
                float4 projPos : TEXCOORD2;
                #endif
            };
            
            // Properties
            float4 _CoreColor;
            float _CoreIntensity;
            float _CoreWidth;
            
            float4 _BorderColor;
            float _BorderIntensity;
            float _BorderThickness;
            float _BorderDistance;
            
            float _EmissionStrength;
            float _BloomThreshold;
            
            float _ScrollSpeed;
            float _NoiseScale;
            float _NoiseIntensity;
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            sampler2D _NoiseTex;
            
            float _Alpha;
            float _SoftParticlesFactor;
            
            #ifdef SOFTPARTICLES_ON
            sampler2D _CameraDepthTexture;
            #endif
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.color = v.color;
                
                #ifdef SOFTPARTICLES_ON
                o.projPos = ComputeScreenPos(o.vertex);
                COMPUTE_EYEDEPTH(o.projPos.z);
                #endif
                
                UNITY_TRANSFER_FOG(o, o.vertex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Получаем UV координаты с анимацией
                float2 scrolledUV = i.uv;
                scrolledUV.x += _Time.y * _ScrollSpeed;
                
                // Шум для искажения
                float2 noiseUV = scrolledUV * _NoiseScale;
                float noise = tex2D(_NoiseTex, noiseUV).r;
                
                // Расчёт расстояния от центра (для цилиндрического эффекта)
                float distFromCenter = abs(i.uv.y - 0.5) * 2.0;
                
                // Добавляем шум к расстоянию
                distFromCenter = saturate(distFromCenter + (noise - 0.5) * _NoiseIntensity);
                
                // Расчёт зон
                float edgeStart = 1.0 - _BorderDistance;
                float borderStart = edgeStart - _BorderThickness;
                float coreEnd = _CoreWidth;
                
                // Маски для каждой зоны
                float edgeMask = smoothstep(edgeStart, 1.0, distFromCenter);
                float borderMask = smoothstep(borderStart, edgeStart, distFromCenter) * (1.0 - edgeMask);
                float coreMask = 1.0 - smoothstep(0, coreEnd, distFromCenter);
                
                // Цвета и интенсивность
                float4 coreColor = _CoreColor * _CoreIntensity * coreMask;
                float4 borderColor = _BorderColor * _BorderIntensity * borderMask;
                
                // Комбинируем цвета
                float4 finalColor = coreColor + borderColor;
                
                // Опциональная текстура
                float4 mainTex = tex2D(_MainTex, scrolledUV);
                finalColor *= mainTex;
                
                // Затухание к краям
                float edgeFade = 1.0 - smoothstep(0.8, 1.0, distFromCenter);
                finalColor.a *= edgeFade * _Alpha * i.color.a;
                
                // === BLOOM CONTROL ===
                // Увеличиваем яркость для bloom эффекта
                #ifdef USE_HDR_COLORS
                    // HDR режим: умножаем на emission strength чтобы превысить bloom threshold
                    float3 emissiveColor = finalColor.rgb * _EmissionStrength;
                    
                    // Гарантируем что яркость превышает threshold
                    float colorBrightness = max(emissiveColor.r, max(emissiveColor.g, emissiveColor.b));
                    if (colorBrightness < _BloomThreshold)
                    {
                        emissiveColor *= (_BloomThreshold / max(colorBrightness, 0.001));
                    }
                    
                    finalColor.rgb = emissiveColor;
                #else
                    // LDR режим: простое умножение
                    finalColor.rgb *= _EmissionStrength;
                #endif
                
                // Soft particles
                #ifdef SOFTPARTICLES_ON
                float sceneZ = LinearEyeDepth(SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)));
                float partZ = i.projPos.z;
                float fade = saturate(_SoftParticlesFactor * (sceneZ - partZ));
                finalColor.a *= fade;
                #endif
                
                UNITY_APPLY_FOG(i.fogCoord, finalColor);
                
                return finalColor;
            }
            ENDCG
        }
    }
    
    FallBack "Sprites/Default"
}