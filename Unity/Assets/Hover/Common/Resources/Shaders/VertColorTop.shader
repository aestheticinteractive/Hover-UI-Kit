Shader "HoverVR/VertColorTop" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white"
	}
	Category {
		Lighting Off
		ZTest Always
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
