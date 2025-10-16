Shader "Custom/AnimatedDiagonalLines"
{
    Properties
    {
        _BackgroundColor ("Background Color", Color) = (0.1, 0.1, 0.2, 1)
        _LineColor ("Line Color", Color) = (0.3, 0.5, 0.8, 1)
        _LineWidth ("Line Width", Range(0.01, 0.5)) = 0.1
        _LineSpacing ("Line Spacing", Range(0.1, 2.0)) = 0.5
        _Angle ("Angle", Range(-180, 180)) = 45
        _Speed ("Speed", Range(-5, 5)) = 1.0
        [Toggle] _IsNeedToRenderBackground ("Render Background", Float) = 1
        _Alpha ("Alpha", Range(0, 1)) = 1.0
    }
    
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Background" }
        LOD 100
        
        Pass
        {
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
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
            
            float4 _BackgroundColor;
            float4 _LineColor;
            float _LineWidth;
            float _LineSpacing;
            float _Angle;
            float _Speed;
            float _IsNeedToRenderBackground;
            float _Alpha;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                // Конвертируем угол в радианы
                float angleRad = _Angle * 0.0174533;
                
                // Поворачиваем UV координаты
                float2 rotatedUV;
                rotatedUV.x = i.uv.x * cos(angleRad) - i.uv.y * sin(angleRad);
                rotatedUV.y = i.uv.x * sin(angleRad) + i.uv.y * cos(angleRad);
                
                // Добавляем движение со временем
                float offset = _Time.y * _Speed;
                
                // Создаём паттерн линий
                float pattern = frac((rotatedUV.x + offset) / _LineSpacing);
                
                // Определяем, находимся ли мы на линии
                float lineMask = step(pattern, _LineWidth / _LineSpacing);
                
                // Смешиваем цвета
                fixed4 col = lerp(_BackgroundColor, _LineColor, lineMask);
                
                // Если фон не нужен - делаем фон прозрачным, оставляем только линии
                if (_IsNeedToRenderBackground < 0.5)
                {
                    col.a = lineMask; // Прозрачность зависит от того, линия это или нет
                }
                
                // Применяем общую прозрачность
                col.a *= _Alpha;
                
                return col;
            }
            ENDCG
        }
    }
}