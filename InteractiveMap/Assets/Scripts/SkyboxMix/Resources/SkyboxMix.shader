﻿Shader "Hidden/SkyboxMix"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SkyboxTex ("Skybox Texture", 2D) = "white" {}
		_blend ("Blend", Range (0, 1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

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

			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			sampler2D _SkyboxTex;
			uniform float _blend;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 colSkybox = tex2D(_SkyboxTex, i.uv);

				 fixed4 blend = lerp( colSkybox, col, _blend );
				 
				 return blend;
			}
			ENDCG
		}
	}
}
