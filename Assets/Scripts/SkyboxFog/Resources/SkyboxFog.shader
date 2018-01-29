Shader "Hidden/SkyboxFog"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SkyboxTex ("Skybox Texture", 2D) = "white" {}
		_fogStart ("Fog start", float) = 200
		_fogEnd ("Fog end", float) = 2000
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
			uniform float  _fogStart, _fogEnd;
			sampler2D_float _CameraDepthTexture;

			fixed4 frag (v2f i) : SV_Target
			{
				// Just a comment
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 colSkybox = tex2D(_SkyboxTex, i.uv);

				float depth = tex2D (_CameraDepthTexture, i.uv);
				float linearDepth = LinearEyeDepth (depth);

				float fogVal = 1.0 - ( _fogEnd - linearDepth ) / (_fogEnd - _fogStart);
				fogVal = max( 0.0, min(1.0, fogVal) );

				 float4 blend = lerp( col, colSkybox, fogVal );
				 return blend;
			}
			ENDCG
		}
	}
}
