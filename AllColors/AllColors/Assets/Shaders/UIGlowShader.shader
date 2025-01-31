Shader "Custom/UIGlowShader"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _TintIntensity ("Tint Intensity", Range(1, 10)) = 1 // Новый параметр для интенсивности Tint
        _GlowColor ("Glow Color", Color) = (1,1,1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 1
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            struct appdata_t
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
                float4 worldPosition : TEXCOORD1;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            float _TintIntensity; // Новый параметр для интенсивности Tint
            fixed4 _GlowColor;
            float _GlowIntensity;
            float4 _MainTex_ST;

            v2f vert (appdata_t v)
            {
                v2f o;
                o.worldPosition = v.vertex;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color * _Color * _TintIntensity; // Усиливаем Tint
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // Получаем цвет текста из текстуры
                fixed4 texColor = tex2D(_MainTex, i.texcoord);

                // Применяем Tint к тексту
                fixed4 tintedText = texColor * i.color;

                // Применяем свечение только к пикселям текста
                fixed4 glow = _GlowColor * _GlowIntensity * texColor.a; // Умножаем на альфа-канал текста
                fixed4 finalColor = tintedText + glow;

                // Сохраняем прозрачность
                finalColor.a = texColor.a;

                return finalColor;
            }
            ENDCG
        }
    }
}