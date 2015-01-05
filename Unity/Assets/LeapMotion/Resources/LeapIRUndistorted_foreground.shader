Shader "LeapMotion/LeapIRUndistorted_Foreground" {
  Properties {
    _Color ("Main Color (A=Opacity)", Color) = (1,1,1,1)
    _MainTex ("Base (A=Opacity)", 2D) = ""
  }
  SubShader {
  	Tags {"Queue" = "Overlay" }
    // Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

    Lighting Off
    Cull Off
    Zwrite On
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      #define MSIZE 5

      uniform float _RayOffsetX;
      uniform float _RayOffsetY;
      uniform float _RayScaleX;
      uniform float _RayScaleY;
      uniform int _BlackIsTransparent;
      uniform float _GammaCorrection;
      uniform sampler2D _MainTex;
      uniform sampler2D _DistortX;
      uniform sampler2D _DistortY;
      uniform float4 _Color;

      struct fragment_input{
        float4 position : SV_POSITION;
        float2 uv : TEXCOORD0;
      };

      fragment_input vert(appdata_img v) {
        fragment_input o;
        o.position = mul(UNITY_MATRIX_MVP, v.vertex);
        o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
        return o;
      }

      void frag(fragment_input input, out float4 col:COLOR, out float depth:DEPTH) {
        // Unwarp the point. Ray range is [-4, 4] X [-4, 4].
        float2 ray = input.uv * float2(8.0, 8.0) - float2(4.0, 4.0);
        float2 texDist = float2(_RayOffsetX, _RayOffsetY) +
                         ray * float2(_RayScaleX, _RayScaleY);

        // Decode X and Y position floats from RGBA and rescale to [-0.6, 1.7).
        float texImageX = DecodeFloatRGBA(tex2D(_DistortX, texDist));
        texImageX = texImageX * 2.3 - 0.6;
        float texImageY = DecodeFloatRGBA(tex2D(_DistortY, texDist));
        texImageY = texImageY * 2.3 - 0.6;

        if (texImageX > 1 || texImageX < 0 || texImageY > 1 || texImageY < 0) {
          col = float4(0, 0, 0, 0);
          depth = 1;
        } 
        else {
          // Find the undistorted pixel location.
          float2 texCoord = float2(texImageX, texImageY);
          float a = pow(tex2D(_MainTex, texCoord).a, (1.0 / _GammaCorrection));

          float4 color = _Color;

          if (_BlackIsTransparent == 1)
            color.a *= a;
          else {
            color = a * color;;
            color.a = 1.0;
          }

          col = color;
          depth = 0;
        }
      }
      ENDCG
    }
  }
  Fallback "Unlit/Texture"
}
