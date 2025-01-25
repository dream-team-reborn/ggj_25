Shader "Custom/EraseShader"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _EraseTex ("Erase Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "Queue"="Transparent" }
        Blend SrcAlpha OneMinusSrcAlpha
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float2 texcoord : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _EraseTex;

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.texcoord = v.texcoord;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 mainColor = tex2D(_MainTex, i.texcoord);
                fixed4 eraseColor = tex2D(_EraseTex, i.texcoord);
                mainColor.a = 1.0f - eraseColor.r;
                return mainColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}