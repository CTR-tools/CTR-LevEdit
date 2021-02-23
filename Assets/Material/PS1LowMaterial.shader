Shader "PS1Low" {
    Properties
    {
        _Tex ("Texture", 2D) = "white" {}
        _flipRotate ("FlipRotate", Int) = 0 
        _invisibleTriggers ("InvisibleTriggers", Int) = 0 
    }
SubShader {
    Pass {
         Cull off
        CGPROGRAM
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        sampler2D  _Tex;
        float4 _Tex_ST;
        int _flipRotate;
        int _invisibleTriggers;
        struct appdata {
            float4 vertex : POSITION;
            fixed4 color : COLOR;
            float2 uv : TEXCOORD0;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            fixed4 color : COLOR;
            float2 uv : TEXCOORD0;
        };
        
        
        #define None 0
        #define Rotate90 1
        #define Rotate180 2
        #define Rotate270 3
        #define FlipRotate270 4
        #define FlipRotate180 5
        #define FlipRotate90 6
        #define Flip 7
        float flipper;
        float2 flagrot(float2 uv, int flag) {
            uv.x = uv.x % 1.0;
            uv.y = uv.y % 1.0;
            float x = uv.x;
            float y = uv.y;
            switch(flag) {
                case Rotate90:
                    //uv = rotuv(uv, 90.0);
                    uv.x = y;
                    uv.y = x;
                    break;
                case Rotate180:
                    uv.x = -x;
                    uv.y = y;
                    break; 
                case Rotate270:
                    uv.x = -y;
                    uv.y = -x;
                    break;
                case FlipRotate90:
                    uv.x = y;
                    uv.y = -x;
                    flipper = 1;
                    break;
                case FlipRotate180: 
                    uv.x = x;
                    uv.y = -y;
                    flipper = 1;
                    break;
                case FlipRotate270:
                    uv.x = -y;
                    uv.y = x;
                    flipper = 1;
                    break;
                case Flip:
                    flipper = 1;
                    break;
            }
            if(flipper==1.0){
                uv.y *= -1.0;
                uv.x *= -1.0;
            }
            return uv;
        }
        v2f vert (appdata v) {
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex );
            o.color = v.color;
            o.uv = TRANSFORM_TEX(v.uv, _Tex);
            return o;
        }
        
        fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target { 
            float2 uv = i.uv;
            //uv = flagrot( uv, _flipRotate);
            if(_invisibleTriggers==1.0 && ( ((i.uv.x*8.0)%2.0)<1.5) ){
                discard;
            } 
            return ( facing > 0 ? i.color : i.color ) * tex2D(_Tex, uv); 
        }
        ENDCG
    }
}
}