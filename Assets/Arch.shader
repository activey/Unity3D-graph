// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "Arch_Design"
{
	Properties 
	{
_shine("Shine", Float) = 1
_furrines("Furrines", Float) = 1
_Color("Main Color", Color) = (1,1,1,1)
_MainTex("Base (RGB) Glow(A)", 2D) = "white" {}
_SpecularMap("Specular Map", 2D) = "white" {}
_Shininess("Shininess", Range(0.01,10) ) = 0.078125
_SpecularMultiply("Specular Multiply", Range(0,10) ) = 0.5
_BumpMap("Normal Map", 2D) = "bump" {}
_DetailNormal("Detail Normal (RGB)", 2D) = "bump" {}
_ReflectColor("Reflection Color", Color) = (1,1,1,0.5)
_Cube("Reflection Cubemap", Cube) = "black" {}
_reflectionMask("Reflection Mask", 2D) = "white" {}
_reflectionMultiply("Reflection Multiply", Range(0,1) ) = 0
_fresnelExponencial("Fresnel Exponencial", Range(0.1,10) ) = 0.5
_fresnelAdd("Fresnel Addiction", Range(0, 1) ) = 0.5
_test("Reflection Addiction", Float) = 0
_GlowMap("Glow Map", 2D) = "black" {}
_GlowIntensity("Glow Intensity", Range(0,5) ) = 0.5
_Speed("Glow Pan Speed", Range(0,10) ) = 0.5

	}
	
	SubShader 
	{
		Tags
		{
"Queue"="Geometry"
"IgnoreProjector"="False"
"RenderType"="Opaque"

		}

		
Cull Back
ZWrite On
ZTest LEqual
ColorMask RGBA
LOD 600
Fog{
}


		CGPROGRAM
#pragma surface surf BlinnPhongEditor  vertex:vert
#pragma target 3.0


float _shine;
float _furrines;
float4 _Color;
sampler2D _MainTex;
sampler2D _SpecularMap;
float _Shininess;
float _SpecularMultiply;
sampler2D _BumpMap;
sampler2D _DetailNormal;
float4 _ReflectColor;
samplerCUBE _Cube;
sampler2D _reflectionMask;
float _reflectionMultiply;
float _fresnelExponencial;
float _test;
sampler2D _GlowMap;
float _GlowIntensity;
float _Speed;
float _fresnelAdd;

			struct EditorSurfaceOutput {
				half3 Albedo;
				half3 Normal;
				half3 Emission;
				half3 Gloss;
				half Specular;
				half Alpha;
				half4 Custom;
			};
			
			inline half4 LightingBlinnPhongEditor_PrePass (EditorSurfaceOutput s, half4 light)
			{
half3 spec = light.a * s.Gloss;
half4 c;
c.rgb = (s.Albedo * light.rgb + light.rgb * spec);
c.a = s.Alpha;
return c;

			}

			inline half4 LightingBlinnPhongEditor (EditorSurfaceOutput s, half3 lightDir, half3 viewDir, half atten)
			{
				half3 h = normalize (lightDir + viewDir);
				
				half diff = max (0, dot ( lightDir, s.Normal ));
				
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular*128.0);
				
				half4 res;
				res.rgb = _LightColor0.rgb * diff;
				res.w = spec * Luminance (_LightColor0.rgb);
				res *= atten * 2.0;

				return LightingBlinnPhongEditor_PrePass( s, res );
			}

			inline half4 LightingBlinnPhongEditor_DirLightmap (EditorSurfaceOutput s, fixed4 color, fixed4 scale, half3 viewDir, bool surfFuncWritesNormal, out half3 specColor)
			{
				UNITY_DIRBASIS
				half3 scalePerBasisVector;
				
				half3 lm = DirLightmapDiffuse (unity_DirBasis, color, scale, s.Normal, surfFuncWritesNormal, scalePerBasisVector);
				
				half3 lightDir = normalize (scalePerBasisVector.x * unity_DirBasis[0] + scalePerBasisVector.y * unity_DirBasis[1] + scalePerBasisVector.z * unity_DirBasis[2]);
				half3 h = normalize (lightDir + viewDir);
			
				float nh = max (0, dot (s.Normal, h));
				float spec = pow (nh, s.Specular * 128.0);
				
				// specColor used outside in the forward path, compiled out in prepass
				specColor = lm * _SpecColor.rgb * s.Gloss * spec;
				
				// spec from the alpha component is used to calculate specular
				// in the Lighting*_Prepass function, it's not used in forward
				return half4(lm, spec);
			}
			
			struct Input {
				float2 uv_MainTex;
float3 viewDir;
float3 sWorldNormal;
float2 uv_reflectionMask;
float2 uv_BumpMap;
float2 uv_DetailNormal;
float2 uv_GlowMap;
float2 uv_SpecularMap;

			};

			void vert (inout appdata_full v, out Input o) {
float4 VertexOutputMaster0_0_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_1_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_2_NoInput = float4(0,0,0,0);
float4 VertexOutputMaster0_3_NoInput = float4(0,0,0,0);

o.sWorldNormal = mul((float3x3)unity_ObjectToWorld, SCALED_NORMAL);

			}
			

			void surf (Input IN, inout EditorSurfaceOutput o) {
				o.Normal = float3(0.0,0.0,1.0);
				o.Alpha = 1.0;
				o.Albedo = 0.0;
				o.Emission = 0.0;
				o.Gloss = 0.0;
				o.Specular = 0.0;
				o.Custom = 0.0;
				
float4 Tex2D0=tex2D(_MainTex,(IN.uv_MainTex.xyxy).xy);
float4 Normalize0=normalize(float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ));
float4 Dot0=dot( Normalize0.xyz, float4( IN.sWorldNormal.x, IN.sWorldNormal.y,IN.sWorldNormal.z,1.0 ).xyz ).xxxx;
float4 Clamp0=clamp(Dot0,float4( 0,0,0,0 ),float4( 1,1,1,1 ));
float4 Invert0= float4(1.0, 1.0, 1.0, 1.0) - Clamp0;
float4 Multiply1=Invert0 * Clamp0;
float4 Pow0=pow(Multiply1,_furrines.xxxx);
float4 Multiply2=Pow0 * _shine.xxxx;
float4 Add0=_Color + Multiply2;
float4 Multiply0=Tex2D0 * Add0;
float4 TexCUBE0=texCUBE(_Cube,float4( IN.sWorldNormal.x, IN.sWorldNormal.y,IN.sWorldNormal.z,1.0 ));
float4 Multiply3=_reflectionMultiply.xxxx * _ReflectColor;
float4 Multiply4=Multiply3 * _test.xxxx;
float4 Multiply6=TexCUBE0 * Multiply4;
float4 Fresnel0_1_NoInput = float4(0,0,1,1);
float4 Fresnel0=clamp((1.0 - dot( normalize( float4( IN.viewDir.x, IN.viewDir.y,IN.viewDir.z,1.0 ).xyz), normalize( Fresnel0_1_NoInput.xyz ) )).xxxx + _fresnelAdd,0,1);
float4 Pow1=pow(Fresnel0,_fresnelExponencial.xxxx);
float4 Clamp1=clamp(Pow1,float4( 0,0,0,0 ),float4( 1.0, 1.0, 1.0, 1.0 ));
float4 Lerp1=lerp(float4( 0.0, 0.0, 0.0, 0.0 ),Clamp1,_reflectionMultiply.xxxx);
float4 Tex2D2=tex2D(_reflectionMask,(IN.uv_reflectionMask.xyxy).xy);
float4 Multiply5=Lerp1 * Tex2D2;
float4 Lerp0=lerp(Multiply0,Multiply6,Multiply5);
float4 Tex2DNormal0=float4(UnpackNormal( tex2D(_BumpMap,(IN.uv_BumpMap.xyxy).xy)).xyz, 1.0 );
float4 Tex2DNormal1=float4(UnpackNormal( tex2D(_DetailNormal,(IN.uv_DetailNormal.xyxy).xy)).xyz, 1.0 );
float4 Add1=Tex2DNormal0 + Tex2DNormal1;
float4 Multiply10=_Time * _Speed.xxxx;
float4 UV_Pan0=float4((IN.uv_GlowMap.xyxy).x + Multiply10.x,(IN.uv_GlowMap.xyxy).y + Multiply10.x,(IN.uv_GlowMap.xyxy).z + Multiply10.x,(IN.uv_GlowMap.xyxy).w + Multiply10.x);
float4 Tex2D3=tex2D(_GlowMap,UV_Pan0.xy);
float4 Multiply9=Tex2D3 * _GlowIntensity.xxxx;
float4 Tex2D1=tex2D(_SpecularMap,(IN.uv_SpecularMap.xyxy).xy);
float4 Multiply8=Tex2D1.aaaa * _Shininess.xxxx;
float4 Multiply7=Tex2D1 * _SpecularMultiply.xxxx;
float4 Multiply11=Multiply7 * Clamp1;
float4 Master0_5_NoInput = float4(1,1,1,1);
float4 Master0_7_NoInput = float4(0,0,0,0);
float4 Master0_6_NoInput = float4(1,1,1,1);
o.Albedo = Lerp0;
o.Normal = Add1;
o.Emission = Multiply9;
o.Specular = Multiply8;
o.Gloss = Multiply11;

				o.Normal = normalize(o.Normal);
			}
		ENDCG
	}
	Fallback "Diffuse"
}