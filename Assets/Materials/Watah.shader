Shader "Unlit/Watah"
{
    Properties
    {
        _MainTex ("Distortion", 2D) = "white" {}
        _XSpeed ("X Speed", float) = 0.2
        _YSpeed ("Y Speed", float) = 0.2
        _Intensity ("Intensity", float) = 1
        _OverlayTex ("Overlay", 2D) = "white" {}
        [HDR]_Color ("Color", Color) = (1, 1, 1, 1)
    }
    SubShader
    {
        // Queue is important! this object must be rendered after
        // Opaque objects.
        Tags { "RenderType"="Transparent" "Queue"="Transparent+1000"}
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
            sampler2D _MainTex;
            sampler2D _OverlayTex;
            float4 _MainTex_ST;
            float _XSpeed;
            float _YSpeed;
            float _Intensity;
            
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

            fixed4 frag (v2f i) : SV_Target{
                fixed4 col = tex2D(_MainTex, i.uv + _Time);
                fixed4 overlay = tex2D(_OverlayTex, i.uv);
                // sampled grab texture
                fixed4 grab = tex2Dproj(_GrabTexture, i.screenUV * float4(1 + (col.a * 0.1) * _Intensity, 1 + (col.a * 0.1) * _Intensity, 1, 1));
                UNITY_APPLY_FOG(i.fogCoord, col);
                //return col;
                // visualize coordinates
                return (grab * col) + ((overlay * _Color) * overlay.a);
            }
            ENDCG
        }
    }
}
