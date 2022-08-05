Shader "World Political Map/Unlit Cities" {
    Properties {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
		_HighlightColor("Highlight Color", Color) = (0,1,1,1)
        _HighlightRadiusSqr("Highlight Radius Sqr", Float) = 0.000005
    }

   	SubShader {
   		
       Tags {
	       "Queue"="Geometry-4"
       }
       ZWrite Off
       Blend SrcAlpha OneMinusSrcAlpha

       Pass {
    	CGPROGRAM
		#pragma vertex vert	
		#pragma fragment frag
        #pragma fragmentoption ARB_precision_hint_fastest
        #include "UnityCG.cginc"

		sampler2D _MainTex;
		half4 _Color;
		float3 _CityHighlightLocalPos;
		half4 _CityHighlightColor;
		float _CityHighlightRadiusSqr;

		struct appdata {
			float4 vertex : POSITION;
			float2 texcoord: TEXCOORD0;
            UNITY_VERTEX_INPUT_INSTANCE_ID
		};

		struct v2f {
			float4 pos : SV_POSITION;
			float2 uv: TEXCOORD0;
			half4 color : COLOR;
            UNITY_VERTEX_INPUT_INSTANCE_ID
            UNITY_VERTEX_OUTPUT_STEREO
		};

		#define dot2(x) dot(x,x)

		v2f vert(appdata v) {
			v2f o;
            UNITY_SETUP_INSTANCE_ID(v);
            UNITY_INITIALIZE_OUTPUT(v2f, o);
            UNITY_TRANSFER_INSTANCE_ID(v, o);
            UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
			o.pos = UnityObjectToClipPos(v.vertex);
			o.uv = v.texcoord;

			float dist = dot2(v.vertex.xyz - _CityHighlightLocalPos);
			o.color = (dist<_CityHighlightRadiusSqr) ? _CityHighlightColor : _Color;

			return o;
		}
		
		half4 frag(v2f i) : SV_Target {
            UNITY_SETUP_INSTANCE_ID(i);
            UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i); 
			half4 p = tex2D(_MainTex, i.uv);
			return p * i.color;					
		}
			
		ENDCG
    }
  }  
}