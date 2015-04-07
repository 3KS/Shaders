Shader "Caveman/Global Positional Shader" {
	Properties {
     _Color ("Color", Color) = (1,1,1,1)
     _ObjectPosition ("Object Y Position", Float) = 0
}


CGINCLUDE
#include "UnityCG.cginc"
#include "AutoLight.cginc"

 float4 _Color;
 float _ObjectPosition;


struct appdataDiffuse {
	float4 vertex : POSITION;
};




ENDCG
 
SubShader {
//Tags {"RenderType"="Opaque" "Queue" = "Geometry" "LightMode" = "ForwardBase"}
	//LOD 300
	Pass {
		Tags {"LightMode" = "ForwardBase"}
		//Blend One One
		//Fog {Color(0,0,0,0)}
		
        CGPROGRAM
        #pragma vertex vert  
        #pragma fragment frag 
        #include "UnityCG.cginc"

        #pragma multi_compile_fwdbase
        #include "AutoLight.cginc"
		struct v2fDiffuse {
			float4 pos : SV_POSITION;
    		float4 localPosition : TEXCOORD1;
			LIGHTING_COORDS(0,1)
		};
		
		v2fDiffuse vert(appdataDiffuse input) {
			v2fDiffuse output; 
    		output.pos =  mul(UNITY_MATRIX_MVP, input.vertex);
    		output.localPosition = mul(_Object2World, input.vertex);
    		TRANSFER_VERTEX_TO_FRAGMENT(output);
    		return output;
		}
        
        float4 frag(v2fDiffuse input) : COLOR {
         	float atten = LIGHT_ATTENUATION(input)*2;
			return float4(abs(input.localPosition.x)/(float)255,abs(input.localPosition.y)/(float)255,abs(input.localPosition.z)/(float)255, 1) * atten; 
         }
         ENDCG  
      }
}
    Fallback "VertexLit"
}