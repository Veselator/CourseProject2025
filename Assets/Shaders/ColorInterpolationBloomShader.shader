Shader "Custom/ColorInterpolationBloomShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MixColor ("Mix Color", Color) = (1,1,1,1)
        _ColorInterpolationValue ("Color Interpolation", Range(0,1)) = 0
        
        [Header(Bloom Settings)]
        _BloomColor ("Bloom Target Color", Color) = (1,0,0,1)
        _ColorTolerance ("Color Tolerance", Range(0,1)) = 0.1
        _BloomIntensity ("Bloom Intensity", Range(0,10)) = 2
    }
    
    SubShader
    {
        Tags 
        { 
            "RenderType"="Transparent" 
            "Queue"="Transparent"
        }
        LOD 100
        
        Cull Off
        Blend SrcAlpha OneMinusSrcAlpha
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
            };
            
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };
            
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MixColor;
            float _ColorInterpolationValue;
            
            float4 _BloomColor;
            float _ColorTolerance;
            float _BloomIntensity;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
    
                // ШАГ 1: Bloom эффект
                // Вычисляем расстояние между цветом пикселя и целевым цветом
                float colorDistance = distance(texColor.rgb, _BloomColor.rgb);
    
                // Если расстояние МЕНЬШЕ порога - цвет близкий, bloom ЕСТЬ
                // Если расстояние БОЛЬШЕ порога - цвет далёкий, bloom НЕТ
                float bloomMask = colorDistance < _ColorTolerance ? 1.0 : 0.0;
    
                // Усиливаем яркость для близких цветов
                fixed3 bloomColor = texColor.rgb * (1.0 + bloomMask * _BloomIntensity);
    
                // ШАГ 2: Color interpolation поверх
                fixed3 finalRGB = lerp(bloomColor, _MixColor.rgb, _ColorInterpolationValue);
    
                return fixed4(finalRGB, texColor.a);
            }
            ENDCG
        }
    }
}