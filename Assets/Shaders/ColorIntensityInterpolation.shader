Shader "Custom/ColorIntensityInterpolation"
{
    Properties
    {
        _ColorIn ("Color In", Color) = (1, 0, 0, 1)
        _IntensityIn ("Intensity In", Range(0, 5)) = 1.0
        _ColorOut ("Color Out", Color) = (0, 0, 1, 1)
        _IntensityOut ("Intensity Out", Range(0, 5)) = 1.0
        _InterpolationValue ("Interpolation Value", Range(0, 1)) = 0.5
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
            
            fixed4 _ColorIn;
            float _IntensityIn;
            fixed4 _ColorOut;
            float _IntensityOut;
            float _InterpolationValue;
            
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            
            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 color = lerp(_ColorIn, _ColorOut, _InterpolationValue);
                float intensity = lerp(_IntensityIn, _IntensityOut, _InterpolationValue);
                color.rgb *= intensity;
                
                return color;
            }
            ENDCG
        }
    }
    
    FallBack "Sprites/Default"
}