Shader "LeapMotion/LeapRGBUndistorted" {
  Properties {
    _Color ("Main Color (A=Opacity)", Color) = (1,1,1,1)
    _MainTex ("Base (A=Opacity)", 2D) = ""
  }
  SubShader {
    // Tags {"Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent"}

    Lighting Off
    Cull Off
    Zwrite Off
    Blend SrcAlpha OneMinusSrcAlpha

    Pass {
      CGPROGRAM
      #pragma vertex vert
      #pragma fragment frag

      #include "UnityCG.cginc"
      #pragma target 3.0

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
      
      float4 colorCorrect(float texImageX, float texImageY) {
        float width = 608.0;
        float height = 540.0;

        float mulitplier = 1.5;

        //float rscale = 1.23;
        //float gscale = 1.04;
        //float bscale = 0.73;

        float rscale = 1.5 * mulitplier;
        float gscale = 1.0 * mulitplier;
        float bscale = 0.5 * mulitplier;
      
        float corr_ir_g = 0.2;
        float corr_ir_rb = 0.2;
        float corr_r_b = 0.1;
        float corr_g_rb = 0.1;
      
        float2 r_offset = float2(0, -0.5);
        float2 g_offset = float2(0.5, -0.5);
        float2 b_offset = float2(0.5, 0);
        float2 texCoord = float2(texImageX, texImageY);
       
        float dx = 1.0/width;
        float dy = 1.0/height;
       
        float2 redOffset = float2(dx, dy)*r_offset;
        float2 greenOffset = float2(dx, dy)*g_offset;
        float2 blueOffset = float2(dx, dy)*b_offset;
       
        float4 input_lf;
       
        input_lf.a = tex2D(_MainTex, texCoord).r;
        // float ir_l = tex2D(_MainTex, texCoord + float2(-dx, 0)).r;
        // float ir_t = tex2D(_MainTex, texCoord + float2(0, -dy)).r;
        // float ir_r = tex2D(_MainTex, texCoord + float2(dx, 0)).r;
        // float ir_b = tex2D(_MainTex, texCoord + float2(0, dy)).r;
        // float ir_hf = iinput_lf.r - 0.25*(ir_l + ir_t + ir_r + ir_b);
       
        input_lf.r = tex2D(_MainTex, texCoord + redOffset).b;
        // float r_l = tex2D(_MainTex, texCoord + redOffset + float2(-dx, 0)).b;
        // float r_t = tex2D(_MainTex, texCoord + redOffset + float2(0, -dy)).b;
        // float r_r = tex2D(_MainTex, texCoord + redOffset + float2(dx, 0)).b;
        // float r_b = tex2D(_MainTex, texCoord + redOffset + float2(0, dy)).b;
        // float r_hf = input_lf.r - 0.25*(r_l + r_t + r_r + r_b);
       
        input_lf.g = tex2D(_MainTex, texCoord + greenOffset).a;
        // float g_l = tex2D(_MainTex, texCoord + greenOffset + float2(-dx, 0)).a;
        // float g_t = tex2D(_MainTex, texCoord + greenOffset + float2(0, -dy)).a;
        // float g_r = tex2D(_MainTex, texCoord + greenOffset + float2(dx, 0)).a;
        // float g_b = tex2D(_MainTex, texCoord + greenOffset + float2(0, dy)).a;
        // float g_hf = input_lf.g - 0.25*(g_l + g_t + g_r + g_b);
       
        input_lf.b = tex2D(_MainTex, texCoord + blueOffset).g;
        // float b_l = tex2D(_MainTex, texCoord + blueOffset + float2(-dx, 0)).g;
        // float b_t = tex2D(_MainTex, texCoord + blueOffset + float2(0, -dy)).g;
        // float b_r = tex2D(_MainTex, texCoord + blueOffset + float2(dx, 0)).g;
        // float b_b = tex2D(_MainTex, texCoord + blueOffset + float2(0, dy)).g;
        // float b_hf = input_lf.b - 0.25*(b_l + b_t + b_r + b_b);
        
        //float4x4 transformation = float4x4(5.6220, -1.5456, 0.3634, -0.1106, -1.6410, 3.1944, -1.7204, 0.0189, 0.1410, 0.4896, 10.8399, -0.1053, -3.7440, -1.9080, -8.6066, 1.0000);
        //float4x4 conservative = float4x4(5.6220, 0.0000, 0.3634, 0.0000, 0.0000, 3.1944, 0.0000, 0.0189, 0.1410, 0.4896, 10.8399, 0.0000, 0.0000, 0.0000, 0.0000, 1.0000);
        float4x4 transformation = float4x4(5.0670, -1.2312, 0.8625, -0.0507, -1.5210, 3.1104, -2.0194, 0.0017, -0.8310, -0.3000, 13.1744, -0.1052, -2.4540, -1.3848, -10.9618, 1.0000);
        float4x4 conservative = float4x4(5.0670, 0.0000, 0.8625, 0.0000, 0.0000, 3.1104, 0.0000, 0.0017, 0.0000, 0.0000, 13.1744, 0.0000, 0.0000, 0.0000, 0.0000, 1.0000);

        // input_lf = bilateral_a*bilateral(texCoord, input_lf) + (1-bilateral_a)*input_lf;
        // input_lf.r += ir_hf*corr_ir_rb + g_hf*corr_g_rb + b_hf*corr_r_b;
        // input_lf.g += ir_hf*corr_ir_g + r_hf*corr_g_rb + b_hf*corr_g_rb;
        // input_lf.b += ir_hf*corr_ir_rb + r_hf*corr_r_b + g_hf*corr_g_rb;

        float4 output_lf = mul(transpose(transformation), input_lf);
        float4 output_lf_fudge = mul(transpose(conservative), input_lf);
        //float4 output_lf_gray = gray*input_lf;

        float fudge_threshold = 0.5;
        //float ir_fudge_threshold = 0.95;
        //float ir_fudge_factor = 0.333*(input_lf.r + input_lf.g + input_lf.b);
       
        float rfudge = input_lf.r > fudge_threshold ? (input_lf.r - fudge_threshold)/(1.0 - fudge_threshold) : 0;
        float gfudge = input_lf.g > fudge_threshold ? (input_lf.g - fudge_threshold)/(1.0 - fudge_threshold) : 0;
        float bfudge = input_lf.b > fudge_threshold ? (input_lf.b - fudge_threshold)/(1.0 - fudge_threshold) : 0;
        //float irfudge = ir_fudge_factor > ir_fudge_threshold ? (ir_fudge_factor - ir_fudge_threshold)/(1.0 - ir_fudge_threshold) : 0;
        rfudge *= rfudge;
        gfudge *= gfudge;
        bfudge *= bfudge;
        //irfudge *= irfudge;
       
        float4 gl_FragColor;
        
        gl_FragColor.r = rfudge*output_lf_fudge.r + (1-rfudge)*output_lf.r;
        gl_FragColor.g = gfudge*output_lf_fudge.g + (1-gfudge)*output_lf.g;
        gl_FragColor.b = bfudge*output_lf_fudge.b + (1-bfudge)*output_lf.b;

        gl_FragColor.r *= rscale;
        gl_FragColor.g *= gscale;
        gl_FragColor.b *= bscale;
       
        //float avgrgb = 0.33333*(input_lf.r + input_lf.g + input_lf.b) - 0.9*input_lf.a;
        //float threshold = min(1, avgrgb*100);
        //threshold *= threshold;
        //gl_FragColor.rgb = output_lf_gray.rgb*(1 - threshold) + gl_FragColor.rgb*(threshold);
       
        float gamma = 1.0/_GammaCorrection;
        //gl_FragColor.rgb = show_ir > 0.5 ? float3(pow(ir_out, gamma)): pow(gl_FragColor.rgb, float3(gamma));
        
        gl_FragColor = float4(pow(gl_FragColor.rgb, float(gamma)), 1.0);
        return gl_FragColor;
      }
      
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
        // Unwarp the point. Ray range is [-4, 4] X [-4, 4].
        float2 ray = input.uv * float2(8.0, 8.0) - float2(4.0, 4.0);
        float2 texDist = float2(_RayOffsetX, _RayOffsetY) +
                         ray * float2(_RayScaleX, _RayScaleY);

        // Decode X and Y position floats from RGBA and rescale to [-0.6, 1.7).
        float texImageX = DecodeFloatRGBA(tex2D(_DistortX, texDist));
        texImageX = texImageX * 2.3 - 0.6;
        float texImageY = DecodeFloatRGBA(tex2D(_DistortY, texDist));
        texImageY = texImageY * 2.3 - 0.6;
        return colorCorrect(texImageX, texImageY);
      }
      ENDCG
    }
  }
  Fallback "Unlit/Texture"
}
