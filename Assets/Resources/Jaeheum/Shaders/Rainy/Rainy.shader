Shader "Unlit/Rainy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {} // 텍스쳐 속성
        _Size("Size", float) = 1              // 물방울 크기
        _T("Time", float) = 1                 // 시간
        _Distortion("Distortion", range(-5, 5)) = 1 // 왜곡 정도 
        _Blur("Blur", range(0, 1)) = 1        // 흐림 정도
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" } // 렌더 타입을 지정
        LOD 100
        //GrabTexture를 사용하여 씬을 캡쳐해서 사용할 것이기 때문에,
        // 이 쉐이더가 적용되는 오브젝트는 캡쳐의 대상이 되지 않도록(=가장 나중에 랜더링 되도록)
        // Queue를 Transparent로 설정
        // 이거 하니까 오류생겨서 일단 막아놓음;;
        // Tags { "RenderType"="Opaque" "Queue" = "Transparent"}
        // GrabPass{"_GrabTexture"}
            
        Pass
        {
            CGPROGRAM 
            
            #pragma vertex vert // 버텍스 셰이더와 
            #pragma fragment frag // 프래그먼트 셰이더 함수 지정
            #pragma multi_compile_fog // 안개 사용을 위한 지시문
            // #define 지시문 => 매크로를 정의하는데 사용
            // S라는 매크로를 정의하여 smoothstep()함수를 간편하게 호출할 수 있다.
            #define S(a, b, t) smoothstep(a, b, t)
            #include "UnityCG.cginc" // 유니티에 내장된 셰이더 함수 사용

            struct appdata // 버텍스 입력 구조체
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f // 버텍스 출력 구조체
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            // 텍스쳐 샘플러 및 속성 변수 선언문들
            sampler2D _MainTex;
            //sampler2D _GrabTexture;
            float4 _MainTex_ST;
            float _Size;
            float _T;
            float _Distortion;
            float _Blur;

            v2f vert (appdata v) // 버텍스 셰이더 함수 정의 
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex); //버텍스 좌표 계산
                o.uv = TRANSFORM_TEX(v.uv, _MainTex); // 텍스쳐 좌표 계산
                UNITY_TRANSFER_FOG(o, o.vertex); // 안개 계산
                return o;
            }

            float N21(float2 p) // 노이즈 
            {
                p =frac(p*float2(123.34, 345.45));
                p += dot(p, p + 34.345);
                return frac(p.x *p.y);
            }

            // 물방울의 양을 늘리기 위해 레이어 생성 : 레이어 하나당 물방울 효과 UV세트
            float3 Layer(float2 UV, float t)  
            {
                float2 aspect = float2(2,1); // 2x1의 크기로 타일링 설정
                float2 uv = UV * _Size * aspect; // 텍스쳐 좌표 계산
                uv.y += t *.25;

                float2 gv = frac(uv) - 0.5; 
                float2 id = floor(uv);
                float n = N21(id); // 텍스쳐 변형에 따른 sin값 변화 적용
                t += n*6.2831; // sin그래프는 2pi주기이므로 주기별 랜덤 반복

                // X좌표 계산
                float w = UV.y * 10;
                float x = (n - .5) * .8;
                x += (.4-abs(x)) * sin(3*w) * pow(sin(w), 6) * 0.45;

                // y좌표 계산
                // 내려갈 때는 빠르고 올라갈 때는 느린 그래프
                float y = -sin(t+sin(t+sin(t)*0.5)) * 0.45;
                // 물방울 하단이 좀 더 부드러운 타원꼴을 나타나게 만듬
                // 각각 -x를 해주는 이유: x좌표가 이동하더라도 형태를 유지할 수 있다.
                y -= (gv.x-x) * (gv.x-x);

                // 물방울 위치 계산
                // gv: 세로로 긴 타원, gv/ aspect: 동그란 원
                float2 dropPos = (gv - float2(x, y)) / aspect;
                float drop = S(.05, .03, length(dropPos)); // 물방울 생성 

                // 궤적 위치 계산
                float2 trailPos = (gv - float2(x, t*.25)) / aspect;
                trailPos.y = (frac(trailPos.y * 8)-0.5) / 8; // 물방울 궤적을 y방향으로 8번 타일링
                float trail = S(.03, .01, length(trailPos)); // 물방울 궤적 그려주기

                float fogTrail = S(-.05, .05, dropPos.y);
                fogTrail *= S(.5, y, gv.y);
                trail *= fogTrail;

                fogTrail *= S(.05, .04, abs(dropPos.x)); // 물방울 따라 물 자국 남기기

                //col += fogTrail * .5;
                //col += trail;
                //col += drop;

                // Drop + Trail 모두 계산된 결과 == 효과 누적 계산
                float2 offs = drop * dropPos + trail * trailPos; 
                //if(gv.x > 0.48 || gv.y > 0.49) col = float4(1,0,0,1);

                return float3(offs, fogTrail);
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float t = fmod(_Time.y + _T, 7200); // 2시간마다 반복되도록

                float4 col = 0; 

                //물방울들의 양 늘리기 
                float3 drops = Layer(i.uv, t);
                drops += Layer(i.uv * 1.23 + 7.54, t);
                drops += Layer(i.uv * 1.35 + 1.54, t);
                drops += Layer(i.uv * 1.57 - 7.54, t);

                float blur = _Blur * 7 * (1 - drops.z);
                col = tex2Dlod(_MainTex, float4(i.uv + drops.xy * _Distortion, 0, blur));
                return col;
            }
            ENDCG
        }
    }
}
