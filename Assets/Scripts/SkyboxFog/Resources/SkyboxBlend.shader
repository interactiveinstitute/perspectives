Shader "Hidden/SkyboxBlend"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_SkyboxTex ("Skybox Texture", 2D) = "white" {}
		_effectBlend ("Blend", Range (0, 1)) = 0
		_blendStart ("Blend start", float) = 200
		_blendEnd ("Blend end", float) = 2000
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
				float2 uv_depth : TEXCOORD1;
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
			uniform float _effectBlend, _blendStart, _blendEnd;
			sampler2D_float _CameraDepthTexture;

			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
				fixed4 colSkybox = tex2D(_SkyboxTex, i.uv);

				float depth = tex2D (_CameraDepthTexture, i.uv);
				float linearDepth = LinearEyeDepth (depth);

				float fogVal = 1.0 - ( _blendEnd - linearDepth ) / (_blendEnd - _blendStart);
				fogVal = min(1.0, fogVal);

				float4 fogColor = float4(fogVal,fogVal,fogVal,1.0);

				 float4 blend = lerp( col, colSkybox, fogVal * _effectBlend );
				 return blend;
			}
			ENDCG
		}
	}
}
