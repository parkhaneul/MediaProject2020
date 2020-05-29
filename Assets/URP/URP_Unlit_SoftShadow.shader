Shader "Unlit/URP_Unlit_SoftShadow"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        Tags { 
            "RenderPipeline" = "UniversalPipeline"
            "RenderType"="Opaque" 
            "Queue"="Geometry+0"
        }
        LOD 100

        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"

            // GPU Instancing
            #pragma multi_compile_instancing
            // make fog work
            #pragma multi_compile_fog

            // Receive Shadow
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS
            #pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
            #pragma multi_compile _ _SHADOWS_SOFT
            // ? 위에 띄는게 맞나? 

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float fogCoord : TEXCOORD1;
                float3 normal : NORMAL;
                float4 shadowCoord : TEXCOORD2;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                UNITY_VERTEX_OUTPUT_STEREO //이건 VR 때문에 필요한 것임.
            };

            CBUFFER_START(UnityPerMaterial)

            TEXTURE2D(_MainTex);
            SAMPLER(sampler_MainTex);
            half4 _MainTex_ST;

            CBUFFER_END

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o); //Instancing 설정
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o); //VR 설정

                o.vertex = TransformObjectToHClip(v.vertex.xyz); //MVP랑 같음 이름만 달라짐.
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normal = TransformObjectToWorldNormal(v.normal); // Normal Transform은 따로 행렬이 필요.
                o.fogCoord = ComputeFogFactor(o.vertex.z);

                VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);
                o.shadowCoord = GetShadowCoord(vertexInput);
                return o;
            }

            half4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(i);

                Light mainLight = GetMainLight(i.shadowCoord);

                float4 col = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.uv);

                float NdotL = saturate(dot(_MainLightPosition.xyz, i.normal)); // NdotL로 간단히 라이팅한다.
                half3 ambient = SampleSH(i.normal); //구면조화함수랑 관련된 내용이라고 함. 아마 Spherical harmony 이거 아닐까..

                col.rgb *= NdotL * _MainLightColor.rgb * mainLight.shadowAttenuation * mainLight.distanceAttenuation + ambient;
                col.rgb = MixFog(col.rgb, i.fogCoord);
                
                // col = i.shadowCoord;
                // col.rgb = NdotL;
                // col.rgb = mainLight.shadowAttenuation;
                // col.rgb = mainLight.distanceAttenuation;

                return col;
            }
            ENDHLSL
        }
    }
}
