Shader "HoverVR/VertColorTexTwoSidedOpaque" {
	Properties {
		_MainTex ("Main Texture", 2D) = "white"
	}
	Category {
		Lighting Off
		ZWrite On
		Cull Off
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {Queue=Geometry}
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
