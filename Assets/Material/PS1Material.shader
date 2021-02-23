Shader "PS1" {
    Properties
    {
        _Tex0 ("Texture", 2D) = "white" {}
        _Tex1 ("Texture", 2D) = "white" {}
        _Tex2 ("Texture", 2D) = "white" {}
        _Tex3 ("Texture", 2D) = "white" {}
        _flipRotate0 ("FlipRotate0", Int) = 0 
        _flipRotate1 ("FlipRotate1", Int) = 0 
        _flipRotate2 ("FlipRotate2", Int) = 0 
        _flipRotate3 ("FlipRotate3", Int) = 0 
        _invisibleTriggers ("InvisibleTriggers", Int) = 0 
        
    }
SubShader {
    Pass {
         Cull off
            Blend SrcAlpha OneMinusSrcAlpha
        CGPROGRAM     
        #include "Lighting.cginc"
        #pragma vertex vert
        #pragma fragment frag
        #include "UnityCG.cginc"
        #include "AutoLight.cginc"
        #include "Lighting.cginc"
        sampler2D  _Tex0,_Tex1,_Tex2,_Tex3;
        int _flipRotate0, _flipRotate1, _flipRotate2, _flipRotate3;
        float4 _Tex0_ST,_Tex1_ST,_Tex2_ST,_Tex3_ST;
        float3 _PointLightPos, _PointLightColor;
        float _PointLightSize;
        int _invisibleTriggers;
        struct appdata {
            float4 vertex : POSITION;
            fixed4 color : COLOR;
            float2 uv : TEXCOORD0;
            float4 normal : NORMAL;
        };

        struct v2f {
            float4 pos : SV_POSITION;
            fixed4 color : COLOR;
            float2 uv : TEXCOORD0;
        }; 
        
        v2f vert (appdata v) {
            float3 worldPos = mul (unity_ObjectToWorld, v.vertex).xyz;
            v2f o;
            o.pos = UnityObjectToClipPos(v.vertex );
            o.color = v.color;
            o.uv = TRANSFORM_TEX(v.uv, _Tex0);
            float3 lightDir = _WorldSpaceLightPos0;
            float NL = saturate(dot(v.normal, lightDir));
            float atten = LIGHT_ATTENUATION(i);
            float pointToPosDist = 10.0*lerp(0.0,1.0,saturate(1.0 - distance(_PointLightPos.xyz,worldPos) / _PointLightSize)); //100.0 * _PointLightSize / ( 1.0 + pow(distance(_PointLightPos.xyz,worldPos),2.0));
            o.color *= max(NL*2.0,0.1) * (_LightColor0 * atten + float4(pointToPosDist * _PointLightColor.r,pointToPosDist * _PointLightColor.g,pointToPosDist * _PointLightColor.b,1.0));
            return o;
        }
        
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
                case None:
                    //uv = rotuv(uv, 90.0);
                    uv.x = x;
                    uv.y = -y;
                    break;
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
                    uv.x = -x;
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
        fixed4 frag (v2f i, fixed facing : VFACE) : SV_Target { 
            //return fixed4(i.uv.x, i.uv.y, 0.0, 1.0);
            float2 uv = i.uv;
            half4 col;
            if(i.uv.y>1.0) {
                if(i.uv.x<1.0) {
                    uv = flagrot( uv, _flipRotate2);
                    //uv.x *= -1.0; //midunk =18 and not 17
                    //uv.y *= -1.0; //midunk =18 and not 17
                    uv.y *= -1.0;
                        col = ( facing > 0 ? i.color : half4(0.0,0.0,0.0,1.0)) * tex2D(_Tex2, uv); 
                } else {
                    uv = flagrot( uv, _flipRotate3);
                    uv.y *= -1.0;
                        col =  ( facing > 0 ? i.color : half4(0.0,0.0,0.0,1.0) ) * tex2D(_Tex3, uv); 
                }
            } else { 
                if(i.uv.x<1.0) {
                    uv = flagrot( uv, _flipRotate0);
                    //uv.x *= -1.0; //midunk =18 and not 17
                    uv.y *= -1.0;
                        col =  ( facing > 0 ? i.color : half4(0.0,0.0,0.0,1.0) ) * tex2D(_Tex0, uv); 
                } else {
                    uv = flagrot(uv, _flipRotate1);
                    uv.y *= -1.0;
                    //uv.y *= -1.0; //midunk =18 and not 17
                        col =  ( facing > 0 ? i.color : half4(0.0,0.0,0.0,1.0) ) * tex2D(_Tex1, uv); 
                }
            }   
            if(_invisibleTriggers==1.0 && ( ((i.uv.x*8.0)%1.0)<0.5) ){
                discard;
            } else {
                col.w=1.0;
            }
            return col;
        }
        ENDCG
    }
}
}