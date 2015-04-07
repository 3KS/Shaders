Shader "Custom/SolidColor" {
    SubShader {
        Pass {
            Tags { "LightMode" = "ForwardBase" }
         
            CGPROGRAM
 
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #include "AutoLight.cginc"
            struct v2f
            {
                float4 pos : SV_POSITION;
                LIGHTING_COORDS(0,1)
            };

            v2f vert(appdata_base v) {
                v2f o;
                o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
                TRANSFER_VERTEX_TO_FRAGMENT(o);
                return o;
            }
 
            fixed4 frag(v2f i) : COLOR {
                float attenuation = LIGHT_ATTENUATION(i);
                return fixed4(1.0,0.0,0.0,1.0) * attenuation;
            }
 
            ENDCG
        }
    }
    Fallback "VertexLit"
}