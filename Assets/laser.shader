// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/laser"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_Colour("Main Colour", Color) = (1, 1, 1, 1)
	}
		SubShader
		{
			Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
			LOD 100

			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha

			Pass
			{
				CGPROGRAM
				#pragma vertex vert
				#pragma fragment frag

				#include "UnityCG.cginc"				

				sampler2D _MainTex;
				float4 _MainTex_ST;

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
				};

				struct v2f {
					float2 uv : TEXCOORD0;
					float4 vertex : SV_POSITION;
				};

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 _Colour;

				fixed4 frag(v2f i) : SV_Target
				{
					float d = abs(0.5 - i.uv.y) * 2.0;

					return fixed4(
						lerp(d, _Colour.x, _Colour.x), 
						lerp(d, _Colour.y, _Colour.y),
						lerp(d, _Colour.z, _Colour.z),
						1.0 - d);
				}

				ENDCG
			}
		}
}
