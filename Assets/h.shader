// Upgrade NOTE: replaced 'samplerRECT' with 'sampler2D'
// Upgrade NOTE: replaced 'texRECTproj' with 'tex2Dproj'

Shader "Ohno/Ohnoes" {
    Properties {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.9, 0.9, 0.9, 1)
        _SpeedX ("X Speed", float) = 0.25
        _SpeedY ("Y Speed", float) = 0.25
        _Intensity ("Intensity", float) = 1
        _IntensityX ("X Intensity", float) = 1
        _IntensityY ("Y Intensity", float) = 1
        _AddX ("Add X", float) = 0
        _AddY ("Add Y", float) = 0
        _ColorAdd ("Color Add", Color) = (0.1, 0.1, 0.1, 0)
        _Mul ("Effect Multiplier", float) = 1
        _TextureDistortionIntensity ("Texture Distortion Intensity", float) = 1
        _ScrollSpeedX ("Scroll X Speed", float) = 0
        _ScrollSpeedY ("Scroll Y Speed", float) = 0
    }
    SubShader{
        // Queue is important! this object must be rendered after
        // Opaque objects.
        Tags { "RenderType"="Transparent" "Queue"="Transparent"}
        LOD 100
        GrabPass{
            // "_BGTex"
            // if the name is assigned to the grab pass
            // all objects that use this shader also use a shared
            // texture grabbed before rendering them.
            // otherwise a new _GrabTexture will be created for each object.
        }
        ZWrite Off
        Pass{
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog
            #include "UnityCG.cginc"

            struct appdata{
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            struct v2f{
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                // this is a slot to put our screen coordinates into
                // it is a float4 instead of float2
                // because we need to use tex2Dproj() instead of tex2D()
                float4 screenUV : TEXCOORD1;
            };
            float4 _Color;
            float _Mul;
            float _SpeedX;
            float _SpeedY;
            float _Intensity;
            float _IntensityX;
            float _IntensityY;
            float _AddX;
            float _AddY;
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _ColorAdd;
            float _TextureDistortionIntensity;
            float _ScrollSpeedX;
            float _ScrollSpeedY;

            // builtin variable to get Grabbed Texture if GrabPass has no name
            sampler2D _GrabTexture;

            v2f vert (appdata v){
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                UNITY_TRANSFER_FOG(o,o.vertex);
                // builtin function to get screen coordinates for tex2Dproj()
                o.screenUV = ComputeGrabScreenPos(o.vertex);
                return o;
            }
            float3 hsv2rgb( float3 c ){
                float3 rgb = clamp( abs(fmod(c.x*6.0+float3(0.0,4.0,2.0),6)-3.0)-1.0, 0, 1);
                rgb = rgb*rgb*(3.0-2.0*rgb);
                return c.z * lerp( float3(1,1,1), rgb, c.y);
            }
            float3 RGB2HSV(float3 c)
            {
                float4 K = float4(0.0, -1.0 / 3.0, 2.0 / 3.0, -1.0);
                float4 p = lerp(float4(c.bg, K.wz), float4(c.gb, K.xy), step(c.b, c.g));
                float4 q = lerp(float4(p.xyw, c.r), float4(c.r, p.yzx), step(p.x, c.r));

                float d = q.x - min(q.w, q.y);
                float e = 1.0e-10;
                return float3(abs(q.z + (q.w - q.y) / (6.0 * d + e)), d / (q.x + e), q.x);
            }

            fixed4 frag (v2f i) : SV_Target{
                /*
                float4 u = i.screenUV;
                fixed grab = tex2Dproj(_GrabTexture, u);
                UNITY_APPLY_FOG(i.fogCoord, grab);
                float4 c = lerp(float4(0, 0, 0, 0), grab * _Color + (tex2D(_MainTex, uv) * _ColorAdd), _Mul);
                return c;
                */
                float2 uv = i.uv;
                uv.y+=cos(uv.x+_SpeedY*_Time*_Mul+_AddY)*_Intensity*_Mul*_IntensityY*_TextureDistortionIntensity;
                uv.x+=cos(uv.y+_SpeedX*_Time*_Mul+_AddX)*_Intensity*_Mul*_IntensityX*_TextureDistortionIntensity;
                uv.y += _Time * _ScrollSpeedY;
                uv.x += _Time * _ScrollSpeedX;
                fixed4 col = tex2D(_MainTex, uv);
                // sampled grab texture
                float4 u = i.screenUV;
                u.y+=cos(u.x+_SpeedY*_Time*_Mul+_AddY)*_Intensity*_Mul*_IntensityY;
                u.x+=cos(u.y+_SpeedX*_Time*_Mul+_AddX)*_Intensity*_Mul*_IntensityX;
                fixed4 grab = tex2Dproj(_GrabTexture, u);
                UNITY_APPLY_FOG(i.fogCoord, col);
                //return col;
                // visualize coordinates
                return ((grab * _Color) + (col * _ColorAdd));
            }
            ENDCG
        }
    }
}