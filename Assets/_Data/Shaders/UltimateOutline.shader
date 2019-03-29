Shader "Outlined/UltimateOutline"
{
	Properties
	{
		_Color("Main Color", Color) = (0.5,0.5,0.5,1)
		_MainTex("Texture", 2D) = "white" {}

		_FirstOutlineColor("Outline color", Color) = (0.5,0,0,1)
		_FirstOutlineWidth("Outlines width", Range(0.0, 2.0)) = 0.02

	
	}

	CGINCLUDE
	#include "UnityCG.cginc"

	struct appdata {
		float4 vertex : POSITION;
		float4 normal : NORMAL;
	};

	uniform float4 _FirstOutlineColor;
	uniform float _FirstOutlineWidth;

	uniform float4 _SecondOutlineColor;
	uniform float _SecondOutlineWidth;

	uniform sampler2D _MainTex;
	uniform float4 _Color;
	uniform float _Angle;

	ENDCG

	SubShader{
		//First outline
		Pass{
			Tags{ "Queue" = "Transparent" "IgnoreProjector" = "True" }
			Stencil {
                Ref 2
                Comp notequal
                Pass keep
            }
			Blend SrcAlpha OneMinusSrcAlpha
			ZWrite Off
			Cull Back
			CGPROGRAM

			struct v2f {
				float4 pos : SV_POSITION;
			};

			#pragma vertex vert
			#pragma fragment frag

			v2f vert(appdata v) {
				appdata original = v;

				float3 scaleDir = normalize(v.vertex.xyz - float4(0,0,0,1));
				//This shader consists of 2 ways of generating outline that are dynamically switched based on demiliter angle
				//If vertex normal is pointed away from object origin then custom outline generation is used (based on scaling along the origin-vertex vector)
				//Otherwise the old-school normal vector scaling is used
				//This way prevents weird artifacts from being created when using either of the methods
				if (degrees(acos(dot(scaleDir.xyz, v.normal.xyz))) > _Angle) {
					v.vertex.xyz += normalize(v.normal.xyz) * _FirstOutlineWidth;
				}else {
					v.vertex.xyz += scaleDir * _FirstOutlineWidth;
				}

				v2f o;
				o.pos = UnityObjectToClipPos(v.vertex);
				return o;
			}

			half4 frag(v2f i) : COLOR{
				return _FirstOutlineColor;
			}

			ENDCG
		}
		

		

		
	}
	Fallback "Diffuse"
}