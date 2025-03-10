Shader "Custom/PlayerBehindObstacal"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,0.5)  // Màu của bóng, mặc định là màu trắng mờ
    }
    SubShader
    {
        Tags { "Queue" = "Overlay" }  // Đảm bảo vẽ trên cùng
        Pass
        {
            Stencil
            {
                Ref 1         // Giá trị tham chiếu Stencil Buffer
                Comp NotEqual // Nếu giá trị này không bằng 1, vẽ Shader
            }
            ColorMask RGB
            Blend SrcAlpha OneMinusSrcAlpha  // Làm trong suốt
            ZWrite Off  // Không ghi vào Z-Buffer
            ZTest Always // Luôn luôn vẽ bất kể khoảng cách

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            struct appdata_t {
                float4 vertex : POSITION;
            };

            struct v2f {
                float4 pos : SV_POSITION;
            };

            fixed4 _Color;

            v2f vert (appdata_t v) {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target {
                return _Color; // Vẽ bóng nhân vật
            }
            ENDCG
        }
    }
}
