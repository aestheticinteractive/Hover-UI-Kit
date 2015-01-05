Shader "LeapMotion/LeapRGBDistorted" {
  Properties {
    _Color ("Main Color (A=Opacity)", Color) = (1,1,1,1)
    _MainTex ("Base (A=Opacity)", 2D) = ""
  }
  SubShader {
    Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

    Lighting Off
    Cull Off
    Zwrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"

      #define MSIZE 5

      uniform int _BlackIsTransparent;
      uniform sampler2D _MainTex;
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

      float4 frag(fragment_input input) : COLOR {
        float2 position = input.uv;
        position.y = 1.0 - position.y;
        float4 textureColor = tex2D(_MainTex, position);
        float a = textureColor.a;

        float4 color = _Color;

        if (_BlackIsTransparent == 1)
          color.a *= a;
        else {
          color = a * color;;
          color.a = 1.0;
        }

        return color;
      }
      ENDCG
    }
  }
}
