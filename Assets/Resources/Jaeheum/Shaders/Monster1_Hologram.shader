Shader "Custom/Monster1_Hologram"
{
    Properties
    {
        _RimColor ("Albedo (RGB)", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _RimThickness("RimLight Thickness", Float) = 3
        _HoloCycle ("Hologram Cycle", Float) = 1
        _HoloInterval ("Hologram Interval", Float) = 1
        _HoloThickness ("Hologram Thickness", Float) = 1
        _HoloOpacity1 ("Hologram Opacity", Float) = 1
        _HoloOpacity2 ("Hologram Opacity", Float) = 1
    }
    SubShader
    {
        Tags { "RenderType" = "Transparent" "Queue" = "Transparent"}

        CGPROGRAM
        #pragma surface surf Lambert noambient alpha:fade

        #pragma target 3.0

        sampler2D _MainTex;
        float4 _RimColor;
        float _RimThickness;
        float _HoloCycle;
        float _HoloInterval;
        float _HoloThickness;
        float _HoloOpacity1;
        float _HoloOpacity2;

        struct Input
        {
            float2 uv_MainTex;
            float3 viewDir;
            float3 worldPos;
        };

        UNITY_INSTANCING_BUFFER_START(Props)
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Normal map is not used, so this line is commented out
            // o.Normal = UnpackNormal(tex2D(_BumpMap, IN.uv_BumpMap));
            
            // Texture sampling
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
            o.Albedo = c.rgb;

            // Rim lighting
            float rim = saturate(dot(o.Normal, IN.viewDir));
            rim = pow(1 - rim, _RimThickness);
            rim += (pow(frac(IN.worldPos.y * _HoloInterval - _Time.y * _HoloCycle), _HoloThickness) * _HoloOpacity1);
            rim += (pow(frac(IN.worldPos.y * _HoloInterval + _Time.y * _HoloCycle), _HoloThickness) * _HoloOpacity2);
            
            o.Emission = _RimColor.rgb * rim;
            o.Alpha = rim;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
