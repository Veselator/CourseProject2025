Shader "Custom/SpriteLitWithReveal"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        
        [Header(Reveal Effect)]
        [Toggle] _UseRevealEffect ("Use Reveal Effect", Float) = 1
        _RevealEdgeColor ("Reveal Edge Color", Color) = (0.5, 0.8, 1, 1)
        _RevealEdgeIntensity ("Reveal Edge Intensity", Range(0, 5)) = 2
        
        [HideInInspector] _RendererColor ("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip ("Flip", Vector) = (1,1,1,1)
        [HideInInspector] _AlphaTex ("External Alpha", 2D) = "white" {}
        [HideInInspector] _EnableExternalAlpha ("Enable External Alpha", Float) = 0
    }
    
    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
        }
        
        Cull Off
        Lighting Off
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        
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
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };
            
            struct v2f
            {
                float4 vertex : SV_POSITION;
                fixed4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float3 worldPos : TEXCOORD1;
            };
            
            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _RendererColor;
            
            // Reveal эффект
            float _UseRevealEffect;
            fixed4 _RevealEdgeColor;
            float _RevealEdgeIntensity;
            
            // Глобальные параметры
            float _PuzzleRevealProgress;
            float4 _PuzzleRevealCenter;
            float _PuzzleNoiseScale;
            int _PuzzleEffectType;
            
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
            
            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                o.color = v.color * _Color * _RendererColor;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                return o;
            }
            
            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 color = tex2D(_MainTex, i.texcoord) * i.color;
                
                #ifdef _USEREVEALEFFECT_ON
                if (_UseRevealEffect > 0.5)
                {
                    float revealMask = 1.0;
                    float edgeFactor = 0.0;
                    
                    if (_PuzzleEffectType == 0)
                    {
                        revealMask = _PuzzleRevealProgress;
                    }
                    else if (_PuzzleEffectType == 1)
                    {
                        float dist = distance(i.worldPos.xy, _PuzzleRevealCenter.xy);
                        float waveProgress = _PuzzleRevealProgress * 15.0;
                        
                        revealMask = smoothstep(waveProgress - 1.0, waveProgress + 0.5, dist);
                        revealMask = 1.0 - revealMask;
                        
                        float edge = abs(dist - waveProgress);
                        edgeFactor = smoothstep(1.0, 0.0, edge) * _PuzzleRevealProgress;
                    }
                    else if (_PuzzleEffectType == 2)
                    {
                        float n = noise(i.worldPos.xy * _PuzzleNoiseScale);
                        revealMask = smoothstep(_PuzzleRevealProgress - 0.1, _PuzzleRevealProgress + 0.1, n);
                        
                        float edge = abs(n - _PuzzleRevealProgress);
                        edgeFactor = smoothstep(0.15, 0.0, edge) * _PuzzleRevealProgress;
                    }
                    else if (_PuzzleEffectType == 3)
                    {
                        float worldY = i.worldPos.y;
                        float centerY = _PuzzleRevealCenter.y;
                        float waveHeight = centerY + (_PuzzleRevealProgress - 0.5) * 20.0;
                        
                        revealMask = smoothstep(waveHeight - 1.0, waveHeight + 0.5, worldY);
                        
                        float edge = abs(worldY - waveHeight);
                        edgeFactor = smoothstep(1.0, 0.0, edge) * _PuzzleRevealProgress;
                    }
                    
                    color.a *= revealMask;
                    
                    if (edgeFactor > 0.01)
                    {
                        fixed4 edgeGlow = _RevealEdgeColor * _RevealEdgeIntensity * edgeFactor;
                        color.rgb += edgeGlow.rgb * color.a;
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