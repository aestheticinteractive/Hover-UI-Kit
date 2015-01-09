//http://answers.unity3d.com/questions/378181/transparent-self-illumin-shaders.html

Shader "Unlit/AlphaSelfIllum" {
	Properties {
		_Color ("Color Tint", Color) = (1,1,1,1)
		_MainTex ("SelfIllum Color (RGB) Alpha (A)", 2D) = "white"
	}
	Category {
		Lighting Off
		ZWrite Off
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		Tags {Queue=Transparent}
		SubShader {
			Pass {
				SetTexture [_MainTex] {
					constantColor [_Color]
					Combine Texture * constant, Texture * constant
				}
			}
		}
	}
}
