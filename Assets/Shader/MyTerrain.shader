Shader "Custom/MyTerrain"
{
	Properties{
		_Tex0("Texture", 2D) = "white" { }
		_Tex1("Texture", 2D) = "white" { }
		_Tex2("Texture", 2D) = "white" { }
		_Tex3("Texture", 2D) = "white" { }
		_Tex4("Texture", 2D) = "white" { }
		_Tex5("Texture", 2D) = "white" { }
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
				float4 tex : TEXCOORD0;
			};

			struct v2f {
				float4 pos : SV_POSITION;
				float2 tex0 : TEXCOORD0;
				float2 tex1 : TEXCOORD1;
				float2 tex2 : TEXCOORD2;
				float2 tex3 : TEXCOORD3;
				float2 tex4 : TEXCOORD4;
				float2 tex5 : TEXCOORD5;
			};

			float4 texelVector[12];
			sampler2D _Tex0;
			sampler2D _Tex1;
			sampler2D _Tex2;
			sampler2D _Tex3;
			sampler2D _Tex4;
			sampler2D _Tex5;

			v2f vert(vertinput v)
			{
				v2f o;
				v.pos.w = 1.0f;
				o.pos = UnityObjectToClipPos(v.pos);

				v.tex.z = 1.0f;
				v.tex.w = 0;
				o.tex0.x = dot(v.tex, texelVector[0]);
				o.tex0.y = dot(v.tex, texelVector[1]);
				o.tex1.x = dot(v.tex, texelVector[2]);
				o.tex1.y = dot(v.tex, texelVector[3]);
				o.tex2.x = dot(v.tex, texelVector[4]);
				o.tex2.y = dot(v.tex, texelVector[5]);
				o.tex3.x = dot(v.tex, texelVector[6]);
				o.tex3.y = dot(v.tex, texelVector[7]);
				o.tex4.x = dot(v.tex, texelVector[8]);
				o.tex4.y = dot(v.tex, texelVector[9]);
				o.tex5.x = dot(v.tex, texelVector[10]);
				o.tex5.y = dot(v.tex, texelVector[11]);

				return o;
			}

			fixed4 frag(v2f input) : SV_Target
			{
				fixed4 reg0;
				fixed4 reg1;
				fixed4 reg2;
				
				reg0 = tex2D(_Tex5, input.tex5);
				reg1 = tex2D(_Tex0, input.tex0);
				reg2 = tex2D(_Tex1, input.tex1);
				reg2 = reg2 - reg1;
				reg1 = (reg2 * reg0.x) + reg1;
				reg2 = tex2D(_Tex2, input.tex2);
				reg2 = reg2 - reg1;
				reg1 = (reg2 * reg0.y) + reg1;
				reg2 = tex2D(_Tex3, input.tex3);
				reg2 = reg2 - reg1;
				reg1 = (reg2 * reg0.z) + reg1;
				reg2 = tex2D(_Tex4, input.tex4);
				reg2 = reg2 - reg1;
				reg1 = (reg2 * reg0.w) + reg1;

				return reg1;
			}
			ENDCG

		}
	}
}
