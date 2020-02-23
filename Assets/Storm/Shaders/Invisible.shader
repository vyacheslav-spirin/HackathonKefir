Shader "Custom/Invisible"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Tint", Color) = (1,1,1,1)
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Transparent" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off

        GrabPass { "_GrabForInvisibleTex" }

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			};
			
			fixed4 _Color;

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = UnityObjectToClipPos(IN.vertex);
				
				OUT.screenPos = ComputeScreenPos(OUT.vertex);
				
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color * _Color;




				return OUT;
			}

			sampler2D _MainTex;
			
			sampler2D _GrabForInvisibleTex;

			fixed4 frag(v2f IN) : SV_Target
			{
                fixed2 uv = IN.screenPos;
    
                fixed4 grapTexCol = tex2D(_GrabForInvisibleTex, IN.screenPos.xy / IN.screenPos.w);
    
                fixed4 color = tex2D(_MainTex, IN.texcoord);
                color.rgb *= _Color.rgb;
                
                color.rgb = lerp(grapTexCol.rgb, color.rgb, _Color.a);

                clip(color.a - 0.5);

                return color;
			}
		ENDCG
		}
	}
}