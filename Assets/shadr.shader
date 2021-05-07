 Shader "Kamaly/FarAlpha"
 {
     Properties
     {
        _MainTex ("Texture", 2D) = "white" {}
        _MinVisDistance("MinDistance",Float) = 0
        _MaxVisDistance("MaxDistance",Float) = 20
        _Position("Position", Vector) = (0, 0,0,0)
        [HDR]_Color("Color",Color) = (0,0,0,1)
     }
     SubShader
     {
             ZWrite Off
             Blend SrcAlpha OneMinusSrcAlpha
         Tags { "RenderType"="Transparent" "Queue"="Transparent" }
         LOD 100
 
         Pass
         {
             CGPROGRAM
             #pragma vertex vert
             #pragma fragment frag 
             // make fog work
             #pragma multi_compile_fog
             
             #include "UnityCG.cginc"
 
         struct v2f
     {
         half4 pos       : POSITION;
         fixed4 color : COLOR0;
         half2 uv        : TEXCOORD0;
     };
 
 
     half4       _Position;
     float4      _Color;
     sampler2D   _MainTex;
     half        _MinVisDistance;
     half        _MaxVisDistance;
 
 
 
     v2f vert(appdata_full v)
     {
         v2f o;
 
         o.pos = mul((half4x4)UNITY_MATRIX_MVP, v.vertex);
         o.uv = v.texcoord.xy;
         o.color = v.color * _Color;
 
         half3 viewDirW = (half3)_Position - mul((half4x4)unity_ObjectToWorld, v.vertex);
         half viewDist = length(viewDirW);
         half falloff = saturate((viewDist - _MinVisDistance) / (_MaxVisDistance - _MinVisDistance));
         o.color.a *= (1.0f - falloff);
         return o;
     }
 
 
     fixed4 frag(v2f i) : COLOR
     {
         fixed4 color = tex2D(_MainTex, i.uv) * i.color;
     return color;
     }
             ENDCG
         }
     }
 }