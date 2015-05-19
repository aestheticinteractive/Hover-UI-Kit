Shader "HoverVR/VertColorTex" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white"
	}
	Category {
		Lighting Off
		ZWrite Off
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {Queue=Transparent}
		SubShader {
			Pass {
				BindChannels {
				   Bind "texcoord", texcoord
				   Bind "Color", color
				}
				SetTexture [_MainTex] {
					Combine texture * primary
				}
			}
		}
	}
}
