Shader "Custom/ColorMixShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _MixColor ("Mix Color", Color) = (1,1,1,1)
        _ColorInterpolationValue ("Color Interpolation", Range(0,1)) = 0
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
    
                // Применяем lerp к полному цвету с учётом альфы
                fixed3 mixedRGB = lerp(texColor.rgb, _MixColor.rgb * texColor.a, _ColorInterpolationValue);
    
                return fixed4(mixedRGB, texColor.a);
            }
            ENDCG
        }
    }
}