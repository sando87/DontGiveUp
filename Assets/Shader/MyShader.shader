Shader "Custom/MyShader"
{
	Properties{
		_Integer("Integer", Int) = 0
		_LightDir("LightDir", Vector) = (1,1,1,0)
		_Color("Main Color", Color) = (1,1,1,0.5)
		_MainTex("Texture", 2D) = "white" { }
	}
	SubShader{
		Pass {

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0
			#include "UnityCG.cginc"

			struct vertinput {
				float4 pos : POSITION;
				float4 texcoord : TEXCOORD0;
				float4 normal : NORMAL;
				uint4 boneIdx : BLENDINDICES;
				float4 weights : BLENDWEIGHTS;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				fixed3 color : COLOR0;
			};

			float4 _LightDir;
			float4x4 _MatBones[45];

			v2f vert(vertinput v)
			{
				v2f o;
				float4 pos;
				v.pos.w = 1.0f;
				pos =   mul(v.pos, _MatBones[v.boneIdx.x] ) * v.weights.x;
				pos += mul(v.pos, _MatBones[v.boneIdx.y] ) * v.weights.y;
				pos += mul(v.pos, _MatBones[v.boneIdx.z] ) * v.weights.z;
				pos += mul(v.pos, _MatBones[v.boneIdx.w]) * v.weights.w;

				o.pos = UnityObjectToClipPos(pos);
				float light = dot(v.normal, _LightDir);
				o.color = fixed3(0, 1, 0) * light;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				return fixed4(i.color, 1);
			}
			ENDCG

		}
	}
}
