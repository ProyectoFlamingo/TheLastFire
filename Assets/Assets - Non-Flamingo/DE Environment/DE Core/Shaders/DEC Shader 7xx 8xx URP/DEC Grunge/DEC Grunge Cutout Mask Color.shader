// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "DEC/Grunge/Grunge Cutout Mask Color"
{
	Properties
	{
		[HideInInspector] _EmissionColor("Emission Color", Color) = (1,1,1,1)
		[HideInInspector] _AlphaCutoff("Alpha Cutoff ", Range(0, 1)) = 0.5
		[ASEBegin][Header(DEBUG SETTINGS)][Enum(Off,0,On,1)]_ZWriteMode("ZWrite Mode", Int) = 1
		[Enum(None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15)]_ColorMask("Color Mask Mode", Int) = 15
		[Enum(Off,0,On,1)]_AlphatoCoverage("Alpha to Coverage", Float) = 0
		[Header(GLOBAL SETTINGS)][Enum(UnityEngine.Rendering.CullMode)]_CullMode("Cull Mode", Int) = 0
		[Enum(Default,0,Flip,1,Mirror,2)]_NormalMode("Normal Mode", Int) = 0
		[Enum(Off,0,Active,1)]_GlancingClipMode("Clip Glancing Angle Mode", Int) = 1
		[Header(MAP MAIN TEXTURE)]_Color("Albedo Tint", Color) = (1,1,1,0)
		[Enum(Off,0,Active,1,Active Inverted,2)]_Gradient_Mode("Gradient Mode", Int) = 0
		[HDR]_Gradient_Top("Gradient Tint Top", Color) = (0.7259277,0.7647059,0.06185123,0)
		[HDR]_Gradient_TopSide("Gradient Tint Top Side", Color) = (0.2569204,0.5525266,0.7279412,0)
		[HDR]_Gradient_Bottom("Gradient Tint Bottom", Color) = (0.3877363,0.5955882,0.188311,0)
		[HDR]_Gradient_BottomSide("Gradient Tint Bottom Side", Color) = (0.7058823,0.2024221,0.2024221,0)
		[HDR]_Gradient_TintCurvature("Gradient Tint Curvature", Color) = (1,0,0.444931,0)
		_Gradient_Ramp("Gradient Ramp", Range( 0 , 1)) = 0
		_Gradient_Contrast("Gradient Contrast", Range( 0 , 1)) = 0
		_Gradient_Height("Gradient Height", Range( 0 , 20)) = 17.80307
		[SingleLineTexture]_MainTex("Albedo Map", 2D) = "white" {}
		_Brightness("Brightness", Range( 0 , 2)) = 1
		_AlphaCutoffBias("Alpha Cutoff Bias", Range( 0 , 1)) = 0.5
		_AlphaCutoffBiasShadow("Alpha Cutoff Bias Shadow", Range( 0.01 , 1)) = 0.5
		_TilingX("Tiling X", Float) = 1
		_TilingY("Tiling Y", Float) = 1
		_OffsetX("Offset X", Float) = 0
		_OffsetY("Offset Y", Float) = 0
		[Normal][SingleLineTexture]_BumpMap("Normal Map", 2D) = "bump" {}
		_NormalStrength("Normal Strength", Float) = 1
		[SingleLineTexture]_MetallicGlossMap("Metallic Map", 2D) = "white" {}
		_MetallicStrength("Metallic Strength", Range( 0 , 1)) = 0
		[SingleLineTexture]_OcclusionMap("Occlusion Map", 2D) = "white" {}
		[Enum(Texture,0,Baked,1)]_OcclusionSourceMode("Occlusion Source Mode", Int) = 1
		_OcclusionStrengthAO("Occlusion Strength", Range( 0 , 1)) = 0.25
		[SingleLineTexture]_SmoothnessMap("Smoothness Map", 2D) = "white" {}
		[Enum(Standard,0,Geometric,1,Geometric Fade,2,Texture,3)]_SmoothnessModeCutout("Smoothness Mode", Int) = 3
		[Enum(Smoothness,0,Roughness,1)]_SmoothnessType("Smoothness Type", Int) = 0
		_SmoothnessStrength("Smoothness Strength", Range( 0 , 1)) = 0.1
		_SmoothnessThreshold("Smoothness Threshold", Range( 0 , 0.5)) = 0
		_SmoothnessVariance("Smoothness Variance", Range( 0 , 1)) = 0.2
		[Header(GRUNGE HORIZONTAL )][Enum(Off,0,Active,1,Active Inverted,2)]_Horizontalwear_Mode("Mode", Int) = 0
		_Horizontalwear_Tint("Tint", Color) = (1,1,1,0)
		[SingleLineTexture]_Horizontalwear_GrungeMap("Grunge Map", 2D) = "white" {}
		[Normal][SingleLineTexture]_Horizontalwear_GrungeNormal("Grunge Normal", 2D) = "bump" {}
		_Horizontalwear_TilingX("Tiling X", Float) = 1
		_Horizontalwear_TilingY("Tiling Y", Float) = 1
		_Horizontalwear_OffsetX("Offset X", Float) = 0
		_Horizontalwear_OffsetY("Offset Y", Float) = 0
		_Horizontalwear_Metallic("Metallic", Range( 0 , 1)) = 0
		_Horizontalwear_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Horizontalwear_Saturation("Saturation", Range( -1 , 5)) = 1
		_Horizontalwear_NormalMultiplier("Normal Multiplier", Range( 0 , 10)) = 1
		[Enum(Map Grunge,0,Map Albedo,1)]_Horizontalwear_MaskingSource("Masking Source", Int) = 0
		_Horizontalwear_MaskingColor("Masking Color", Color) = (0.5019608,0,0,0)
		_Horizontalwear_MaskingColorRange("Masking Color Range", Range( 0 , 1)) = 0.2
		_Horizontalwear_MaskingColorFuzziness("Masking Color Fuzziness", Range( 0 , 1)) = 0.1
		_Horizontalwear_BlendStrength("Blend Strength", Range( 0 , 0.99)) = 0.5
		_Horizontalwear_BlendMix("Blend Mix", Range( 0 , 1.5)) = 1
		[Header(GRUNGE VERTICAL)][Enum(Off,0,Active,1,Active Inverted,2)]_Verticalwear_Mode("Mode", Int) = 0
		_Verticalwear_Tint("Tint", Color) = (1,1,1,1)
		[SingleLineTexture]_Verticalwear_GrungeMap("Grunge Map", 2D) = "white" {}
		[Normal][SingleLineTexture]_Verticalwear_GrungeNormal("Grunge Normal", 2D) = "bump" {}
		_Verticalwear_TilingX("Tiling X", Float) = 1
		_Verticalwear_TilingY("Tiling Y", Float) = 1
		_Verticalwear_OffsetX("Offset X", Float) = 0
		_Verticalwear_OffsetY("Offset Y", Float) = 0
		_Verticalwear_Metallic("Metallic", Range( 0 , 1)) = 0
		_Verticalwear_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Verticalwear_Saturation("Saturation", Range( -1 , 5)) = 1
		_Verticalwear_NormalMultiplier("Normal Strength", Range( 0 , 10)) = 1
		[Enum(Map Grunge,0,Map Albedo,1)]_Verticalwear_MaskingSource("Masking Source", Int) = 0
		_Verticalwear_MaskingColor("Masking Color", Color) = (0.5019608,0,0,0)
		_Verticalwear_MaskingColorRange("Masking Color Range", Range( 0 , 1)) = 0.2
		_Verticalwear_MaskingColorFuzziness("Masking Color Fuzziness", Range( 0 , 1)) = 0.1
		_Verticalwear_BlendMix("Blend Mix", Range( 0 , 1.5)) = 1
		_Verticalwear_BlendStrength("Blend Strength", Range( 0 , 0.99)) = 0.5
		[Header(GRUNGE CURVATURE WEAR)][Enum(Off,0,Active,1,Active Inverted,2)]_Curvaturewear_Mode("Mode", Int) = 0
		_Curvaturewear_Tint("Tint", Color) = (1,1,1,1)
		[SingleLineTexture]_Curvaturewear_GrungeMap("Grunge Map", 2D) = "white" {}
		[Normal][SingleLineTexture]_Curvaturewear_GrungeNormal("Grunge Normal", 2D) = "bump" {}
		_Curvaturewear_TilingX("Tiling X", Float) = 1
		_Curvaturewear_TilingY("Tiling Y", Float) = 1
		_Curvaturewear_OffsetX("Offset X", Float) = 0
		_Curvaturewear_OffsetY("Offset Y", Float) = 0
		_Curvaturewear_Metallic("Metallic", Range( 0 , 1)) = 0
		_Curvaturewear_Smoothness("Smoothness", Range( 0 , 1)) = 0
		_Curvaturewear_Saturation("Saturation", Range( -1 , 5)) = 1
		_Curvaturewear_NormalMultiplier("Normal Multiplier", Range( 0 , 10)) = 1
		[Enum(Map Grunge,0,Map Albedo,1)]_Curvaturewear_MaskingSource("Masking Source", Int) = 0
		_Curvaturewear_MaskingColor("Masking Color", Color) = (0.5019608,0,0,0)
		_Curvaturewear_MaskingColorRange("Masking Color Range", Range( 0 , 1)) = 0.2
		_Curvaturewear_MaskingColorFuzziness("Masking Color Fuzziness", Range( 0 , 1)) = 0.1
		_Curvaturewear_BlendMix("Blend Mix", Range( 0 , 1)) = 0
		_Curvaturewear_BlendStrength("Blend Strength", Range( 0 , 0.99)) = 0.5
		[ASEEnd]_Curvaturewear_BlendHardness("Blend Hardness", Range( 0.75 , 2)) = 2

		//_TransmissionShadow( "Transmission Shadow", Range( 0, 1 ) ) = 0.5
		//_TransStrength( "Trans Strength", Range( 0, 50 ) ) = 1
		//_TransNormal( "Trans Normal Distortion", Range( 0, 1 ) ) = 0.5
		//_TransScattering( "Trans Scattering", Range( 1, 50 ) ) = 2
		//_TransDirect( "Trans Direct", Range( 0, 1 ) ) = 0.9
		//_TransAmbient( "Trans Ambient", Range( 0, 1 ) ) = 0.1
		//_TransShadow( "Trans Shadow", Range( 0, 1 ) ) = 0.5
		//_TessPhongStrength( "Tess Phong Strength", Range( 0, 1 ) ) = 0.5
		//_TessValue( "Tess Max Tessellation", Range( 1, 32 ) ) = 16
		//_TessMin( "Tess Min Distance", Float ) = 10
		//_TessMax( "Tess Max Distance", Float ) = 25
		//_TessEdgeLength ( "Tess Edge length", Range( 2, 50 ) ) = 16
		//_TessMaxDisp( "Tess Max Displacement", Float ) = 25
	}

	SubShader
	{
		LOD 0

		

		Tags { "RenderPipeline"="UniversalPipeline" "RenderType"="Opaque" "Queue"="Geometry-10" "NatureRendererInstancing"="True" }
		Cull [_CullMode]
		AlphaToMask [_AlphatoCoverage]
		HLSLINCLUDE
		#pragma target 3.5

		#pragma prefer_hlslcc gles
		


		#ifndef ASE_TESS_FUNCS
		#define ASE_TESS_FUNCS
		float4 FixedTess( float tessValue )
		{
			return tessValue;
		}
		
		float CalcDistanceTessFactor (float4 vertex, float minDist, float maxDist, float tess, float4x4 o2w, float3 cameraPos )
		{
			float3 wpos = mul(o2w,vertex).xyz;
			float dist = distance (wpos, cameraPos);
			float f = clamp(1.0 - (dist - minDist) / (maxDist - minDist), 0.01, 1.0) * tess;
			return f;
		}

		float4 CalcTriEdgeTessFactors (float3 triVertexFactors)
		{
			float4 tess;
			tess.x = 0.5 * (triVertexFactors.y + triVertexFactors.z);
			tess.y = 0.5 * (triVertexFactors.x + triVertexFactors.z);
			tess.z = 0.5 * (triVertexFactors.x + triVertexFactors.y);
			tess.w = (triVertexFactors.x + triVertexFactors.y + triVertexFactors.z) / 3.0f;
			return tess;
		}

		float CalcEdgeTessFactor (float3 wpos0, float3 wpos1, float edgeLen, float3 cameraPos, float4 scParams )
		{
			float dist = distance (0.5 * (wpos0+wpos1), cameraPos);
			float len = distance(wpos0, wpos1);
			float f = max(len * scParams.y / (edgeLen * dist), 1.0);
			return f;
		}

		float DistanceFromPlane (float3 pos, float4 plane)
		{
			float d = dot (float4(pos,1.0f), plane);
			return d;
		}

		bool WorldViewFrustumCull (float3 wpos0, float3 wpos1, float3 wpos2, float cullEps, float4 planes[6] )
		{
			float4 planeTest;
			planeTest.x = (( DistanceFromPlane(wpos0, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[0]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[0]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.y = (( DistanceFromPlane(wpos0, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[1]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[1]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.z = (( DistanceFromPlane(wpos0, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[2]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[2]) > -cullEps) ? 1.0f : 0.0f );
			planeTest.w = (( DistanceFromPlane(wpos0, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos1, planes[3]) > -cullEps) ? 1.0f : 0.0f ) +
						  (( DistanceFromPlane(wpos2, planes[3]) > -cullEps) ? 1.0f : 0.0f );
			return !all (planeTest);
		}

		float4 DistanceBasedTess( float4 v0, float4 v1, float4 v2, float tess, float minDist, float maxDist, float4x4 o2w, float3 cameraPos )
		{
			float3 f;
			f.x = CalcDistanceTessFactor (v0,minDist,maxDist,tess,o2w,cameraPos);
			f.y = CalcDistanceTessFactor (v1,minDist,maxDist,tess,o2w,cameraPos);
			f.z = CalcDistanceTessFactor (v2,minDist,maxDist,tess,o2w,cameraPos);

			return CalcTriEdgeTessFactors (f);
		}

		float4 EdgeLengthBasedTess( float4 v0, float4 v1, float4 v2, float edgeLength, float4x4 o2w, float3 cameraPos, float4 scParams )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;
			tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
			tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
			tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
			tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			return tess;
		}

		float4 EdgeLengthBasedTessCull( float4 v0, float4 v1, float4 v2, float edgeLength, float maxDisplacement, float4x4 o2w, float3 cameraPos, float4 scParams, float4 planes[6] )
		{
			float3 pos0 = mul(o2w,v0).xyz;
			float3 pos1 = mul(o2w,v1).xyz;
			float3 pos2 = mul(o2w,v2).xyz;
			float4 tess;

			if (WorldViewFrustumCull(pos0, pos1, pos2, maxDisplacement, planes))
			{
				tess = 0.0f;
			}
			else
			{
				tess.x = CalcEdgeTessFactor (pos1, pos2, edgeLength, cameraPos, scParams);
				tess.y = CalcEdgeTessFactor (pos2, pos0, edgeLength, cameraPos, scParams);
				tess.z = CalcEdgeTessFactor (pos0, pos1, edgeLength, cameraPos, scParams);
				tess.w = (tess.x + tess.y + tess.z) / 3.0f;
			}
			return tess;
		}
		#endif //ASE_TESS_FUNCS
		ENDHLSL

		
		Pass
		{
			
			Name "Forward"
			Tags { "LightMode"="UniversalForward" "NatureRendererInstancing"="True" }
			
			Blend One Zero, One Zero
			ZWrite [_ZWriteMode]
			ZTest LEqual
			Offset 0 , 0
			ColorMask [_ColorMask]
			

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#define _ALPHATEST_SHADOW_ON 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70503
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile _ DIRLIGHTMAP_COMBINED
			#pragma multi_compile _ LIGHTMAP_ON

			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_FORWARD

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#if ASE_SRP_VERSION <= 70108
			#define REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR
			#endif

			#if defined(UNITY_INSTANCING_ENABLED) && defined(_TERRAIN_INSTANCED_PERPIXEL_NORMAL)
			    #define ENABLE_TERRAIN_PERPIXEL_NORMAL
			#endif

			#define ASE_NEEDS_FRAG_WORLD_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_TANGENT
			#define ASE_NEEDS_FRAG_WORLD_BITANGENT
			#define ASE_NEEDS_FRAG_WORLD_VIEW_DIR
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:SetupNatureRenderer forwardadd
			#pragma multi_compile GPU_FRUSTUM_ON __
			#include "Nature Renderer.cginc"
			#pragma multi_compile_local _ NATURE_RENDERER


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord : TEXCOORD0;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				float4 lightmapUVOrVertexSH : TEXCOORD0;
				half4 fogFactorAndVertexLight : TEXCOORD1;
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				float4 shadowCoord : TEXCOORD2;
				#endif
				float4 tSpace0 : TEXCOORD3;
				float4 tSpace1 : TEXCOORD4;
				float4 tSpace2 : TEXCOORD5;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 screenPos : TEXCOORD6;
				#endif
				float4 ase_texcoord7 : TEXCOORD7;
				float4 ase_texcoord8 : TEXCOORD8;
				float4 ase_color : COLOR;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Verticalwear_MaskingColor;
			float4 _Horizontalwear_Tint;
			float4 _Curvaturewear_Tint;
			float4 _Horizontalwear_MaskingColor;
			float4 _Verticalwear_Tint;
			float4 _Gradient_Top;
			float4 _Gradient_Bottom;
			float4 _Gradient_BottomSide;
			float4 _Gradient_TintCurvature;
			float4 _Gradient_TopSide;
			float4 _Curvaturewear_MaskingColor;
			float4 _Color;
			float _Curvaturewear_BlendMix;
			half _NormalStrength;
			half _Curvaturewear_Saturation;
			float _Curvaturewear_OffsetY;
			int _Curvaturewear_MaskingSource;
			half _Curvaturewear_MaskingColorRange;
			half _Curvaturewear_MaskingColorFuzziness;
			float _Curvaturewear_BlendStrength;
			float _Curvaturewear_BlendHardness;
			float _Curvaturewear_OffsetX;
			float _Curvaturewear_TilingY;
			float _Curvaturewear_TilingX;
			int _NormalMode;
			float _MetallicStrength;
			float _Verticalwear_NormalMultiplier;
			float _Curvaturewear_Smoothness;
			float _Verticalwear_Smoothness;
			float _Horizontalwear_Smoothness;
			float _OcclusionStrengthAO;
			int _OcclusionSourceMode;
			int _SmoothnessType;
			int _GlancingClipMode;
			float _SmoothnessThreshold;
			float _SmoothnessVariance;
			float _SmoothnessStrength;
			int _SmoothnessModeCutout;
			float _Curvaturewear_Metallic;
			float _Verticalwear_Metallic;
			float _Horizontalwear_Metallic;
			float _Curvaturewear_NormalMultiplier;
			float _Horizontalwear_NormalMultiplier;
			float _Verticalwear_BlendMix;
			int _ColorMask;
			half _Verticalwear_MaskingColorFuzziness;
			float _OffsetY;
			float _OffsetX;
			float _TilingY;
			float _TilingX;
			float _Gradient_Contrast;
			float _Gradient_Ramp;
			half _Brightness;
			float _Gradient_Height;
			int _Horizontalwear_Mode;
			int _Verticalwear_Mode;
			int _Curvaturewear_Mode;
			int _ZWriteMode;
			float _AlphatoCoverage;
			int _CullMode;
			int _Gradient_Mode;
			float _Horizontalwear_TilingX;
			float _Horizontalwear_TilingY;
			float _Horizontalwear_OffsetX;
			half _Verticalwear_MaskingColorRange;
			int _Verticalwear_MaskingSource;
			half _AlphaCutoffBias;
			half _Verticalwear_Saturation;
			float _Verticalwear_OffsetY;
			float _Verticalwear_OffsetX;
			float _Verticalwear_TilingY;
			float _Verticalwear_TilingX;
			float _Horizontalwear_BlendMix;
			float _Horizontalwear_BlendStrength;
			half _Horizontalwear_MaskingColorFuzziness;
			half _Horizontalwear_MaskingColorRange;
			int _Horizontalwear_MaskingSource;
			half _Horizontalwear_Saturation;
			float _Horizontalwear_OffsetY;
			float _Verticalwear_BlendStrength;
			half _AlphaCutoffBiasShadow;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_trilinear_repeat);
			TEXTURE2D(_Horizontalwear_GrungeMap);
			TEXTURE2D(_Verticalwear_GrungeMap);
			TEXTURE2D(_Curvaturewear_GrungeMap);
			TEXTURE2D(_BumpMap);
			TEXTURE2D(_Horizontalwear_GrungeNormal);
			TEXTURE2D(_Verticalwear_GrungeNormal);
			TEXTURE2D(_Curvaturewear_GrungeNormal);
			TEXTURE2D(_MetallicGlossMap);
			TEXTURE2D(_SmoothnessMap);
			TEXTURE2D(_OcclusionMap);


			float4 mod289( float4 x )
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			
			float4 perm( float4 x )
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float SimpleNoise3D( float3 p )
			{
				 float3 a = floor(p);
				    float3 d = p - a;
				    d = d * d * (3.0 - 2.0 * d);
				 float4 b = a.xxyy + float4(0.0, 1.0, 0.0, 1.0);
				    float4 k1 = perm(b.xyxy);
				 float4 k2 = perm(k1.xyxy + b.zzww);
				    float4 c = k2 + a.zzzz;
				    float4 k3 = perm(c);
				    float4 k4 = perm(c + 1.0);
				    float4 o1 = frac(k3 * (1.0 / 41.0));
				 float4 o2 = frac(k4 * (1.0 / 41.0));
				    float4 o3 = o2 * d.z + o1 * (1.0 - d.z);
				    float2 o4 = o3.yw * d.x + o3.xz * (1.0 - d.x);
				    return o4.y * d.y + o4.x * (1.0 - d.y);
			}
			
			float4 float4switch143_g39811( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1506_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1476_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1269_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1512_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1400_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1377_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1516_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1491_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1380_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float3 float3switch56_g39824( int m_switch, float3 m_Default, float3 m_Flip, float3 m_Mirror )
			{
				if(m_switch ==0)
					return m_Default;
				else if(m_switch ==1)
					return m_Flip;
				else if(m_switch ==2)
					return m_Mirror;
				else
				return float3(0,0,0);
			}
			
			float3 float3switch37_g39824( int m_switch, float3 m_Default, float3 m_Flip, float3 m_Mirror )
			{
				if(m_switch ==0)
					return m_Default;
				else if(m_switch ==1)
					return m_Flip;
				else if(m_switch ==2)
					return m_Mirror;
				else
				return float3(0,0,0);
			}
			
			float3 AdditionalLightsFlat( float3 WorldPosition )
			{
				float3 Color = 0;
				#ifdef _ADDITIONAL_LIGHTS
				int numLights = GetAdditionalLightsCount();
				for(int i = 0; i<numLights;i++)
				{
					Light light = GetAdditionalLight(i, WorldPosition);
					Color += light.color *(light.distanceAttenuation * light.shadowAttenuation);
					
				}
				#endif
				return Color;
			}
			
			float3 float3switch1329_g39827( int m_switch, float3 m_Off, float3 m_Active, float3 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float3(0,0,0);
			}
			
			float3 float3switch1413_g39827( int m_switch, float3 m_Off, float3 m_Active, float3 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float3(0,0,0);
			}
			
			float3 float3switch1416_g39827( int m_switch, float3 m_Off, float3 m_Active, float3 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float3(0,0,0);
			}
			
			float floatswitch1434_g39827( int m_switch, float m_Off, float m_Active, float m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float(0);
			}
			
			float floatswitch1435_g39827( int m_switch, float m_Off, float m_Active, float m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float(0);
			}
			
			float floatswitch1436_g39827( int m_switch, float m_Off, float m_Active, float m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float(0);
			}
			
			float floatswitch1287_g37947( int m_switch, float m_Off, float m_Active )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else
				return float(0);
			}
			
			float4 float4switch189_g39758( int m_switch, float4 m_Smoothness, float4 m_Roughness )
			{
				if(m_switch ==0)
					return m_Smoothness;
				else if(m_switch ==1)
					return m_Roughness;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch188_g39758( int m_switch, float4 m_Standard, float4 m_Geometric, float4 m_GeometricFade, float4 m_Texture )
			{
				if(m_switch ==0)
					return m_Standard;
				else if(m_switch ==1)
					return m_Geometric;
				else if(m_switch ==2)
					return m_GeometricFade;
				else if(m_switch ==3)
					return m_Texture;
				else
				return float4(0,0,0,0);
			}
			
			float floatswitch1075_g39827( int m_switch, float m_Off, float m_Active, float m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float(0);
			}
			
			float floatswitch1432_g39827( int m_switch, float m_Off, float m_Active, float m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float(0);
			}
			
			float floatswitch1433_g39827( int m_switch, float m_Off, float m_Active, float m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float(0);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord7 = v.vertex;
				o.ase_texcoord8.xy = v.texcoord.xy;
				o.ase_color = v.ase_color;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord8.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif
				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float3 positionVS = TransformWorldToView( positionWS );
				float4 positionCS = TransformWorldToHClip( positionWS );

				VertexNormalInputs normalInput = GetVertexNormalInputs( v.ase_normal, v.ase_tangent );

				o.tSpace0 = float4( normalInput.normalWS, positionWS.x);
				o.tSpace1 = float4( normalInput.tangentWS, positionWS.y);
				o.tSpace2 = float4( normalInput.bitangentWS, positionWS.z);

				OUTPUT_LIGHTMAP_UV( v.texcoord1, unity_LightmapST, o.lightmapUVOrVertexSH.xy );
				OUTPUT_SH( normalInput.normalWS.xyz, o.lightmapUVOrVertexSH.xyz );

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					o.lightmapUVOrVertexSH.zw = v.texcoord;
					o.lightmapUVOrVertexSH.xy = v.texcoord * unity_LightmapST.xy + unity_LightmapST.zw;
				#endif

				half3 vertexLight = VertexLighting( positionWS, normalInput.normalWS );
				#ifdef ASE_FOG
					half fogFactor = ComputeFogFactor( positionCS.z );
				#else
					half fogFactor = 0;
				#endif
				o.fogFactorAndVertexLight = half4(fogFactor, vertexLight);
				
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
				VertexPositionInputs vertexInput = (VertexPositionInputs)0;
				vertexInput.positionWS = positionWS;
				vertexInput.positionCS = positionCS;
				o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				
				o.clipPos = positionCS;
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				o.screenPos = ComputeScreenPos(positionCS);
				#endif
				return o;
			}
			
			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_tangent : TANGENT;
				float4 texcoord : TEXCOORD0;
				float4 texcoord1 : TEXCOORD1;
				float4 ase_color : COLOR;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_tangent = v.ase_tangent;
				o.texcoord = v.texcoord;
				o.texcoord1 = v.texcoord1;
				o.ase_color = v.ase_color;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_tangent = patch[0].ase_tangent * bary.x + patch[1].ase_tangent * bary.y + patch[2].ase_tangent * bary.z;
				o.texcoord = patch[0].texcoord * bary.x + patch[1].texcoord * bary.y + patch[2].texcoord * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.ase_color = patch[0].ase_color * bary.x + patch[1].ase_color * bary.y + patch[2].ase_color * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag ( VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						, FRONT_FACE_TYPE ase_vface : FRONT_FACE_SEMANTIC ) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(IN);

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif

				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float2 sampleCoords = (IN.lightmapUVOrVertexSH.zw / _TerrainHeightmapRecipSize.zw + 0.5f) * _TerrainHeightmapRecipSize.xy;
					float3 WorldNormal = TransformObjectToWorldNormal(normalize(SAMPLE_TEXTURE2D(_TerrainNormalmapTexture, sampler_TerrainNormalmapTexture, sampleCoords).rgb * 2 - 1));
					float3 WorldTangent = -cross(GetObjectToWorldMatrix()._13_23_33, WorldNormal);
					float3 WorldBiTangent = cross(WorldNormal, -WorldTangent);
				#else
					float3 WorldNormal = normalize( IN.tSpace0.xyz );
					float3 WorldTangent = IN.tSpace1.xyz;
					float3 WorldBiTangent = IN.tSpace2.xyz;
				#endif
				float3 WorldPosition = float3(IN.tSpace0.w,IN.tSpace1.w,IN.tSpace2.w);
				float3 WorldViewDirection = _WorldSpaceCameraPos.xyz  - WorldPosition;
				float4 ShadowCoords = float4( 0, 0, 0, 0 );
				#if defined(ASE_NEEDS_FRAG_SCREEN_POSITION)
				float4 ScreenPos = IN.screenPos;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
					ShadowCoords = IN.shadowCoord;
				#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
					ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
				#endif
	
				WorldViewDirection = SafeNormalize( WorldViewDirection );

				int m_switch1380_g39827 = _Curvaturewear_Mode;
				int m_switch1377_g39827 = _Verticalwear_Mode;
				int m_switch1269_g39827 = _Horizontalwear_Mode;
				int _Gradient_Mode161_g39811 = _Gradient_Mode;
				int m_switch143_g39811 = _Gradient_Mode161_g39811;
				float4 TINT147_g39811 = _Color;
				float4 m_Off143_g39811 = TINT147_g39811;
				float4 _Vector0 = float4(0.001,0.001,0.001,0.001);
				float3 temp_output_275_0_g39811 = abs( WorldNormal );
				float WN_HORIZONTAL284_g39811 = ( ( temp_output_275_0_g39811 * temp_output_275_0_g39811 ).y + 0.07 );
				float4 lerpResult341_g39811 = lerp( _Gradient_TintCurvature , _Vector0 , WN_HORIZONTAL284_g39811);
				float4 VECTOR_IN_MASKED343_g39811 = lerpResult341_g39811;
				float4 Vect_001349_g39811 = _Vector0;
				float2 _Vector3 = float2(0,0.45);
				float3 temp_cast_2 = (0.5).xxx;
				float3 break277_g39811 = ( temp_output_275_0_g39811 - temp_cast_2 );
				float WN_X280_g39811 = break277_g39811.x;
				float smoothstepResult303_g39811 = smoothstep( _Vector3.x , _Vector3.y , ( WN_X280_g39811 + 1.25 ));
				float smoothstepResult299_g39811 = smoothstep( _Vector3.x , _Vector3.y , ( -WN_X280_g39811 + 0.02 ));
				float lerpResult306_g39811 = lerp( 0.0 , smoothstepResult303_g39811 , smoothstepResult299_g39811);
				float4 lerpResult301_g39811 = lerp( VECTOR_IN_MASKED343_g39811 , Vect_001349_g39811 , lerpResult306_g39811);
				float4 MASK_VERTICAL_X307_g39811 = lerpResult301_g39811;
				float2 _Vector1 = float2(0,0.85);
				float WN_Y281_g39811 = break277_g39811.y;
				float smoothstepResult334_g39811 = smoothstep( _Vector1.x , _Vector1.y , ( -WN_Y281_g39811 + 0.35 ));
				float4 lerpResult333_g39811 = lerp( VECTOR_IN_MASKED343_g39811 , Vect_001349_g39811 , smoothstepResult334_g39811);
				float4 MASK_VERTICAL_Y320_g39811 = lerpResult333_g39811;
				float2 _HelloRicardo = float2(1.8,1.4);
				float WN_Z282_g39811 = break277_g39811.z;
				float smoothstepResult331_g39811 = smoothstep( _HelloRicardo.x , _HelloRicardo.y , ( WN_Z282_g39811 + 2.06 ));
				float lerpResult327_g39811 = lerp( smoothstepResult331_g39811 , 0.0 , ( -WN_Z282_g39811 + 2.05 ));
				float4 lerpResult314_g39811 = lerp( Vect_001349_g39811 , VECTOR_IN_MASKED343_g39811 , lerpResult327_g39811);
				float4 MASK_VERTICAL_Z315_g39811 = lerpResult314_g39811;
				float4 Curvature353_g39811 = saturate( ( MASK_VERTICAL_X307_g39811 + MASK_VERTICAL_Y320_g39811 + MASK_VERTICAL_Z315_g39811 ) );
				float4 lerpResult117_g39811 = lerp( _Gradient_BottomSide , _Gradient_Bottom , WN_HORIZONTAL284_g39811);
				float4 lerpResult111_g39811 = lerp( _Gradient_TopSide , _Gradient_Top , WN_HORIZONTAL284_g39811);
				float clampResult106_g39811 = clamp( ( IN.ase_texcoord7.xyz.y / _Gradient_Height ) , 0.0 , 1.0 );
				float4 lerpResult113_g39811 = lerp( lerpResult117_g39811 , lerpResult111_g39811 , clampResult106_g39811);
				float3 temp_output_145_0_g39811 = (( Curvature353_g39811 + lerpResult113_g39811 )).xyz;
				float _Gradient_Ramp154_g39811 = _Gradient_Ramp;
				float _Gradient_Contrast157_g39811 = _Gradient_Contrast;
				float2 appendResult168_g37947 = (float2(_TilingX , _TilingY));
				float2 appendResult167_g37947 = (float2(_OffsetX , _OffsetY));
				float2 texCoord2_g39802 = IN.ase_texcoord8.xy * appendResult168_g37947 + appendResult167_g37947;
				float2 appendResult6_g39802 = (float2(texCoord2_g39802.x , texCoord2_g39802.y));
				float2 OUT_UV213_g37947 = appendResult6_g39802;
				float2 UV40_g39808 = OUT_UV213_g37947;
				float4 tex2DNode63_g39808 = SAMPLE_TEXTURE2D( _MainTex, sampler_trilinear_repeat, UV40_g39808 );
				float4 ALBEDO_RGBA1381_g37947 = tex2DNode63_g39808;
				float4 ALBEDO151_g39811 = ALBEDO_RGBA1381_g37947;
				float smoothstepResult142_g39811 = smoothstep( ( _Gradient_Ramp154_g39811 - ( _Gradient_Ramp154_g39811 * ( 1.0 - _Gradient_Contrast157_g39811 ) ) ) , ALBEDO151_g39811.x , _Gradient_Ramp154_g39811);
				float4 lerpResult139_g39811 = lerp( TINT147_g39811 , float4( temp_output_145_0_g39811 , 0.0 ) , smoothstepResult142_g39811);
				float4 m_Active143_g39811 = lerpResult139_g39811;
				float4 lerpResult141_g39811 = lerp( float4( temp_output_145_0_g39811 , 0.0 ) , TINT147_g39811 , smoothstepResult142_g39811);
				float4 m_ActiveInverted143_g39811 = lerpResult141_g39811;
				float4 localfloat4switch143_g39811 = float4switch143_g39811( m_switch143_g39811 , m_Off143_g39811 , m_Active143_g39811 , m_ActiveInverted143_g39811 );
				float4 temp_output_3_0_g37947 = ( localfloat4switch143_g39811 * float4( (ALBEDO_RGBA1381_g37947).rgb , 0.0 ) * _Brightness );
				float4 temp_output_6_0_g39827 = ( temp_output_3_0_g37947 + float4(0,0,0,0) );
				float4 m_Off1269_g39827 = temp_output_6_0_g39827;
				float2 appendResult1344_g39827 = (float2(_Horizontalwear_TilingX , _Horizontalwear_TilingY));
				float2 appendResult1343_g39827 = (float2(_Horizontalwear_OffsetX , _Horizontalwear_OffsetY));
				float2 texCoord1342_g39827 = IN.ase_texcoord8.xy * appendResult1344_g39827 + appendResult1343_g39827;
				float4 tex2DNode1327_g39827 = SAMPLE_TEXTURE2D( _Horizontalwear_GrungeMap, sampler_trilinear_repeat, texCoord1342_g39827 );
				float4 temp_output_12_0_g39830 = ( float4( (_Horizontalwear_Tint).rgb , 0.0 ) * tex2DNode1327_g39827 );
				float3 desaturateInitialColor10_g39830 = temp_output_12_0_g39830.xyz;
				float desaturateDot10_g39830 = dot( desaturateInitialColor10_g39830, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39830 = lerp( desaturateInitialColor10_g39830, desaturateDot10_g39830.xxx, ( 1.0 - ( _Horizontalwear_Saturation + 1.0 ) ) );
				float3 FINAL_MASK_HORIZONTAL804_g39827 = desaturateVar10_g39830;
				float4 temp_cast_15 = (0.5).xxxx;
				float4 temp_cast_16 = (0.75).xxxx;
				int _Horizontalwear_Mode1003_g39827 = _Horizontalwear_Mode;
				int m_switch1476_g39827 = _Horizontalwear_Mode1003_g39827;
				float4 break91_g39853 = _Horizontalwear_MaskingColor;
				float3 appendResult87_g39853 = (float3(break91_g39853.r , break91_g39853.g , break91_g39853.b));
				int m_switch1506_g39827 = _Horizontalwear_MaskingSource;
				float4 _Horizontalwear_GrungeMap1508_g39827 = tex2DNode1327_g39827;
				float4 m_MapGrunge1506_g39827 = _Horizontalwear_GrungeMap1508_g39827;
				float4 ALBEDO_IN1447_g39827 = temp_output_6_0_g39827;
				float4 m_MapAlbedo1506_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1506_g39827 = float4switch1506_g39827( m_switch1506_g39827 , m_MapGrunge1506_g39827 , m_MapAlbedo1506_g39827 );
				float4 break89_g39853 = localfloat4switch1506_g39827;
				float3 appendResult88_g39853 = (float3(break89_g39853.x , break89_g39853.y , break89_g39853.z));
				float4 temp_cast_18 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1476_g39827 = temp_cast_18;
				float4 temp_cast_19 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1476_g39827 = temp_cast_19;
				float4 temp_cast_20 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1476_g39827 = temp_cast_20;
				float4 localfloat4switch1476_g39827 = float4switch1476_g39827( m_switch1476_g39827 , m_Off1476_g39827 , m_Active1476_g39827 , m_ActiveInverted1476_g39827 );
				float4 temp_output_44_0_g39857 = localfloat4switch1476_g39827;
				float4 VECTOR_IN212_g39857 = temp_output_44_0_g39857;
				float4 _Vec = float4(0.001,0.001,0.001,0.001);
				float4 Vect_0019_g39857 = _Vec;
				float2 _SmoothHorizontal = float2(0,0.5);
				float3 temp_output_40_0_g39857 = abs( WorldNormal );
				float WN_HORIZONTAL141_g39857 = ( ( temp_output_40_0_g39857 * temp_output_40_0_g39857 ).y + 0.07 );
				float smoothstepResult205_g39857 = smoothstep( _SmoothHorizontal.x , _SmoothHorizontal.y , ( ( 1.0 - WN_HORIZONTAL141_g39857 ) + -0.1 ));
				float4 lerpResult200_g39857 = lerp( VECTOR_IN212_g39857 , Vect_0019_g39857 , smoothstepResult205_g39857);
				float4 MASK_HORIZONTAL204_g39857 = lerpResult200_g39857;
				float4 temp_cast_21 = (_Horizontalwear_BlendStrength).xxxx;
				float4 temp_cast_22 = (-1.0).xxxx;
				float4 smoothstepResult75_g39857 = smoothstep( temp_cast_15 , temp_cast_16 , (float4( 0,0,0,0 ) + (( 1.0 - MASK_HORIZONTAL204_g39857 ) - temp_cast_21) * (temp_cast_22 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_21)));
				float4 _MinOldVect4 = float4(0,0,0,0);
				float MASK_HORIZONTAL808_g39827 = ( (( ( smoothstepResult75_g39857 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x * _Horizontalwear_BlendMix );
				float4 lerpResult861_g39827 = lerp( temp_output_6_0_g39827 , float4( FINAL_MASK_HORIZONTAL804_g39827 , 0.0 ) , MASK_HORIZONTAL808_g39827);
				float4 m_Active1269_g39827 = lerpResult861_g39827;
				float4 m_ActiveInverted1269_g39827 = lerpResult861_g39827;
				float4 localfloat4switch1269_g39827 = float4switch1269_g39827( m_switch1269_g39827 , m_Off1269_g39827 , m_Active1269_g39827 , m_ActiveInverted1269_g39827 );
				float4 m_Off1377_g39827 = localfloat4switch1269_g39827;
				float2 appendResult1384_g39827 = (float2(_Verticalwear_TilingX , _Verticalwear_TilingY));
				float2 appendResult1386_g39827 = (float2(_Verticalwear_OffsetX , _Verticalwear_OffsetY));
				float2 texCoord1388_g39827 = IN.ase_texcoord8.xy * appendResult1384_g39827 + appendResult1386_g39827;
				float4 tex2DNode1397_g39827 = SAMPLE_TEXTURE2D( _Verticalwear_GrungeMap, sampler_trilinear_repeat, texCoord1388_g39827 );
				float4 temp_output_12_0_g39842 = ( float4( (_Verticalwear_Tint).rgb , 0.0 ) * tex2DNode1397_g39827 );
				float3 desaturateInitialColor10_g39842 = temp_output_12_0_g39842.xyz;
				float desaturateDot10_g39842 = dot( desaturateInitialColor10_g39842, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39842 = lerp( desaturateInitialColor10_g39842, desaturateDot10_g39842.xxx, ( 1.0 - ( _Verticalwear_Saturation + 1.0 ) ) );
				float3 FINAL_MASK_VERTICAL394_g39827 = desaturateVar10_g39842;
				float4 temp_cast_27 = (0.5).xxxx;
				float4 temp_cast_28 = (0.75).xxxx;
				int _Verticalwear_Mode1004_g39827 = _Verticalwear_Mode;
				int m_switch1400_g39827 = _Verticalwear_Mode1004_g39827;
				float4 break91_g39848 = _Verticalwear_MaskingColor;
				float3 appendResult87_g39848 = (float3(break91_g39848.r , break91_g39848.g , break91_g39848.b));
				int m_switch1512_g39827 = _Verticalwear_MaskingSource;
				float4 _Verticalwear_GrungeMap1509_g39827 = tex2DNode1397_g39827;
				float4 m_MapGrunge1512_g39827 = _Verticalwear_GrungeMap1509_g39827;
				float4 m_MapAlbedo1512_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1512_g39827 = float4switch1512_g39827( m_switch1512_g39827 , m_MapGrunge1512_g39827 , m_MapAlbedo1512_g39827 );
				float4 break89_g39848 = localfloat4switch1512_g39827;
				float3 appendResult88_g39848 = (float3(break89_g39848.x , break89_g39848.y , break89_g39848.z));
				float4 temp_cast_30 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1400_g39827 = temp_cast_30;
				float4 temp_cast_31 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1400_g39827 = temp_cast_31;
				float4 temp_cast_32 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1400_g39827 = temp_cast_32;
				float4 localfloat4switch1400_g39827 = float4switch1400_g39827( m_switch1400_g39827 , m_Off1400_g39827 , m_Active1400_g39827 , m_ActiveInverted1400_g39827 );
				float4 temp_output_44_0_g39828 = localfloat4switch1400_g39827;
				float3 temp_output_40_0_g39828 = abs( WorldNormal );
				float WN_HORIZONTAL141_g39828 = ( ( temp_output_40_0_g39828 * temp_output_40_0_g39828 ).y + 0.07 );
				float4 lerpResult2_g39828 = lerp( temp_output_44_0_g39828 , _Vec , WN_HORIZONTAL141_g39828);
				float4 VECTOR_IN_MASKED112_g39828 = lerpResult2_g39828;
				float4 Vect_0019_g39828 = _Vec;
				float3 break349_g39828 = WorldNormal;
				float WN_NY173_g39828 = break349_g39828.y;
				float WN_NZ184_g39828 = break349_g39828.z;
				float WN_NX186_g39828 = break349_g39828.x;
				float temp_output_175_0_g39828 = ( -WN_NY173_g39828 + WN_NZ184_g39828 + -WN_NX186_g39828 );
				float4 lerpResult133_g39828 = lerp( VECTOR_IN_MASKED112_g39828 , Vect_0019_g39828 , ( temp_output_175_0_g39828 + -temp_output_175_0_g39828 + 0.07 ));
				float4 MASK_SIDE163_g39828 = lerpResult133_g39828;
				float4 temp_cast_33 = (_Verticalwear_BlendStrength).xxxx;
				float4 temp_cast_34 = (-2.0).xxxx;
				float4 smoothstepResult75_g39828 = smoothstep( temp_cast_27 , temp_cast_28 , (float4( 0,0,0,0 ) + (( 1.0 - MASK_SIDE163_g39828 ) - temp_cast_33) * (temp_cast_34 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_33)));
				float MASK_VERTICAL602_g39827 = ( (( ( smoothstepResult75_g39828 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x * _Verticalwear_BlendMix );
				float4 lerpResult1227_g39827 = lerp( localfloat4switch1269_g39827 , float4( FINAL_MASK_VERTICAL394_g39827 , 0.0 ) , MASK_VERTICAL602_g39827);
				float4 m_Active1377_g39827 = lerpResult1227_g39827;
				float4 m_ActiveInverted1377_g39827 = lerpResult1227_g39827;
				float4 localfloat4switch1377_g39827 = float4switch1377_g39827( m_switch1377_g39827 , m_Off1377_g39827 , m_Active1377_g39827 , m_ActiveInverted1377_g39827 );
				float4 m_Off1380_g39827 = localfloat4switch1377_g39827;
				float2 appendResult1393_g39827 = (float2(_Curvaturewear_TilingX , _Curvaturewear_TilingY));
				float2 appendResult1390_g39827 = (float2(_Curvaturewear_OffsetX , _Curvaturewear_OffsetY));
				float2 texCoord1389_g39827 = IN.ase_texcoord8.xy * appendResult1393_g39827 + appendResult1390_g39827;
				float4 tex2DNode1396_g39827 = SAMPLE_TEXTURE2D( _Curvaturewear_GrungeMap, sampler_trilinear_repeat, texCoord1389_g39827 );
				float4 temp_output_12_0_g39834 = ( float4( (_Curvaturewear_Tint).rgb , 0.0 ) * tex2DNode1396_g39827 );
				float3 desaturateInitialColor10_g39834 = temp_output_12_0_g39834.xyz;
				float desaturateDot10_g39834 = dot( desaturateInitialColor10_g39834, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39834 = lerp( desaturateInitialColor10_g39834, desaturateDot10_g39834.xxx, ( 1.0 - ( _Curvaturewear_Saturation + 1.0 ) ) );
				float4 lerpResult1544_g39827 = lerp( ALBEDO_IN1447_g39827 , float4( desaturateVar10_g39834 , 0.0 ) , _Curvaturewear_BlendMix);
				float4 FINAL_MASK_CURVATURE586_g39827 = lerpResult1544_g39827;
				float4 temp_cast_39 = (0.5).xxxx;
				float4 temp_cast_40 = (0.75).xxxx;
				int _Curvaturewear_Mode398_g39827 = _Curvaturewear_Mode;
				int m_switch1491_g39827 = _Curvaturewear_Mode398_g39827;
				float4 break91_g39855 = _Curvaturewear_MaskingColor;
				float3 appendResult87_g39855 = (float3(break91_g39855.r , break91_g39855.g , break91_g39855.b));
				int m_switch1516_g39827 = _Curvaturewear_MaskingSource;
				float4 _Curvaturewear_GrungeMap1510_g39827 = tex2DNode1396_g39827;
				float4 m_MapGrunge1516_g39827 = _Curvaturewear_GrungeMap1510_g39827;
				float4 m_MapAlbedo1516_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1516_g39827 = float4switch1516_g39827( m_switch1516_g39827 , m_MapGrunge1516_g39827 , m_MapAlbedo1516_g39827 );
				float4 break89_g39855 = localfloat4switch1516_g39827;
				float3 appendResult88_g39855 = (float3(break89_g39855.x , break89_g39855.y , break89_g39855.z));
				float4 temp_cast_42 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1491_g39827 = temp_cast_42;
				float4 temp_cast_43 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1491_g39827 = temp_cast_43;
				float4 temp_cast_44 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1491_g39827 = temp_cast_44;
				float4 localfloat4switch1491_g39827 = float4switch1491_g39827( m_switch1491_g39827 , m_Off1491_g39827 , m_Active1491_g39827 , m_ActiveInverted1491_g39827 );
				float4 temp_output_44_0_g39836 = localfloat4switch1491_g39827;
				float3 temp_output_40_0_g39836 = abs( WorldNormal );
				float WN_HORIZONTAL141_g39836 = ( ( temp_output_40_0_g39836 * temp_output_40_0_g39836 ).y + 0.07 );
				float4 lerpResult2_g39836 = lerp( temp_output_44_0_g39836 , _Vec , WN_HORIZONTAL141_g39836);
				float4 VECTOR_IN_MASKED112_g39836 = lerpResult2_g39836;
				float4 Vect_0019_g39836 = _Vec;
				float2 _SmoothX = float2(0,0.45);
				float3 temp_cast_45 = (0.5).xxx;
				float3 break24_g39836 = ( temp_output_40_0_g39836 - temp_cast_45 );
				float WN_X134_g39836 = break24_g39836.x;
				float smoothstepResult8_g39836 = smoothstep( _SmoothX.x , _SmoothX.y , ( WN_X134_g39836 + 1.25 ));
				float smoothstepResult22_g39836 = smoothstep( _SmoothX.x , _SmoothX.y , ( -WN_X134_g39836 + 0.02 ));
				float lerpResult11_g39836 = lerp( 0.0 , smoothstepResult8_g39836 , smoothstepResult22_g39836);
				float4 lerpResult14_g39836 = lerp( VECTOR_IN_MASKED112_g39836 , Vect_0019_g39836 , lerpResult11_g39836);
				float4 MASK_VERTICAL_X159_g39836 = lerpResult14_g39836;
				float2 _SmoothY = float2(0,0.85);
				float WN_Y135_g39836 = break24_g39836.y;
				float smoothstepResult29_g39836 = smoothstep( _SmoothY.x , _SmoothY.y , ( -WN_Y135_g39836 + 0.35 ));
				float4 lerpResult6_g39836 = lerp( VECTOR_IN_MASKED112_g39836 , Vect_0019_g39836 , smoothstepResult29_g39836);
				float4 MASK_VERTICAL_Y157_g39836 = lerpResult6_g39836;
				float2 _SmoothZ = float2(1.8,1.4);
				float WN_Z136_g39836 = break24_g39836.z;
				float smoothstepResult26_g39836 = smoothstep( _SmoothZ.x , _SmoothZ.y , ( WN_Z136_g39836 + 2.06 ));
				float lerpResult34_g39836 = lerp( smoothstepResult26_g39836 , 0.0 , ( -WN_Z136_g39836 + 2.05 ));
				float4 lerpResult5_g39836 = lerp( Vect_0019_g39836 , VECTOR_IN_MASKED112_g39836 , lerpResult34_g39836);
				float4 MASK_VERTICAL_Z155_g39836 = lerpResult5_g39836;
				float4 temp_cast_46 = (_Curvaturewear_BlendStrength).xxxx;
				float4 temp_cast_47 = (_Curvaturewear_BlendHardness).xxxx;
				float4 smoothstepResult75_g39836 = smoothstep( temp_cast_39 , temp_cast_40 , (float4( 0,0,0,0 ) + (( 1.0 - ( MASK_VERTICAL_X159_g39836 + MASK_VERTICAL_Y157_g39836 + MASK_VERTICAL_Z155_g39836 ) ) - temp_cast_46) * (temp_cast_47 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_46)));
				float MASK_CURVATURE401_g39827 = (( ( smoothstepResult75_g39836 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x;
				float4 lerpResult860_g39827 = lerp( FINAL_MASK_CURVATURE586_g39827 , localfloat4switch1377_g39827 , MASK_CURVATURE401_g39827);
				float4 m_Active1380_g39827 = lerpResult860_g39827;
				float4 m_ActiveInverted1380_g39827 = lerpResult860_g39827;
				float4 localfloat4switch1380_g39827 = float4switch1380_g39827( m_switch1380_g39827 , m_Off1380_g39827 , m_Active1380_g39827 , m_ActiveInverted1380_g39827 );
				
				int m_switch1416_g39827 = _Curvaturewear_Mode;
				int m_switch1413_g39827 = _Verticalwear_Mode;
				int m_switch1329_g39827 = _Horizontalwear_Mode;
				float4 NORMAL_RGB1382_g37947 = SAMPLE_TEXTURE2D( _BumpMap, sampler_trilinear_repeat, UV40_g39808 );
				float4 temp_output_1_0_g39806 = NORMAL_RGB1382_g37947;
				float temp_output_8_0_g39806 = _NormalStrength;
				float3 unpack52_g39806 = UnpackNormalScale( temp_output_1_0_g39806, temp_output_8_0_g39806 );
				unpack52_g39806.z = lerp( 1, unpack52_g39806.z, saturate(temp_output_8_0_g39806) );
				float3 temp_output_1519_59_g37947 = unpack52_g39806;
				float3 NORMAL_IN42_g39824 = temp_output_1519_59_g37947;
				int m_switch56_g39824 = _NormalMode;
				float3 m_Default56_g39824 = float3(1,1,1);
				float3 m_Flip56_g39824 = float3(-1,-1,-1);
				float3 m_Mirror56_g39824 = float3(1,1,-1);
				float3 localfloat3switch56_g39824 = float3switch56_g39824( m_switch56_g39824 , m_Default56_g39824 , m_Flip56_g39824 , m_Mirror56_g39824 );
				float3 switchResult58_g39824 = (((ase_vface>0)?(NORMAL_IN42_g39824):(( NORMAL_IN42_g39824 * localfloat3switch56_g39824 ))));
				int m_switch37_g39824 = _NormalMode;
				float3 m_Default37_g39824 = NORMAL_IN42_g39824;
				float3 m_Flip37_g39824 = ( NORMAL_IN42_g39824 * ase_vface );
				float3 break33_g39824 = NORMAL_IN42_g39824;
				float3 appendResult41_g39824 = (float3(break33_g39824.x , break33_g39824.y , ( break33_g39824.z * ase_vface )));
				float3 m_Mirror37_g39824 = appendResult41_g39824;
				float3 localfloat3switch37_g39824 = float3switch37_g39824( m_switch37_g39824 , m_Default37_g39824 , m_Flip37_g39824 , m_Mirror37_g39824 );
				float3 temp_output_1610_30_g37947 = localfloat3switch37_g39824;
				float3 NORMAL_OUT1491_g37947 = temp_output_1610_30_g37947;
				float3 temp_output_16_0_g39796 = NORMAL_OUT1491_g37947;
				float3 tanToWorld0 = float3( WorldTangent.x, WorldBiTangent.x, WorldNormal.x );
				float3 tanToWorld1 = float3( WorldTangent.y, WorldBiTangent.y, WorldNormal.y );
				float3 tanToWorld2 = float3( WorldTangent.z, WorldBiTangent.z, WorldNormal.z );
				float3 tanNormal134_g39796 = temp_output_16_0_g39796;
				float3 worldNormal134_g39796 = float3(dot(tanToWorld0,tanNormal134_g39796), dot(tanToWorld1,tanNormal134_g39796), dot(tanToWorld2,tanNormal134_g39796));
				float3 NORMAL_TANGENTSPACE135_g39796 = worldNormal134_g39796;
				float3 WorldPosition60_g39796 = NORMAL_TANGENTSPACE135_g39796;
				float3 localAdditionalLightsFlat60_g39796 = AdditionalLightsFlat( WorldPosition60_g39796 );
				float3 ADDITIONAL_LIGHT1212_g37947 = localAdditionalLightsFlat60_g39796;
				float3 temp_output_8_0_g39827 = ( switchResult58_g39824 + ADDITIONAL_LIGHT1212_g37947 );
				float3 m_Off1329_g39827 = temp_output_8_0_g39827;
				float4 temp_output_1_0_g39840 = SAMPLE_TEXTURE2D( _Horizontalwear_GrungeNormal, sampler_trilinear_repeat, texCoord1342_g39827 );
				float temp_output_8_0_g39840 = ( _Horizontalwear_NormalMultiplier * _Horizontalwear_BlendStrength );
				float3 unpack52_g39840 = UnpackNormalScale( temp_output_1_0_g39840, temp_output_8_0_g39840 );
				unpack52_g39840.z = lerp( 1, unpack52_g39840.z, saturate(temp_output_8_0_g39840) );
				float3 FINAL_MASKNORMAL_HORIZONTAL803_g39827 = unpack52_g39840;
				float3 lerpResult1335_g39827 = lerp( temp_output_8_0_g39827 , FINAL_MASKNORMAL_HORIZONTAL803_g39827 , MASK_HORIZONTAL808_g39827);
				float3 m_Active1329_g39827 = lerpResult1335_g39827;
				float3 m_ActiveInverted1329_g39827 = lerpResult1335_g39827;
				float3 localfloat3switch1329_g39827 = float3switch1329_g39827( m_switch1329_g39827 , m_Off1329_g39827 , m_Active1329_g39827 , m_ActiveInverted1329_g39827 );
				float3 m_Off1413_g39827 = localfloat3switch1329_g39827;
				float4 temp_output_1_0_g39838 = SAMPLE_TEXTURE2D( _Verticalwear_GrungeNormal, sampler_trilinear_repeat, texCoord1388_g39827 );
				float temp_output_8_0_g39838 = ( _Verticalwear_NormalMultiplier * _Verticalwear_BlendStrength );
				float3 unpack52_g39838 = UnpackNormalScale( temp_output_1_0_g39838, temp_output_8_0_g39838 );
				unpack52_g39838.z = lerp( 1, unpack52_g39838.z, saturate(temp_output_8_0_g39838) );
				float3 FINAL_MASKNORMAL_VERTICAL630_g39827 = unpack52_g39838;
				float3 lerpResult1414_g39827 = lerp( localfloat3switch1329_g39827 , FINAL_MASKNORMAL_VERTICAL630_g39827 , MASK_VERTICAL602_g39827);
				float3 m_Active1413_g39827 = lerpResult1414_g39827;
				float3 m_ActiveInverted1413_g39827 = lerpResult1414_g39827;
				float3 localfloat3switch1413_g39827 = float3switch1413_g39827( m_switch1413_g39827 , m_Off1413_g39827 , m_Active1413_g39827 , m_ActiveInverted1413_g39827 );
				float3 m_Off1416_g39827 = localfloat3switch1413_g39827;
				float3 NORMAL_IN1546_g39827 = temp_output_8_0_g39827;
				float4 temp_output_1_0_g39832 = SAMPLE_TEXTURE2D( _Curvaturewear_GrungeNormal, sampler_trilinear_repeat, texCoord1389_g39827 );
				float temp_output_8_0_g39832 = ( _Curvaturewear_NormalMultiplier * _Curvaturewear_BlendStrength );
				float3 unpack52_g39832 = UnpackNormalScale( temp_output_1_0_g39832, temp_output_8_0_g39832 );
				unpack52_g39832.z = lerp( 1, unpack52_g39832.z, saturate(temp_output_8_0_g39832) );
				float3 lerpResult1537_g39827 = lerp( NORMAL_IN1546_g39827 , unpack52_g39832 , _Curvaturewear_BlendMix);
				float3 FINAL_MASKNORMAL_CURVATURE599_g39827 = lerpResult1537_g39827;
				float3 lerpResult1417_g39827 = lerp( FINAL_MASKNORMAL_CURVATURE599_g39827 , localfloat3switch1413_g39827 , MASK_CURVATURE401_g39827);
				float3 m_Active1416_g39827 = lerpResult1417_g39827;
				float3 m_ActiveInverted1416_g39827 = lerpResult1417_g39827;
				float3 localfloat3switch1416_g39827 = float3switch1416_g39827( m_switch1416_g39827 , m_Off1416_g39827 , m_Active1416_g39827 , m_ActiveInverted1416_g39827 );
				
				int m_switch1436_g39827 = _Curvaturewear_Mode;
				int m_switch1435_g39827 = _Verticalwear_Mode;
				int m_switch1434_g39827 = _Horizontalwear_Mode;
				float4 MASK_B1377_g37947 = SAMPLE_TEXTURE2D( _MetallicGlossMap, sampler_trilinear_repeat, UV40_g39808 );
				float4 temp_output_977_0_g39827 = ( _MetallicStrength * MASK_B1377_g37947 );
				float m_Off1434_g39827 = temp_output_977_0_g39827.x;
				float FINAL_MASKMETALLIC_HORIZONTAL1026_g39827 = _Horizontalwear_Metallic;
				float4 temp_cast_55 = (FINAL_MASKMETALLIC_HORIZONTAL1026_g39827).xxxx;
				float4 lerpResult1439_g39827 = lerp( temp_output_977_0_g39827 , temp_cast_55 , MASK_HORIZONTAL808_g39827);
				float m_Active1434_g39827 = lerpResult1439_g39827.x;
				float m_ActiveInverted1434_g39827 = lerpResult1439_g39827.x;
				float localfloatswitch1434_g39827 = floatswitch1434_g39827( m_switch1434_g39827 , m_Off1434_g39827 , m_Active1434_g39827 , m_ActiveInverted1434_g39827 );
				float m_Off1435_g39827 = localfloatswitch1434_g39827;
				float FINAL_MASKMETALLIC_VERTICAL1024_g39827 = _Verticalwear_Metallic;
				float lerpResult1438_g39827 = lerp( localfloatswitch1434_g39827 , FINAL_MASKMETALLIC_VERTICAL1024_g39827 , MASK_VERTICAL602_g39827);
				float m_Active1435_g39827 = lerpResult1438_g39827;
				float m_ActiveInverted1435_g39827 = lerpResult1438_g39827;
				float localfloatswitch1435_g39827 = floatswitch1435_g39827( m_switch1435_g39827 , m_Off1435_g39827 , m_Active1435_g39827 , m_ActiveInverted1435_g39827 );
				float m_Off1436_g39827 = localfloatswitch1435_g39827;
				float4 METALLIC_IN1555_g39827 = temp_output_977_0_g39827;
				float4 temp_cast_58 = (_Curvaturewear_Metallic).xxxx;
				float4 lerpResult1553_g39827 = lerp( METALLIC_IN1555_g39827 , temp_cast_58 , _Curvaturewear_BlendMix);
				float4 FINAL_MASKMETALLIC_CURVATURE1022_g39827 = lerpResult1553_g39827;
				float4 temp_cast_59 = (localfloatswitch1435_g39827).xxxx;
				float4 lerpResult1437_g39827 = lerp( FINAL_MASKMETALLIC_CURVATURE1022_g39827 , temp_cast_59 , MASK_CURVATURE401_g39827);
				float m_Active1436_g39827 = lerpResult1437_g39827.x;
				float m_ActiveInverted1436_g39827 = lerpResult1437_g39827.x;
				float localfloatswitch1436_g39827 = floatswitch1436_g39827( m_switch1436_g39827 , m_Off1436_g39827 , m_Active1436_g39827 , m_ActiveInverted1436_g39827 );
				
				int m_switch1433_g39827 = _Curvaturewear_Mode;
				int m_switch1432_g39827 = _Verticalwear_Mode;
				int m_switch1075_g39827 = _Horizontalwear_Mode;
				int m_switch188_g39758 = _SmoothnessModeCutout;
				float3 tanNormal1608_g37947 = NORMAL_OUT1491_g37947;
				float3 worldNormal1608_g37947 = normalize( float3(dot(tanToWorld0,tanNormal1608_g37947), dot(tanToWorld1,tanNormal1608_g37947), dot(tanToWorld2,tanNormal1608_g37947)) );
				float3 temp_output_4_0_g39758 = worldNormal1608_g37947;
				float3 temp_output_178_0_g39758 = ddx( temp_output_4_0_g39758 );
				float dotResult195_g39758 = dot( temp_output_178_0_g39758 , temp_output_178_0_g39758 );
				float3 temp_output_175_0_g39758 = ddy( temp_output_4_0_g39758 );
				float dotResult201_g39758 = dot( temp_output_175_0_g39758 , temp_output_175_0_g39758 );
				float temp_output_196_0_g39758 = sqrt( saturate( ( ( _SmoothnessStrength * _SmoothnessStrength ) + min( ( ( _SmoothnessVariance * ( dotResult195_g39758 + dotResult201_g39758 ) ) * 2.0 ) , ( _SmoothnessThreshold * _SmoothnessThreshold ) ) ) ) );
				float ALBEDO_R172_g37947 = tex2DNode63_g39808.r;
				float4 temp_cast_62 = (ALBEDO_R172_g37947).xxxx;
				float4 temp_output_9_0_g39758 = temp_cast_62;
				float4 m_Standard188_g39758 = ( min( temp_output_196_0_g39758 , 0.5 ) * ( 1.0 - ( temp_output_9_0_g39758 * float4( 0.79,0,0,0 ) ) ) );
				float4 temp_output_187_0_g39758 = ( 1.0 - temp_output_9_0_g39758 );
				float4 m_Geometric188_g39758 = ( temp_output_196_0_g39758 * temp_output_187_0_g39758 );
				float ALBEDO_A926_g37947 = tex2DNode63_g39808.a;
				int m_switch1287_g37947 = _GlancingClipMode;
				float m_Off1287_g37947 = 1.0;
				float3 normalizeResult1294_g37947 = normalize( cross( ddx( WorldPosition ) , ddy( WorldPosition ) ) );
				float dotResult1301_g37947 = dot( WorldViewDirection , normalizeResult1294_g37947 );
				float temp_output_1321_0_g37947 = ( 1.0 - abs( dotResult1301_g37947 ) );
				float m_Active1287_g37947 = ( 1.0 - ( temp_output_1321_0_g37947 * temp_output_1321_0_g37947 ) );
				float localfloatswitch1287_g37947 = floatswitch1287_g37947( m_switch1287_g37947 , m_Off1287_g37947 , m_Active1287_g37947 );
				float OPACITY_OUTMASK1277_g37947 = localfloatswitch1287_g37947;
				float CLIP_OPACITY200_g37947 = ( ALBEDO_A926_g37947 * OPACITY_OUTMASK1277_g37947 );
				float4 temp_cast_63 = (CLIP_OPACITY200_g37947).xxxx;
				float4 m_GeometricFade188_g39758 = ( temp_output_196_0_g39758 * temp_output_187_0_g39758 * temp_cast_63 );
				int m_switch189_g39758 = _SmoothnessType;
				float4 MASK_G158_g37947 = SAMPLE_TEXTURE2D( _SmoothnessMap, sampler_trilinear_repeat, UV40_g39808 );
				float4 temp_output_83_0_g39758 = MASK_G158_g37947;
				float4 m_Smoothness189_g39758 = temp_output_83_0_g39758;
				float4 m_Roughness189_g39758 = ( 1.0 - temp_output_83_0_g39758 );
				float4 localfloat4switch189_g39758 = float4switch189_g39758( m_switch189_g39758 , m_Smoothness189_g39758 , m_Roughness189_g39758 );
				float4 m_Texture188_g39758 = ( temp_output_196_0_g39758 * localfloat4switch189_g39758 );
				float4 localfloat4switch188_g39758 = float4switch188_g39758( m_switch188_g39758 , m_Standard188_g39758 , m_Geometric188_g39758 , m_GeometricFade188_g39758 , m_Texture188_g39758 );
				float4 temp_output_1324_33_g37947 = localfloat4switch188_g39758;
				float4 color1225_g37947 = IsGammaSpace() ? float4(1,1,1,1) : float4(1,1,1,1);
				float4 MASK_R1378_g37947 = SAMPLE_TEXTURE2D( _OcclusionMap, sampler_trilinear_repeat, UV40_g39808 );
				float4 temp_cast_65 = (IN.ase_color.a).xxxx;
				float4 lerpResult28_g37947 = lerp( MASK_R1378_g37947 , temp_cast_65 , (float)_OcclusionSourceMode);
				float4 lerpResult7_g37947 = lerp( color1225_g37947 , lerpResult28_g37947 , _OcclusionStrengthAO);
				float4 temp_output_989_0_g39827 = ( temp_output_1324_33_g37947 * lerpResult7_g37947 );
				float m_Off1075_g39827 = temp_output_989_0_g39827.x;
				float FINAL_MASKSMOOTHNESS_HORIZONTAL1025_g39827 = _Horizontalwear_Smoothness;
				float4 temp_cast_69 = (FINAL_MASKSMOOTHNESS_HORIZONTAL1025_g39827).xxxx;
				float4 lerpResult1030_g39827 = lerp( temp_output_989_0_g39827 , temp_cast_69 , MASK_HORIZONTAL808_g39827);
				float m_Active1075_g39827 = lerpResult1030_g39827.x;
				float m_ActiveInverted1075_g39827 = lerpResult1030_g39827.x;
				float localfloatswitch1075_g39827 = floatswitch1075_g39827( m_switch1075_g39827 , m_Off1075_g39827 , m_Active1075_g39827 , m_ActiveInverted1075_g39827 );
				float m_Off1432_g39827 = localfloatswitch1075_g39827;
				float FINAL_MASKSMOOTHNESS_VERTICAL1023_g39827 = _Verticalwear_Smoothness;
				float lerpResult1028_g39827 = lerp( localfloatswitch1075_g39827 , FINAL_MASKSMOOTHNESS_VERTICAL1023_g39827 , MASK_VERTICAL602_g39827);
				float m_Active1432_g39827 = lerpResult1028_g39827;
				float m_ActiveInverted1432_g39827 = lerpResult1028_g39827;
				float localfloatswitch1432_g39827 = floatswitch1432_g39827( m_switch1432_g39827 , m_Off1432_g39827 , m_Active1432_g39827 , m_ActiveInverted1432_g39827 );
				float m_Off1433_g39827 = localfloatswitch1432_g39827;
				float4 SMOOTHNESS_IN1554_g39827 = temp_output_989_0_g39827;
				float4 temp_cast_72 = (_Curvaturewear_Smoothness).xxxx;
				float4 lerpResult1552_g39827 = lerp( SMOOTHNESS_IN1554_g39827 , temp_cast_72 , _Curvaturewear_BlendMix);
				float4 FINAL_MASKSMOOTHNESS_CURVATURE1021_g39827 = lerpResult1552_g39827;
				float4 temp_cast_73 = (localfloatswitch1432_g39827).xxxx;
				float4 lerpResult1031_g39827 = lerp( FINAL_MASKSMOOTHNESS_CURVATURE1021_g39827 , temp_cast_73 , MASK_CURVATURE401_g39827);
				float m_Active1433_g39827 = lerpResult1031_g39827.x;
				float m_ActiveInverted1433_g39827 = lerpResult1031_g39827.x;
				float localfloatswitch1433_g39827 = floatswitch1433_g39827( m_switch1433_g39827 , m_Off1433_g39827 , m_Active1433_g39827 , m_ActiveInverted1433_g39827 );
				
				float AlphaCutoffBias1289_g37947 = _AlphaCutoffBias;
				
				float3 Albedo = localfloat4switch1380_g39827.xyz;
				float3 Normal = localfloat3switch1416_g39827;
				float3 Emission = 0;
				float3 Specular = 0.5;
				float Metallic = localfloatswitch1436_g39827;
				float Smoothness = localfloatswitch1433_g39827;
				float Occlusion = saturate( lerpResult7_g37947 ).r;
				float Alpha = ( ALBEDO_A926_g37947 * OPACITY_OUTMASK1277_g37947 );
				float AlphaClipThreshold = AlphaCutoffBias1289_g37947;
				float AlphaClipThresholdShadow = _AlphaCutoffBiasShadow;
				float3 BakedGI = 0;
				float3 RefractionColor = 1;
				float RefractionIndex = 1;
				float3 Transmission = 1;
				float3 Translucency = 1;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				InputData inputData;
				inputData.positionWS = WorldPosition;
				inputData.viewDirectionWS = WorldViewDirection;
				inputData.shadowCoord = ShadowCoords;

				#ifdef _NORMALMAP
					#if _NORMAL_DROPOFF_TS
					inputData.normalWS = TransformTangentToWorld(Normal, half3x3( WorldTangent, WorldBiTangent, WorldNormal ));
					#elif _NORMAL_DROPOFF_OS
					inputData.normalWS = TransformObjectToWorldNormal(Normal);
					#elif _NORMAL_DROPOFF_WS
					inputData.normalWS = Normal;
					#endif
					inputData.normalWS = NormalizeNormalPerPixel(inputData.normalWS);
				#else
					inputData.normalWS = WorldNormal;
				#endif

				#ifdef ASE_FOG
					inputData.fogCoord = IN.fogFactorAndVertexLight.x;
				#endif

				inputData.vertexLighting = IN.fogFactorAndVertexLight.yzw;
				#if defined(ENABLE_TERRAIN_PERPIXEL_NORMAL)
					float3 SH = SampleSH(inputData.normalWS.xyz);
				#else
					float3 SH = IN.lightmapUVOrVertexSH.xyz;
				#endif

				inputData.bakedGI = SAMPLE_GI( IN.lightmapUVOrVertexSH.xy, SH, inputData.normalWS );
				#ifdef _ASE_BAKEDGI
					inputData.bakedGI = BakedGI;
				#endif
				half4 color = UniversalFragmentPBR(
					inputData, 
					Albedo, 
					Metallic, 
					Specular, 
					Smoothness, 
					Occlusion, 
					Emission, 
					Alpha);

				#ifdef _TRANSMISSION_ASE
				{
					float shadow = _TransmissionShadow;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );
					half3 mainTransmission = max(0 , -dot(inputData.normalWS, mainLight.direction)) * mainAtten * Transmission;
					color.rgb += Albedo * mainTransmission;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 transmission = max(0 , -dot(inputData.normalWS, light.direction)) * atten * Transmission;
							color.rgb += Albedo * transmission;
						}
					#endif
				}
				#endif

				#ifdef _TRANSLUCENCY_ASE
				{
					float shadow = _TransShadow;
					float normal = _TransNormal;
					float scattering = _TransScattering;
					float direct = _TransDirect;
					float ambient = _TransAmbient;
					float strength = _TransStrength;

					Light mainLight = GetMainLight( inputData.shadowCoord );
					float3 mainAtten = mainLight.color * mainLight.distanceAttenuation;
					mainAtten = lerp( mainAtten, mainAtten * mainLight.shadowAttenuation, shadow );

					half3 mainLightDir = mainLight.direction + inputData.normalWS * normal;
					half mainVdotL = pow( saturate( dot( inputData.viewDirectionWS, -mainLightDir ) ), scattering );
					half3 mainTranslucency = mainAtten * ( mainVdotL * direct + inputData.bakedGI * ambient ) * Translucency;
					color.rgb += Albedo * mainTranslucency * strength;

					#ifdef _ADDITIONAL_LIGHTS
						int transPixelLightCount = GetAdditionalLightsCount();
						for (int i = 0; i < transPixelLightCount; ++i)
						{
							Light light = GetAdditionalLight(i, inputData.positionWS);
							float3 atten = light.color * light.distanceAttenuation;
							atten = lerp( atten, atten * light.shadowAttenuation, shadow );

							half3 lightDir = light.direction + inputData.normalWS * normal;
							half VdotL = pow( saturate( dot( inputData.viewDirectionWS, -lightDir ) ), scattering );
							half3 translucency = atten * ( VdotL * direct + inputData.bakedGI * ambient ) * Translucency;
							color.rgb += Albedo * translucency * strength;
						}
					#endif
				}
				#endif

				#ifdef _REFRACTION_ASE
					float4 projScreenPos = ScreenPos / ScreenPos.w;
					float3 refractionOffset = ( RefractionIndex - 1.0 ) * mul( UNITY_MATRIX_V, float4( WorldNormal, 0 ) ).xyz * ( 1.0 - dot( WorldNormal, WorldViewDirection ) );
					projScreenPos.xy += refractionOffset.xy;
					float3 refraction = SHADERGRAPH_SAMPLE_SCENE_COLOR( projScreenPos.xy ) * RefractionColor;
					color.rgb = lerp( refraction, color.rgb, color.a );
					color.a = 1;
				#endif

				#ifdef ASE_FINAL_COLOR_ALPHA_MULTIPLY
					color.rgb *= color.a;
				#endif

				#ifdef ASE_FOG
					#ifdef TERRAIN_SPLAT_ADDPASS
						color.rgb = MixFogColor(color.rgb, half3( 0, 0, 0 ), IN.fogFactorAndVertexLight.x );
					#else
						color.rgb = MixFog(color.rgb, IN.fogFactorAndVertexLight.x);
					#endif
				#endif
				
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif

				return color;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "ShadowCaster"
			Tags { "LightMode"="ShadowCaster" }

			ZWrite On
			ZTest LEqual
			AlphaToMask Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#define _ALPHATEST_SHADOW_ON 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70503
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_SHADOWCASTER

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:SetupNatureRenderer forwardadd
			#pragma multi_compile GPU_FRUSTUM_ON __
			#include "Nature Renderer.cginc"
			#pragma multi_compile_local _ NATURE_RENDERER


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Verticalwear_MaskingColor;
			float4 _Horizontalwear_Tint;
			float4 _Curvaturewear_Tint;
			float4 _Horizontalwear_MaskingColor;
			float4 _Verticalwear_Tint;
			float4 _Gradient_Top;
			float4 _Gradient_Bottom;
			float4 _Gradient_BottomSide;
			float4 _Gradient_TintCurvature;
			float4 _Gradient_TopSide;
			float4 _Curvaturewear_MaskingColor;
			float4 _Color;
			float _Curvaturewear_BlendMix;
			half _NormalStrength;
			half _Curvaturewear_Saturation;
			float _Curvaturewear_OffsetY;
			int _Curvaturewear_MaskingSource;
			half _Curvaturewear_MaskingColorRange;
			half _Curvaturewear_MaskingColorFuzziness;
			float _Curvaturewear_BlendStrength;
			float _Curvaturewear_BlendHardness;
			float _Curvaturewear_OffsetX;
			float _Curvaturewear_TilingY;
			float _Curvaturewear_TilingX;
			int _NormalMode;
			float _MetallicStrength;
			float _Verticalwear_NormalMultiplier;
			float _Curvaturewear_Smoothness;
			float _Verticalwear_Smoothness;
			float _Horizontalwear_Smoothness;
			float _OcclusionStrengthAO;
			int _OcclusionSourceMode;
			int _SmoothnessType;
			int _GlancingClipMode;
			float _SmoothnessThreshold;
			float _SmoothnessVariance;
			float _SmoothnessStrength;
			int _SmoothnessModeCutout;
			float _Curvaturewear_Metallic;
			float _Verticalwear_Metallic;
			float _Horizontalwear_Metallic;
			float _Curvaturewear_NormalMultiplier;
			float _Horizontalwear_NormalMultiplier;
			float _Verticalwear_BlendMix;
			int _ColorMask;
			half _Verticalwear_MaskingColorFuzziness;
			float _OffsetY;
			float _OffsetX;
			float _TilingY;
			float _TilingX;
			float _Gradient_Contrast;
			float _Gradient_Ramp;
			half _Brightness;
			float _Gradient_Height;
			int _Horizontalwear_Mode;
			int _Verticalwear_Mode;
			int _Curvaturewear_Mode;
			int _ZWriteMode;
			float _AlphatoCoverage;
			int _CullMode;
			int _Gradient_Mode;
			float _Horizontalwear_TilingX;
			float _Horizontalwear_TilingY;
			float _Horizontalwear_OffsetX;
			half _Verticalwear_MaskingColorRange;
			int _Verticalwear_MaskingSource;
			half _AlphaCutoffBias;
			half _Verticalwear_Saturation;
			float _Verticalwear_OffsetY;
			float _Verticalwear_OffsetX;
			float _Verticalwear_TilingY;
			float _Verticalwear_TilingX;
			float _Horizontalwear_BlendMix;
			float _Horizontalwear_BlendStrength;
			half _Horizontalwear_MaskingColorFuzziness;
			half _Horizontalwear_MaskingColorRange;
			int _Horizontalwear_MaskingSource;
			half _Horizontalwear_Saturation;
			float _Horizontalwear_OffsetY;
			float _Verticalwear_BlendStrength;
			half _AlphaCutoffBiasShadow;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_trilinear_repeat);


			float4 mod289( float4 x )
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			
			float4 perm( float4 x )
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float SimpleNoise3D( float3 p )
			{
				 float3 a = floor(p);
				    float3 d = p - a;
				    d = d * d * (3.0 - 2.0 * d);
				 float4 b = a.xxyy + float4(0.0, 1.0, 0.0, 1.0);
				    float4 k1 = perm(b.xyxy);
				 float4 k2 = perm(k1.xyxy + b.zzww);
				    float4 c = k2 + a.zzzz;
				    float4 k3 = perm(c);
				    float4 k4 = perm(c + 1.0);
				    float4 o1 = frac(k3 * (1.0 / 41.0));
				 float4 o2 = frac(k4 * (1.0 / 41.0));
				    float4 o3 = o2 * d.z + o1 * (1.0 - d.z);
				    float2 o4 = o3.yw * d.x + o3.xz * (1.0 - d.x);
				    return o4.y * d.y + o4.x * (1.0 - d.y);
			}
			
			float floatswitch1287_g37947( int m_switch, float m_Off, float m_Active )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else
				return float(0);
			}
			

			float3 _LightDirection;

			VertexOutput VertexFunction( VertexInput v )
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif
				float3 normalWS = TransformObjectToWorldDir(v.ase_normal);

				float4 clipPos = TransformWorldToHClip( ApplyShadowBias( positionWS, normalWS, _LightDirection ) );

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = clipPos;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif

			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );
				
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 appendResult168_g37947 = (float2(_TilingX , _TilingY));
				float2 appendResult167_g37947 = (float2(_OffsetX , _OffsetY));
				float2 texCoord2_g39802 = IN.ase_texcoord2.xy * appendResult168_g37947 + appendResult167_g37947;
				float2 appendResult6_g39802 = (float2(texCoord2_g39802.x , texCoord2_g39802.y));
				float2 OUT_UV213_g37947 = appendResult6_g39802;
				float2 UV40_g39808 = OUT_UV213_g37947;
				float4 tex2DNode63_g39808 = SAMPLE_TEXTURE2D( _MainTex, sampler_trilinear_repeat, UV40_g39808 );
				float ALBEDO_A926_g37947 = tex2DNode63_g39808.a;
				int m_switch1287_g37947 = _GlancingClipMode;
				float m_Off1287_g37947 = 1.0;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float3 normalizeResult1294_g37947 = normalize( cross( ddx( WorldPosition ) , ddy( WorldPosition ) ) );
				float dotResult1301_g37947 = dot( ase_worldViewDir , normalizeResult1294_g37947 );
				float temp_output_1321_0_g37947 = ( 1.0 - abs( dotResult1301_g37947 ) );
				float m_Active1287_g37947 = ( 1.0 - ( temp_output_1321_0_g37947 * temp_output_1321_0_g37947 ) );
				float localfloatswitch1287_g37947 = floatswitch1287_g37947( m_switch1287_g37947 , m_Off1287_g37947 , m_Active1287_g37947 );
				float OPACITY_OUTMASK1277_g37947 = localfloatswitch1287_g37947;
				
				float AlphaCutoffBias1289_g37947 = _AlphaCutoffBias;
				
				float Alpha = ( ALBEDO_A926_g37947 * OPACITY_OUTMASK1277_g37947 );
				float AlphaClipThreshold = AlphaCutoffBias1289_g37947;
				float AlphaClipThresholdShadow = _AlphaCutoffBiasShadow;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					#ifdef _ALPHATEST_SHADOW_ON
						clip(Alpha - AlphaClipThresholdShadow);
					#else
						clip(Alpha - AlphaClipThreshold);
					#endif
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
					outputDepth = DepthValue;
				#endif
				return 0;
			}

			ENDHLSL
		}

		
		Pass
		{
			
			Name "DepthOnly"
			Tags { "LightMode"="DepthOnly" }

			ZWrite On
			ColorMask 0
			AlphaToMask Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#define _ALPHATEST_SHADOW_ON 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70503
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_DEPTHONLY

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:SetupNatureRenderer forwardadd
			#pragma multi_compile GPU_FRUSTUM_ON __
			#include "Nature Renderer.cginc"
			#pragma multi_compile_local _ NATURE_RENDERER


			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Verticalwear_MaskingColor;
			float4 _Horizontalwear_Tint;
			float4 _Curvaturewear_Tint;
			float4 _Horizontalwear_MaskingColor;
			float4 _Verticalwear_Tint;
			float4 _Gradient_Top;
			float4 _Gradient_Bottom;
			float4 _Gradient_BottomSide;
			float4 _Gradient_TintCurvature;
			float4 _Gradient_TopSide;
			float4 _Curvaturewear_MaskingColor;
			float4 _Color;
			float _Curvaturewear_BlendMix;
			half _NormalStrength;
			half _Curvaturewear_Saturation;
			float _Curvaturewear_OffsetY;
			int _Curvaturewear_MaskingSource;
			half _Curvaturewear_MaskingColorRange;
			half _Curvaturewear_MaskingColorFuzziness;
			float _Curvaturewear_BlendStrength;
			float _Curvaturewear_BlendHardness;
			float _Curvaturewear_OffsetX;
			float _Curvaturewear_TilingY;
			float _Curvaturewear_TilingX;
			int _NormalMode;
			float _MetallicStrength;
			float _Verticalwear_NormalMultiplier;
			float _Curvaturewear_Smoothness;
			float _Verticalwear_Smoothness;
			float _Horizontalwear_Smoothness;
			float _OcclusionStrengthAO;
			int _OcclusionSourceMode;
			int _SmoothnessType;
			int _GlancingClipMode;
			float _SmoothnessThreshold;
			float _SmoothnessVariance;
			float _SmoothnessStrength;
			int _SmoothnessModeCutout;
			float _Curvaturewear_Metallic;
			float _Verticalwear_Metallic;
			float _Horizontalwear_Metallic;
			float _Curvaturewear_NormalMultiplier;
			float _Horizontalwear_NormalMultiplier;
			float _Verticalwear_BlendMix;
			int _ColorMask;
			half _Verticalwear_MaskingColorFuzziness;
			float _OffsetY;
			float _OffsetX;
			float _TilingY;
			float _TilingX;
			float _Gradient_Contrast;
			float _Gradient_Ramp;
			half _Brightness;
			float _Gradient_Height;
			int _Horizontalwear_Mode;
			int _Verticalwear_Mode;
			int _Curvaturewear_Mode;
			int _ZWriteMode;
			float _AlphatoCoverage;
			int _CullMode;
			int _Gradient_Mode;
			float _Horizontalwear_TilingX;
			float _Horizontalwear_TilingY;
			float _Horizontalwear_OffsetX;
			half _Verticalwear_MaskingColorRange;
			int _Verticalwear_MaskingSource;
			half _AlphaCutoffBias;
			half _Verticalwear_Saturation;
			float _Verticalwear_OffsetY;
			float _Verticalwear_OffsetX;
			float _Verticalwear_TilingY;
			float _Verticalwear_TilingX;
			float _Horizontalwear_BlendMix;
			float _Horizontalwear_BlendStrength;
			half _Horizontalwear_MaskingColorFuzziness;
			half _Horizontalwear_MaskingColorRange;
			int _Horizontalwear_MaskingSource;
			half _Horizontalwear_Saturation;
			float _Horizontalwear_OffsetY;
			float _Verticalwear_BlendStrength;
			half _AlphaCutoffBiasShadow;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_trilinear_repeat);


			float4 mod289( float4 x )
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			
			float4 perm( float4 x )
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float SimpleNoise3D( float3 p )
			{
				 float3 a = floor(p);
				    float3 d = p - a;
				    d = d * d * (3.0 - 2.0 * d);
				 float4 b = a.xxyy + float4(0.0, 1.0, 0.0, 1.0);
				    float4 k1 = perm(b.xyxy);
				 float4 k2 = perm(k1.xyxy + b.zzww);
				    float4 c = k2 + a.zzzz;
				    float4 k3 = perm(c);
				    float4 k4 = perm(c + 1.0);
				    float4 o1 = frac(k3 * (1.0 / 41.0));
				 float4 o2 = frac(k4 * (1.0 / 41.0));
				    float4 o3 = o2 * d.z + o1 * (1.0 - d.z);
				    float2 o4 = o3.yw * d.x + o3.xz * (1.0 - d.x);
				    return o4.y * d.y + o4.x * (1.0 - d.y);
			}
			
			float floatswitch1287_g37947( int m_switch, float m_Off, float m_Active )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else
				return float(0);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.ase_texcoord2.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.zw = 0;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;
				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			#if defined(ASE_EARLY_Z_DEPTH_OPTIMIZE)
				#define ASE_SV_DEPTH SV_DepthLessEqual  
			#else
				#define ASE_SV_DEPTH SV_Depth
			#endif
			half4 frag(	VertexOutput IN 
						#ifdef ASE_DEPTH_WRITE_ON
						,out float outputDepth : ASE_SV_DEPTH
						#endif
						 ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				float2 appendResult168_g37947 = (float2(_TilingX , _TilingY));
				float2 appendResult167_g37947 = (float2(_OffsetX , _OffsetY));
				float2 texCoord2_g39802 = IN.ase_texcoord2.xy * appendResult168_g37947 + appendResult167_g37947;
				float2 appendResult6_g39802 = (float2(texCoord2_g39802.x , texCoord2_g39802.y));
				float2 OUT_UV213_g37947 = appendResult6_g39802;
				float2 UV40_g39808 = OUT_UV213_g37947;
				float4 tex2DNode63_g39808 = SAMPLE_TEXTURE2D( _MainTex, sampler_trilinear_repeat, UV40_g39808 );
				float ALBEDO_A926_g37947 = tex2DNode63_g39808.a;
				int m_switch1287_g37947 = _GlancingClipMode;
				float m_Off1287_g37947 = 1.0;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float3 normalizeResult1294_g37947 = normalize( cross( ddx( WorldPosition ) , ddy( WorldPosition ) ) );
				float dotResult1301_g37947 = dot( ase_worldViewDir , normalizeResult1294_g37947 );
				float temp_output_1321_0_g37947 = ( 1.0 - abs( dotResult1301_g37947 ) );
				float m_Active1287_g37947 = ( 1.0 - ( temp_output_1321_0_g37947 * temp_output_1321_0_g37947 ) );
				float localfloatswitch1287_g37947 = floatswitch1287_g37947( m_switch1287_g37947 , m_Off1287_g37947 , m_Active1287_g37947 );
				float OPACITY_OUTMASK1277_g37947 = localfloatswitch1287_g37947;
				
				float AlphaCutoffBias1289_g37947 = _AlphaCutoffBias;
				
				float Alpha = ( ALBEDO_A926_g37947 * OPACITY_OUTMASK1277_g37947 );
				float AlphaClipThreshold = AlphaCutoffBias1289_g37947;
				#ifdef ASE_DEPTH_WRITE_ON
				float DepthValue = 0;
				#endif

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				#ifdef LOD_FADE_CROSSFADE
					LODDitheringTransition( IN.clipPos.xyz, unity_LODFade.x );
				#endif
				#ifdef ASE_DEPTH_WRITE_ON
				outputDepth = DepthValue;
				#endif
				return 0;
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Meta"
			Tags { "LightMode"="Meta" }

			Cull Off

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#define _ALPHATEST_SHADOW_ON 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70503
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_META

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/MetaInput.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:SetupNatureRenderer forwardadd
			#pragma multi_compile GPU_FRUSTUM_ON __
			#include "Nature Renderer.cginc"
			#pragma multi_compile_local _ NATURE_RENDERER


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Verticalwear_MaskingColor;
			float4 _Horizontalwear_Tint;
			float4 _Curvaturewear_Tint;
			float4 _Horizontalwear_MaskingColor;
			float4 _Verticalwear_Tint;
			float4 _Gradient_Top;
			float4 _Gradient_Bottom;
			float4 _Gradient_BottomSide;
			float4 _Gradient_TintCurvature;
			float4 _Gradient_TopSide;
			float4 _Curvaturewear_MaskingColor;
			float4 _Color;
			float _Curvaturewear_BlendMix;
			half _NormalStrength;
			half _Curvaturewear_Saturation;
			float _Curvaturewear_OffsetY;
			int _Curvaturewear_MaskingSource;
			half _Curvaturewear_MaskingColorRange;
			half _Curvaturewear_MaskingColorFuzziness;
			float _Curvaturewear_BlendStrength;
			float _Curvaturewear_BlendHardness;
			float _Curvaturewear_OffsetX;
			float _Curvaturewear_TilingY;
			float _Curvaturewear_TilingX;
			int _NormalMode;
			float _MetallicStrength;
			float _Verticalwear_NormalMultiplier;
			float _Curvaturewear_Smoothness;
			float _Verticalwear_Smoothness;
			float _Horizontalwear_Smoothness;
			float _OcclusionStrengthAO;
			int _OcclusionSourceMode;
			int _SmoothnessType;
			int _GlancingClipMode;
			float _SmoothnessThreshold;
			float _SmoothnessVariance;
			float _SmoothnessStrength;
			int _SmoothnessModeCutout;
			float _Curvaturewear_Metallic;
			float _Verticalwear_Metallic;
			float _Horizontalwear_Metallic;
			float _Curvaturewear_NormalMultiplier;
			float _Horizontalwear_NormalMultiplier;
			float _Verticalwear_BlendMix;
			int _ColorMask;
			half _Verticalwear_MaskingColorFuzziness;
			float _OffsetY;
			float _OffsetX;
			float _TilingY;
			float _TilingX;
			float _Gradient_Contrast;
			float _Gradient_Ramp;
			half _Brightness;
			float _Gradient_Height;
			int _Horizontalwear_Mode;
			int _Verticalwear_Mode;
			int _Curvaturewear_Mode;
			int _ZWriteMode;
			float _AlphatoCoverage;
			int _CullMode;
			int _Gradient_Mode;
			float _Horizontalwear_TilingX;
			float _Horizontalwear_TilingY;
			float _Horizontalwear_OffsetX;
			half _Verticalwear_MaskingColorRange;
			int _Verticalwear_MaskingSource;
			half _AlphaCutoffBias;
			half _Verticalwear_Saturation;
			float _Verticalwear_OffsetY;
			float _Verticalwear_OffsetX;
			float _Verticalwear_TilingY;
			float _Verticalwear_TilingX;
			float _Horizontalwear_BlendMix;
			float _Horizontalwear_BlendStrength;
			half _Horizontalwear_MaskingColorFuzziness;
			half _Horizontalwear_MaskingColorRange;
			int _Horizontalwear_MaskingSource;
			half _Horizontalwear_Saturation;
			float _Horizontalwear_OffsetY;
			float _Verticalwear_BlendStrength;
			half _AlphaCutoffBiasShadow;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_trilinear_repeat);
			TEXTURE2D(_Horizontalwear_GrungeMap);
			TEXTURE2D(_Verticalwear_GrungeMap);
			TEXTURE2D(_Curvaturewear_GrungeMap);


			float4 mod289( float4 x )
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			
			float4 perm( float4 x )
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float SimpleNoise3D( float3 p )
			{
				 float3 a = floor(p);
				    float3 d = p - a;
				    d = d * d * (3.0 - 2.0 * d);
				 float4 b = a.xxyy + float4(0.0, 1.0, 0.0, 1.0);
				    float4 k1 = perm(b.xyxy);
				 float4 k2 = perm(k1.xyxy + b.zzww);
				    float4 c = k2 + a.zzzz;
				    float4 k3 = perm(c);
				    float4 k4 = perm(c + 1.0);
				    float4 o1 = frac(k3 * (1.0 / 41.0));
				 float4 o2 = frac(k4 * (1.0 / 41.0));
				    float4 o3 = o2 * d.z + o1 * (1.0 - d.z);
				    float2 o4 = o3.yw * d.x + o3.xz * (1.0 - d.x);
				    return o4.y * d.y + o4.x * (1.0 - d.y);
			}
			
			float4 float4switch143_g39811( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1506_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1476_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1269_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1512_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1400_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1377_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1516_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1491_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1380_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float floatswitch1287_g37947( int m_switch, float m_Off, float m_Active )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else
				return float(0);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				o.ase_texcoord3 = v.vertex;
				o.ase_texcoord4.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord4.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				o.clipPos = MetaVertexPosition( v.vertex, v.texcoord1.xy, v.texcoord1.xy, unity_LightmapST, unity_DynamicLightmapST );
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = o.clipPos;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 texcoord1 : TEXCOORD1;
				float4 texcoord2 : TEXCOORD2;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.texcoord1 = v.texcoord1;
				o.texcoord2 = v.texcoord2;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.texcoord1 = patch[0].texcoord1 * bary.x + patch[1].texcoord1 * bary.y + patch[2].texcoord1 * bary.z;
				o.texcoord2 = patch[0].texcoord2 * bary.x + patch[1].texcoord2 * bary.y + patch[2].texcoord2 * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				int m_switch1380_g39827 = _Curvaturewear_Mode;
				int m_switch1377_g39827 = _Verticalwear_Mode;
				int m_switch1269_g39827 = _Horizontalwear_Mode;
				int _Gradient_Mode161_g39811 = _Gradient_Mode;
				int m_switch143_g39811 = _Gradient_Mode161_g39811;
				float4 TINT147_g39811 = _Color;
				float4 m_Off143_g39811 = TINT147_g39811;
				float4 _Vector0 = float4(0.001,0.001,0.001,0.001);
				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float3 temp_output_275_0_g39811 = abs( ase_worldNormal );
				float WN_HORIZONTAL284_g39811 = ( ( temp_output_275_0_g39811 * temp_output_275_0_g39811 ).y + 0.07 );
				float4 lerpResult341_g39811 = lerp( _Gradient_TintCurvature , _Vector0 , WN_HORIZONTAL284_g39811);
				float4 VECTOR_IN_MASKED343_g39811 = lerpResult341_g39811;
				float4 Vect_001349_g39811 = _Vector0;
				float2 _Vector3 = float2(0,0.45);
				float3 temp_cast_2 = (0.5).xxx;
				float3 break277_g39811 = ( temp_output_275_0_g39811 - temp_cast_2 );
				float WN_X280_g39811 = break277_g39811.x;
				float smoothstepResult303_g39811 = smoothstep( _Vector3.x , _Vector3.y , ( WN_X280_g39811 + 1.25 ));
				float smoothstepResult299_g39811 = smoothstep( _Vector3.x , _Vector3.y , ( -WN_X280_g39811 + 0.02 ));
				float lerpResult306_g39811 = lerp( 0.0 , smoothstepResult303_g39811 , smoothstepResult299_g39811);
				float4 lerpResult301_g39811 = lerp( VECTOR_IN_MASKED343_g39811 , Vect_001349_g39811 , lerpResult306_g39811);
				float4 MASK_VERTICAL_X307_g39811 = lerpResult301_g39811;
				float2 _Vector1 = float2(0,0.85);
				float WN_Y281_g39811 = break277_g39811.y;
				float smoothstepResult334_g39811 = smoothstep( _Vector1.x , _Vector1.y , ( -WN_Y281_g39811 + 0.35 ));
				float4 lerpResult333_g39811 = lerp( VECTOR_IN_MASKED343_g39811 , Vect_001349_g39811 , smoothstepResult334_g39811);
				float4 MASK_VERTICAL_Y320_g39811 = lerpResult333_g39811;
				float2 _HelloRicardo = float2(1.8,1.4);
				float WN_Z282_g39811 = break277_g39811.z;
				float smoothstepResult331_g39811 = smoothstep( _HelloRicardo.x , _HelloRicardo.y , ( WN_Z282_g39811 + 2.06 ));
				float lerpResult327_g39811 = lerp( smoothstepResult331_g39811 , 0.0 , ( -WN_Z282_g39811 + 2.05 ));
				float4 lerpResult314_g39811 = lerp( Vect_001349_g39811 , VECTOR_IN_MASKED343_g39811 , lerpResult327_g39811);
				float4 MASK_VERTICAL_Z315_g39811 = lerpResult314_g39811;
				float4 Curvature353_g39811 = saturate( ( MASK_VERTICAL_X307_g39811 + MASK_VERTICAL_Y320_g39811 + MASK_VERTICAL_Z315_g39811 ) );
				float4 lerpResult117_g39811 = lerp( _Gradient_BottomSide , _Gradient_Bottom , WN_HORIZONTAL284_g39811);
				float4 lerpResult111_g39811 = lerp( _Gradient_TopSide , _Gradient_Top , WN_HORIZONTAL284_g39811);
				float clampResult106_g39811 = clamp( ( IN.ase_texcoord3.xyz.y / _Gradient_Height ) , 0.0 , 1.0 );
				float4 lerpResult113_g39811 = lerp( lerpResult117_g39811 , lerpResult111_g39811 , clampResult106_g39811);
				float3 temp_output_145_0_g39811 = (( Curvature353_g39811 + lerpResult113_g39811 )).xyz;
				float _Gradient_Ramp154_g39811 = _Gradient_Ramp;
				float _Gradient_Contrast157_g39811 = _Gradient_Contrast;
				float2 appendResult168_g37947 = (float2(_TilingX , _TilingY));
				float2 appendResult167_g37947 = (float2(_OffsetX , _OffsetY));
				float2 texCoord2_g39802 = IN.ase_texcoord4.xy * appendResult168_g37947 + appendResult167_g37947;
				float2 appendResult6_g39802 = (float2(texCoord2_g39802.x , texCoord2_g39802.y));
				float2 OUT_UV213_g37947 = appendResult6_g39802;
				float2 UV40_g39808 = OUT_UV213_g37947;
				float4 tex2DNode63_g39808 = SAMPLE_TEXTURE2D( _MainTex, sampler_trilinear_repeat, UV40_g39808 );
				float4 ALBEDO_RGBA1381_g37947 = tex2DNode63_g39808;
				float4 ALBEDO151_g39811 = ALBEDO_RGBA1381_g37947;
				float smoothstepResult142_g39811 = smoothstep( ( _Gradient_Ramp154_g39811 - ( _Gradient_Ramp154_g39811 * ( 1.0 - _Gradient_Contrast157_g39811 ) ) ) , ALBEDO151_g39811.x , _Gradient_Ramp154_g39811);
				float4 lerpResult139_g39811 = lerp( TINT147_g39811 , float4( temp_output_145_0_g39811 , 0.0 ) , smoothstepResult142_g39811);
				float4 m_Active143_g39811 = lerpResult139_g39811;
				float4 lerpResult141_g39811 = lerp( float4( temp_output_145_0_g39811 , 0.0 ) , TINT147_g39811 , smoothstepResult142_g39811);
				float4 m_ActiveInverted143_g39811 = lerpResult141_g39811;
				float4 localfloat4switch143_g39811 = float4switch143_g39811( m_switch143_g39811 , m_Off143_g39811 , m_Active143_g39811 , m_ActiveInverted143_g39811 );
				float4 temp_output_3_0_g37947 = ( localfloat4switch143_g39811 * float4( (ALBEDO_RGBA1381_g37947).rgb , 0.0 ) * _Brightness );
				float4 temp_output_6_0_g39827 = ( temp_output_3_0_g37947 + float4(0,0,0,0) );
				float4 m_Off1269_g39827 = temp_output_6_0_g39827;
				float2 appendResult1344_g39827 = (float2(_Horizontalwear_TilingX , _Horizontalwear_TilingY));
				float2 appendResult1343_g39827 = (float2(_Horizontalwear_OffsetX , _Horizontalwear_OffsetY));
				float2 texCoord1342_g39827 = IN.ase_texcoord4.xy * appendResult1344_g39827 + appendResult1343_g39827;
				float4 tex2DNode1327_g39827 = SAMPLE_TEXTURE2D( _Horizontalwear_GrungeMap, sampler_trilinear_repeat, texCoord1342_g39827 );
				float4 temp_output_12_0_g39830 = ( float4( (_Horizontalwear_Tint).rgb , 0.0 ) * tex2DNode1327_g39827 );
				float3 desaturateInitialColor10_g39830 = temp_output_12_0_g39830.xyz;
				float desaturateDot10_g39830 = dot( desaturateInitialColor10_g39830, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39830 = lerp( desaturateInitialColor10_g39830, desaturateDot10_g39830.xxx, ( 1.0 - ( _Horizontalwear_Saturation + 1.0 ) ) );
				float3 FINAL_MASK_HORIZONTAL804_g39827 = desaturateVar10_g39830;
				float4 temp_cast_15 = (0.5).xxxx;
				float4 temp_cast_16 = (0.75).xxxx;
				int _Horizontalwear_Mode1003_g39827 = _Horizontalwear_Mode;
				int m_switch1476_g39827 = _Horizontalwear_Mode1003_g39827;
				float4 break91_g39853 = _Horizontalwear_MaskingColor;
				float3 appendResult87_g39853 = (float3(break91_g39853.r , break91_g39853.g , break91_g39853.b));
				int m_switch1506_g39827 = _Horizontalwear_MaskingSource;
				float4 _Horizontalwear_GrungeMap1508_g39827 = tex2DNode1327_g39827;
				float4 m_MapGrunge1506_g39827 = _Horizontalwear_GrungeMap1508_g39827;
				float4 ALBEDO_IN1447_g39827 = temp_output_6_0_g39827;
				float4 m_MapAlbedo1506_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1506_g39827 = float4switch1506_g39827( m_switch1506_g39827 , m_MapGrunge1506_g39827 , m_MapAlbedo1506_g39827 );
				float4 break89_g39853 = localfloat4switch1506_g39827;
				float3 appendResult88_g39853 = (float3(break89_g39853.x , break89_g39853.y , break89_g39853.z));
				float4 temp_cast_18 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1476_g39827 = temp_cast_18;
				float4 temp_cast_19 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1476_g39827 = temp_cast_19;
				float4 temp_cast_20 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1476_g39827 = temp_cast_20;
				float4 localfloat4switch1476_g39827 = float4switch1476_g39827( m_switch1476_g39827 , m_Off1476_g39827 , m_Active1476_g39827 , m_ActiveInverted1476_g39827 );
				float4 temp_output_44_0_g39857 = localfloat4switch1476_g39827;
				float4 VECTOR_IN212_g39857 = temp_output_44_0_g39857;
				float4 _Vec = float4(0.001,0.001,0.001,0.001);
				float4 Vect_0019_g39857 = _Vec;
				float2 _SmoothHorizontal = float2(0,0.5);
				float3 temp_output_40_0_g39857 = abs( ase_worldNormal );
				float WN_HORIZONTAL141_g39857 = ( ( temp_output_40_0_g39857 * temp_output_40_0_g39857 ).y + 0.07 );
				float smoothstepResult205_g39857 = smoothstep( _SmoothHorizontal.x , _SmoothHorizontal.y , ( ( 1.0 - WN_HORIZONTAL141_g39857 ) + -0.1 ));
				float4 lerpResult200_g39857 = lerp( VECTOR_IN212_g39857 , Vect_0019_g39857 , smoothstepResult205_g39857);
				float4 MASK_HORIZONTAL204_g39857 = lerpResult200_g39857;
				float4 temp_cast_21 = (_Horizontalwear_BlendStrength).xxxx;
				float4 temp_cast_22 = (-1.0).xxxx;
				float4 smoothstepResult75_g39857 = smoothstep( temp_cast_15 , temp_cast_16 , (float4( 0,0,0,0 ) + (( 1.0 - MASK_HORIZONTAL204_g39857 ) - temp_cast_21) * (temp_cast_22 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_21)));
				float4 _MinOldVect4 = float4(0,0,0,0);
				float MASK_HORIZONTAL808_g39827 = ( (( ( smoothstepResult75_g39857 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x * _Horizontalwear_BlendMix );
				float4 lerpResult861_g39827 = lerp( temp_output_6_0_g39827 , float4( FINAL_MASK_HORIZONTAL804_g39827 , 0.0 ) , MASK_HORIZONTAL808_g39827);
				float4 m_Active1269_g39827 = lerpResult861_g39827;
				float4 m_ActiveInverted1269_g39827 = lerpResult861_g39827;
				float4 localfloat4switch1269_g39827 = float4switch1269_g39827( m_switch1269_g39827 , m_Off1269_g39827 , m_Active1269_g39827 , m_ActiveInverted1269_g39827 );
				float4 m_Off1377_g39827 = localfloat4switch1269_g39827;
				float2 appendResult1384_g39827 = (float2(_Verticalwear_TilingX , _Verticalwear_TilingY));
				float2 appendResult1386_g39827 = (float2(_Verticalwear_OffsetX , _Verticalwear_OffsetY));
				float2 texCoord1388_g39827 = IN.ase_texcoord4.xy * appendResult1384_g39827 + appendResult1386_g39827;
				float4 tex2DNode1397_g39827 = SAMPLE_TEXTURE2D( _Verticalwear_GrungeMap, sampler_trilinear_repeat, texCoord1388_g39827 );
				float4 temp_output_12_0_g39842 = ( float4( (_Verticalwear_Tint).rgb , 0.0 ) * tex2DNode1397_g39827 );
				float3 desaturateInitialColor10_g39842 = temp_output_12_0_g39842.xyz;
				float desaturateDot10_g39842 = dot( desaturateInitialColor10_g39842, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39842 = lerp( desaturateInitialColor10_g39842, desaturateDot10_g39842.xxx, ( 1.0 - ( _Verticalwear_Saturation + 1.0 ) ) );
				float3 FINAL_MASK_VERTICAL394_g39827 = desaturateVar10_g39842;
				float4 temp_cast_27 = (0.5).xxxx;
				float4 temp_cast_28 = (0.75).xxxx;
				int _Verticalwear_Mode1004_g39827 = _Verticalwear_Mode;
				int m_switch1400_g39827 = _Verticalwear_Mode1004_g39827;
				float4 break91_g39848 = _Verticalwear_MaskingColor;
				float3 appendResult87_g39848 = (float3(break91_g39848.r , break91_g39848.g , break91_g39848.b));
				int m_switch1512_g39827 = _Verticalwear_MaskingSource;
				float4 _Verticalwear_GrungeMap1509_g39827 = tex2DNode1397_g39827;
				float4 m_MapGrunge1512_g39827 = _Verticalwear_GrungeMap1509_g39827;
				float4 m_MapAlbedo1512_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1512_g39827 = float4switch1512_g39827( m_switch1512_g39827 , m_MapGrunge1512_g39827 , m_MapAlbedo1512_g39827 );
				float4 break89_g39848 = localfloat4switch1512_g39827;
				float3 appendResult88_g39848 = (float3(break89_g39848.x , break89_g39848.y , break89_g39848.z));
				float4 temp_cast_30 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1400_g39827 = temp_cast_30;
				float4 temp_cast_31 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1400_g39827 = temp_cast_31;
				float4 temp_cast_32 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1400_g39827 = temp_cast_32;
				float4 localfloat4switch1400_g39827 = float4switch1400_g39827( m_switch1400_g39827 , m_Off1400_g39827 , m_Active1400_g39827 , m_ActiveInverted1400_g39827 );
				float4 temp_output_44_0_g39828 = localfloat4switch1400_g39827;
				float3 temp_output_40_0_g39828 = abs( ase_worldNormal );
				float WN_HORIZONTAL141_g39828 = ( ( temp_output_40_0_g39828 * temp_output_40_0_g39828 ).y + 0.07 );
				float4 lerpResult2_g39828 = lerp( temp_output_44_0_g39828 , _Vec , WN_HORIZONTAL141_g39828);
				float4 VECTOR_IN_MASKED112_g39828 = lerpResult2_g39828;
				float4 Vect_0019_g39828 = _Vec;
				float3 break349_g39828 = ase_worldNormal;
				float WN_NY173_g39828 = break349_g39828.y;
				float WN_NZ184_g39828 = break349_g39828.z;
				float WN_NX186_g39828 = break349_g39828.x;
				float temp_output_175_0_g39828 = ( -WN_NY173_g39828 + WN_NZ184_g39828 + -WN_NX186_g39828 );
				float4 lerpResult133_g39828 = lerp( VECTOR_IN_MASKED112_g39828 , Vect_0019_g39828 , ( temp_output_175_0_g39828 + -temp_output_175_0_g39828 + 0.07 ));
				float4 MASK_SIDE163_g39828 = lerpResult133_g39828;
				float4 temp_cast_33 = (_Verticalwear_BlendStrength).xxxx;
				float4 temp_cast_34 = (-2.0).xxxx;
				float4 smoothstepResult75_g39828 = smoothstep( temp_cast_27 , temp_cast_28 , (float4( 0,0,0,0 ) + (( 1.0 - MASK_SIDE163_g39828 ) - temp_cast_33) * (temp_cast_34 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_33)));
				float MASK_VERTICAL602_g39827 = ( (( ( smoothstepResult75_g39828 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x * _Verticalwear_BlendMix );
				float4 lerpResult1227_g39827 = lerp( localfloat4switch1269_g39827 , float4( FINAL_MASK_VERTICAL394_g39827 , 0.0 ) , MASK_VERTICAL602_g39827);
				float4 m_Active1377_g39827 = lerpResult1227_g39827;
				float4 m_ActiveInverted1377_g39827 = lerpResult1227_g39827;
				float4 localfloat4switch1377_g39827 = float4switch1377_g39827( m_switch1377_g39827 , m_Off1377_g39827 , m_Active1377_g39827 , m_ActiveInverted1377_g39827 );
				float4 m_Off1380_g39827 = localfloat4switch1377_g39827;
				float2 appendResult1393_g39827 = (float2(_Curvaturewear_TilingX , _Curvaturewear_TilingY));
				float2 appendResult1390_g39827 = (float2(_Curvaturewear_OffsetX , _Curvaturewear_OffsetY));
				float2 texCoord1389_g39827 = IN.ase_texcoord4.xy * appendResult1393_g39827 + appendResult1390_g39827;
				float4 tex2DNode1396_g39827 = SAMPLE_TEXTURE2D( _Curvaturewear_GrungeMap, sampler_trilinear_repeat, texCoord1389_g39827 );
				float4 temp_output_12_0_g39834 = ( float4( (_Curvaturewear_Tint).rgb , 0.0 ) * tex2DNode1396_g39827 );
				float3 desaturateInitialColor10_g39834 = temp_output_12_0_g39834.xyz;
				float desaturateDot10_g39834 = dot( desaturateInitialColor10_g39834, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39834 = lerp( desaturateInitialColor10_g39834, desaturateDot10_g39834.xxx, ( 1.0 - ( _Curvaturewear_Saturation + 1.0 ) ) );
				float4 lerpResult1544_g39827 = lerp( ALBEDO_IN1447_g39827 , float4( desaturateVar10_g39834 , 0.0 ) , _Curvaturewear_BlendMix);
				float4 FINAL_MASK_CURVATURE586_g39827 = lerpResult1544_g39827;
				float4 temp_cast_39 = (0.5).xxxx;
				float4 temp_cast_40 = (0.75).xxxx;
				int _Curvaturewear_Mode398_g39827 = _Curvaturewear_Mode;
				int m_switch1491_g39827 = _Curvaturewear_Mode398_g39827;
				float4 break91_g39855 = _Curvaturewear_MaskingColor;
				float3 appendResult87_g39855 = (float3(break91_g39855.r , break91_g39855.g , break91_g39855.b));
				int m_switch1516_g39827 = _Curvaturewear_MaskingSource;
				float4 _Curvaturewear_GrungeMap1510_g39827 = tex2DNode1396_g39827;
				float4 m_MapGrunge1516_g39827 = _Curvaturewear_GrungeMap1510_g39827;
				float4 m_MapAlbedo1516_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1516_g39827 = float4switch1516_g39827( m_switch1516_g39827 , m_MapGrunge1516_g39827 , m_MapAlbedo1516_g39827 );
				float4 break89_g39855 = localfloat4switch1516_g39827;
				float3 appendResult88_g39855 = (float3(break89_g39855.x , break89_g39855.y , break89_g39855.z));
				float4 temp_cast_42 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1491_g39827 = temp_cast_42;
				float4 temp_cast_43 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1491_g39827 = temp_cast_43;
				float4 temp_cast_44 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1491_g39827 = temp_cast_44;
				float4 localfloat4switch1491_g39827 = float4switch1491_g39827( m_switch1491_g39827 , m_Off1491_g39827 , m_Active1491_g39827 , m_ActiveInverted1491_g39827 );
				float4 temp_output_44_0_g39836 = localfloat4switch1491_g39827;
				float3 temp_output_40_0_g39836 = abs( ase_worldNormal );
				float WN_HORIZONTAL141_g39836 = ( ( temp_output_40_0_g39836 * temp_output_40_0_g39836 ).y + 0.07 );
				float4 lerpResult2_g39836 = lerp( temp_output_44_0_g39836 , _Vec , WN_HORIZONTAL141_g39836);
				float4 VECTOR_IN_MASKED112_g39836 = lerpResult2_g39836;
				float4 Vect_0019_g39836 = _Vec;
				float2 _SmoothX = float2(0,0.45);
				float3 temp_cast_45 = (0.5).xxx;
				float3 break24_g39836 = ( temp_output_40_0_g39836 - temp_cast_45 );
				float WN_X134_g39836 = break24_g39836.x;
				float smoothstepResult8_g39836 = smoothstep( _SmoothX.x , _SmoothX.y , ( WN_X134_g39836 + 1.25 ));
				float smoothstepResult22_g39836 = smoothstep( _SmoothX.x , _SmoothX.y , ( -WN_X134_g39836 + 0.02 ));
				float lerpResult11_g39836 = lerp( 0.0 , smoothstepResult8_g39836 , smoothstepResult22_g39836);
				float4 lerpResult14_g39836 = lerp( VECTOR_IN_MASKED112_g39836 , Vect_0019_g39836 , lerpResult11_g39836);
				float4 MASK_VERTICAL_X159_g39836 = lerpResult14_g39836;
				float2 _SmoothY = float2(0,0.85);
				float WN_Y135_g39836 = break24_g39836.y;
				float smoothstepResult29_g39836 = smoothstep( _SmoothY.x , _SmoothY.y , ( -WN_Y135_g39836 + 0.35 ));
				float4 lerpResult6_g39836 = lerp( VECTOR_IN_MASKED112_g39836 , Vect_0019_g39836 , smoothstepResult29_g39836);
				float4 MASK_VERTICAL_Y157_g39836 = lerpResult6_g39836;
				float2 _SmoothZ = float2(1.8,1.4);
				float WN_Z136_g39836 = break24_g39836.z;
				float smoothstepResult26_g39836 = smoothstep( _SmoothZ.x , _SmoothZ.y , ( WN_Z136_g39836 + 2.06 ));
				float lerpResult34_g39836 = lerp( smoothstepResult26_g39836 , 0.0 , ( -WN_Z136_g39836 + 2.05 ));
				float4 lerpResult5_g39836 = lerp( Vect_0019_g39836 , VECTOR_IN_MASKED112_g39836 , lerpResult34_g39836);
				float4 MASK_VERTICAL_Z155_g39836 = lerpResult5_g39836;
				float4 temp_cast_46 = (_Curvaturewear_BlendStrength).xxxx;
				float4 temp_cast_47 = (_Curvaturewear_BlendHardness).xxxx;
				float4 smoothstepResult75_g39836 = smoothstep( temp_cast_39 , temp_cast_40 , (float4( 0,0,0,0 ) + (( 1.0 - ( MASK_VERTICAL_X159_g39836 + MASK_VERTICAL_Y157_g39836 + MASK_VERTICAL_Z155_g39836 ) ) - temp_cast_46) * (temp_cast_47 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_46)));
				float MASK_CURVATURE401_g39827 = (( ( smoothstepResult75_g39836 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x;
				float4 lerpResult860_g39827 = lerp( FINAL_MASK_CURVATURE586_g39827 , localfloat4switch1377_g39827 , MASK_CURVATURE401_g39827);
				float4 m_Active1380_g39827 = lerpResult860_g39827;
				float4 m_ActiveInverted1380_g39827 = lerpResult860_g39827;
				float4 localfloat4switch1380_g39827 = float4switch1380_g39827( m_switch1380_g39827 , m_Off1380_g39827 , m_Active1380_g39827 , m_ActiveInverted1380_g39827 );
				
				float ALBEDO_A926_g37947 = tex2DNode63_g39808.a;
				int m_switch1287_g37947 = _GlancingClipMode;
				float m_Off1287_g37947 = 1.0;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float3 normalizeResult1294_g37947 = normalize( cross( ddx( WorldPosition ) , ddy( WorldPosition ) ) );
				float dotResult1301_g37947 = dot( ase_worldViewDir , normalizeResult1294_g37947 );
				float temp_output_1321_0_g37947 = ( 1.0 - abs( dotResult1301_g37947 ) );
				float m_Active1287_g37947 = ( 1.0 - ( temp_output_1321_0_g37947 * temp_output_1321_0_g37947 ) );
				float localfloatswitch1287_g37947 = floatswitch1287_g37947( m_switch1287_g37947 , m_Off1287_g37947 , m_Active1287_g37947 );
				float OPACITY_OUTMASK1277_g37947 = localfloatswitch1287_g37947;
				
				float AlphaCutoffBias1289_g37947 = _AlphaCutoffBias;
				
				
				float3 Albedo = localfloat4switch1380_g39827.xyz;
				float3 Emission = 0;
				float Alpha = ( ALBEDO_A926_g37947 * OPACITY_OUTMASK1277_g37947 );
				float AlphaClipThreshold = AlphaCutoffBias1289_g37947;

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				MetaInput metaInput = (MetaInput)0;
				metaInput.Albedo = Albedo;
				metaInput.Emission = Emission;
				
				return MetaFragment(metaInput);
			}
			ENDHLSL
		}

		
		Pass
		{
			
			Name "Universal2D"
			Tags { "LightMode"="Universal2D" }

			Blend One Zero, One Zero
			ZWrite [_ZWriteMode]
			ZTest LEqual
			Offset 0 , 0
			ColorMask [_ColorMask]

			HLSLPROGRAM
			
			#define _NORMAL_DROPOFF_TS 1
			#define _ALPHATEST_SHADOW_ON 1
			#pragma multi_compile_instancing
			#pragma multi_compile _ LOD_FADE_CROSSFADE
			#pragma multi_compile_fog
			#define ASE_FOG 1
			#define _ALPHATEST_ON 1
			#define _NORMALMAP 1
			#define ASE_SRP_VERSION 70503
			#define ASE_USING_SAMPLING_MACROS 1

			
			#pragma vertex vert
			#pragma fragment frag

			#define SHADERPASS_2D

			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/UnityInstancing.hlsl"
			#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
			
			#define ASE_NEEDS_VERT_NORMAL
			#define ASE_NEEDS_FRAG_WORLD_POSITION
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile_instancing
			#pragma instancing_options procedural:SetupNatureRenderer forwardadd
			#pragma multi_compile GPU_FRUSTUM_ON __
			#include "Nature Renderer.cginc"
			#pragma multi_compile_local _ NATURE_RENDERER


			#pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A

			struct VertexInput
			{
				float4 vertex : POSITION;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos : SV_POSITION;
				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 worldPos : TEXCOORD0;
				#endif
				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
				float4 shadowCoord : TEXCOORD1;
				#endif
				float4 ase_texcoord2 : TEXCOORD2;
				float4 ase_texcoord3 : TEXCOORD3;
				float4 ase_texcoord4 : TEXCOORD4;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			CBUFFER_START(UnityPerMaterial)
			float4 _Verticalwear_MaskingColor;
			float4 _Horizontalwear_Tint;
			float4 _Curvaturewear_Tint;
			float4 _Horizontalwear_MaskingColor;
			float4 _Verticalwear_Tint;
			float4 _Gradient_Top;
			float4 _Gradient_Bottom;
			float4 _Gradient_BottomSide;
			float4 _Gradient_TintCurvature;
			float4 _Gradient_TopSide;
			float4 _Curvaturewear_MaskingColor;
			float4 _Color;
			float _Curvaturewear_BlendMix;
			half _NormalStrength;
			half _Curvaturewear_Saturation;
			float _Curvaturewear_OffsetY;
			int _Curvaturewear_MaskingSource;
			half _Curvaturewear_MaskingColorRange;
			half _Curvaturewear_MaskingColorFuzziness;
			float _Curvaturewear_BlendStrength;
			float _Curvaturewear_BlendHardness;
			float _Curvaturewear_OffsetX;
			float _Curvaturewear_TilingY;
			float _Curvaturewear_TilingX;
			int _NormalMode;
			float _MetallicStrength;
			float _Verticalwear_NormalMultiplier;
			float _Curvaturewear_Smoothness;
			float _Verticalwear_Smoothness;
			float _Horizontalwear_Smoothness;
			float _OcclusionStrengthAO;
			int _OcclusionSourceMode;
			int _SmoothnessType;
			int _GlancingClipMode;
			float _SmoothnessThreshold;
			float _SmoothnessVariance;
			float _SmoothnessStrength;
			int _SmoothnessModeCutout;
			float _Curvaturewear_Metallic;
			float _Verticalwear_Metallic;
			float _Horizontalwear_Metallic;
			float _Curvaturewear_NormalMultiplier;
			float _Horizontalwear_NormalMultiplier;
			float _Verticalwear_BlendMix;
			int _ColorMask;
			half _Verticalwear_MaskingColorFuzziness;
			float _OffsetY;
			float _OffsetX;
			float _TilingY;
			float _TilingX;
			float _Gradient_Contrast;
			float _Gradient_Ramp;
			half _Brightness;
			float _Gradient_Height;
			int _Horizontalwear_Mode;
			int _Verticalwear_Mode;
			int _Curvaturewear_Mode;
			int _ZWriteMode;
			float _AlphatoCoverage;
			int _CullMode;
			int _Gradient_Mode;
			float _Horizontalwear_TilingX;
			float _Horizontalwear_TilingY;
			float _Horizontalwear_OffsetX;
			half _Verticalwear_MaskingColorRange;
			int _Verticalwear_MaskingSource;
			half _AlphaCutoffBias;
			half _Verticalwear_Saturation;
			float _Verticalwear_OffsetY;
			float _Verticalwear_OffsetX;
			float _Verticalwear_TilingY;
			float _Verticalwear_TilingX;
			float _Horizontalwear_BlendMix;
			float _Horizontalwear_BlendStrength;
			half _Horizontalwear_MaskingColorFuzziness;
			half _Horizontalwear_MaskingColorRange;
			int _Horizontalwear_MaskingSource;
			half _Horizontalwear_Saturation;
			float _Horizontalwear_OffsetY;
			float _Verticalwear_BlendStrength;
			half _AlphaCutoffBiasShadow;
			#ifdef _TRANSMISSION_ASE
				float _TransmissionShadow;
			#endif
			#ifdef _TRANSLUCENCY_ASE
				float _TransStrength;
				float _TransNormal;
				float _TransScattering;
				float _TransDirect;
				float _TransAmbient;
				float _TransShadow;
			#endif
			#ifdef TESSELLATION_ON
				float _TessPhongStrength;
				float _TessValue;
				float _TessMin;
				float _TessMax;
				float _TessEdgeLength;
				float _TessMaxDisp;
			#endif
			CBUFFER_END
			TEXTURE2D(_MainTex);
			SAMPLER(sampler_trilinear_repeat);
			TEXTURE2D(_Horizontalwear_GrungeMap);
			TEXTURE2D(_Verticalwear_GrungeMap);
			TEXTURE2D(_Curvaturewear_GrungeMap);


			float4 mod289( float4 x )
			{
				return x - floor(x * (1.0 / 289.0)) * 289.0;
			}
			
			float4 perm( float4 x )
			{
				return mod289(((x * 34.0) + 1.0) * x);
			}
			
			float SimpleNoise3D( float3 p )
			{
				 float3 a = floor(p);
				    float3 d = p - a;
				    d = d * d * (3.0 - 2.0 * d);
				 float4 b = a.xxyy + float4(0.0, 1.0, 0.0, 1.0);
				    float4 k1 = perm(b.xyxy);
				 float4 k2 = perm(k1.xyxy + b.zzww);
				    float4 c = k2 + a.zzzz;
				    float4 k3 = perm(c);
				    float4 k4 = perm(c + 1.0);
				    float4 o1 = frac(k3 * (1.0 / 41.0));
				 float4 o2 = frac(k4 * (1.0 / 41.0));
				    float4 o3 = o2 * d.z + o1 * (1.0 - d.z);
				    float2 o4 = o3.yw * d.x + o3.xz * (1.0 - d.x);
				    return o4.y * d.y + o4.x * (1.0 - d.y);
			}
			
			float4 float4switch143_g39811( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1506_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1476_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1269_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1512_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1400_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1377_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1516_g39827( int m_switch, float4 m_MapGrunge, float4 m_MapAlbedo )
			{
				if(m_switch ==0)
					return m_MapGrunge;
				else if(m_switch ==1)
					return m_MapAlbedo;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1491_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float4 float4switch1380_g39827( int m_switch, float4 m_Off, float4 m_Active, float4 m_ActiveInverted )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else if(m_switch ==2)
					return m_ActiveInverted;
				else
				return float4(0,0,0,0);
			}
			
			float floatswitch1287_g37947( int m_switch, float m_Off, float m_Active )
			{
				if(m_switch ==0)
					return m_Off;
				else if(m_switch ==1)
					return m_Active;
				else
				return float(0);
			}
			

			VertexOutput VertexFunction( VertexInput v  )
			{
				VertexOutput o = (VertexOutput)0;
				UNITY_SETUP_INSTANCE_ID( v );
				UNITY_TRANSFER_INSTANCE_ID( v, o );
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO( o );

				float3 ase_worldNormal = TransformObjectToWorldNormal(v.ase_normal);
				o.ase_texcoord2.xyz = ase_worldNormal;
				
				o.ase_texcoord3 = v.vertex;
				o.ase_texcoord4.xy = v.ase_texcoord.xy;
				
				//setting value to unused interpolator channels and avoid initialization warnings
				o.ase_texcoord2.w = 0;
				o.ase_texcoord4.zw = 0;
				
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					float3 defaultVertexValue = v.vertex.xyz;
				#else
					float3 defaultVertexValue = float3(0, 0, 0);
				#endif
				float3 vertexValue = defaultVertexValue;
				#ifdef ASE_ABSOLUTE_VERTEX_POS
					v.vertex.xyz = vertexValue;
				#else
					v.vertex.xyz += vertexValue;
				#endif

				v.ase_normal = v.ase_normal;

				float3 positionWS = TransformObjectToWorld( v.vertex.xyz );
				float4 positionCS = TransformWorldToHClip( positionWS );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				o.worldPos = positionWS;
				#endif

				#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR) && defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					VertexPositionInputs vertexInput = (VertexPositionInputs)0;
					vertexInput.positionWS = positionWS;
					vertexInput.positionCS = positionCS;
					o.shadowCoord = GetShadowCoord( vertexInput );
				#endif

				o.clipPos = positionCS;
				return o;
			}

			#if defined(TESSELLATION_ON)
			struct VertexControl
			{
				float4 vertex : INTERNALTESSPOS;
				float3 ase_normal : NORMAL;
				float4 ase_texcoord : TEXCOORD0;

				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct TessellationFactors
			{
				float edge[3] : SV_TessFactor;
				float inside : SV_InsideTessFactor;
			};

			VertexControl vert ( VertexInput v )
			{
				VertexControl o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				o.vertex = v.vertex;
				o.ase_normal = v.ase_normal;
				o.ase_texcoord = v.ase_texcoord;
				return o;
			}

			TessellationFactors TessellationFunction (InputPatch<VertexControl,3> v)
			{
				TessellationFactors o;
				float4 tf = 1;
				float tessValue = _TessValue; float tessMin = _TessMin; float tessMax = _TessMax;
				float edgeLength = _TessEdgeLength; float tessMaxDisp = _TessMaxDisp;
				#if defined(ASE_FIXED_TESSELLATION)
				tf = FixedTess( tessValue );
				#elif defined(ASE_DISTANCE_TESSELLATION)
				tf = DistanceBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, tessValue, tessMin, tessMax, GetObjectToWorldMatrix(), _WorldSpaceCameraPos );
				#elif defined(ASE_LENGTH_TESSELLATION)
				tf = EdgeLengthBasedTess(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams );
				#elif defined(ASE_LENGTH_CULL_TESSELLATION)
				tf = EdgeLengthBasedTessCull(v[0].vertex, v[1].vertex, v[2].vertex, edgeLength, tessMaxDisp, GetObjectToWorldMatrix(), _WorldSpaceCameraPos, _ScreenParams, unity_CameraWorldClipPlanes );
				#endif
				o.edge[0] = tf.x; o.edge[1] = tf.y; o.edge[2] = tf.z; o.inside = tf.w;
				return o;
			}

			[domain("tri")]
			[partitioning("fractional_odd")]
			[outputtopology("triangle_cw")]
			[patchconstantfunc("TessellationFunction")]
			[outputcontrolpoints(3)]
			VertexControl HullFunction(InputPatch<VertexControl, 3> patch, uint id : SV_OutputControlPointID)
			{
			   return patch[id];
			}

			[domain("tri")]
			VertexOutput DomainFunction(TessellationFactors factors, OutputPatch<VertexControl, 3> patch, float3 bary : SV_DomainLocation)
			{
				VertexInput o = (VertexInput) 0;
				o.vertex = patch[0].vertex * bary.x + patch[1].vertex * bary.y + patch[2].vertex * bary.z;
				o.ase_normal = patch[0].ase_normal * bary.x + patch[1].ase_normal * bary.y + patch[2].ase_normal * bary.z;
				o.ase_texcoord = patch[0].ase_texcoord * bary.x + patch[1].ase_texcoord * bary.y + patch[2].ase_texcoord * bary.z;
				#if defined(ASE_PHONG_TESSELLATION)
				float3 pp[3];
				for (int i = 0; i < 3; ++i)
					pp[i] = o.vertex.xyz - patch[i].ase_normal * (dot(o.vertex.xyz, patch[i].ase_normal) - dot(patch[i].vertex.xyz, patch[i].ase_normal));
				float phongStrength = _TessPhongStrength;
				o.vertex.xyz = phongStrength * (pp[0]*bary.x + pp[1]*bary.y + pp[2]*bary.z) + (1.0f-phongStrength) * o.vertex.xyz;
				#endif
				UNITY_TRANSFER_INSTANCE_ID(patch[0], o);
				return VertexFunction(o);
			}
			#else
			VertexOutput vert ( VertexInput v )
			{
				return VertexFunction( v );
			}
			#endif

			half4 frag(VertexOutput IN  ) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID( IN );
				UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX( IN );

				#if defined(ASE_NEEDS_FRAG_WORLD_POSITION)
				float3 WorldPosition = IN.worldPos;
				#endif
				float4 ShadowCoords = float4( 0, 0, 0, 0 );

				#if defined(ASE_NEEDS_FRAG_SHADOWCOORDS)
					#if defined(REQUIRES_VERTEX_SHADOW_COORD_INTERPOLATOR)
						ShadowCoords = IN.shadowCoord;
					#elif defined(MAIN_LIGHT_CALCULATE_SHADOWS)
						ShadowCoords = TransformWorldToShadowCoord( WorldPosition );
					#endif
				#endif

				int m_switch1380_g39827 = _Curvaturewear_Mode;
				int m_switch1377_g39827 = _Verticalwear_Mode;
				int m_switch1269_g39827 = _Horizontalwear_Mode;
				int _Gradient_Mode161_g39811 = _Gradient_Mode;
				int m_switch143_g39811 = _Gradient_Mode161_g39811;
				float4 TINT147_g39811 = _Color;
				float4 m_Off143_g39811 = TINT147_g39811;
				float4 _Vector0 = float4(0.001,0.001,0.001,0.001);
				float3 ase_worldNormal = IN.ase_texcoord2.xyz;
				float3 temp_output_275_0_g39811 = abs( ase_worldNormal );
				float WN_HORIZONTAL284_g39811 = ( ( temp_output_275_0_g39811 * temp_output_275_0_g39811 ).y + 0.07 );
				float4 lerpResult341_g39811 = lerp( _Gradient_TintCurvature , _Vector0 , WN_HORIZONTAL284_g39811);
				float4 VECTOR_IN_MASKED343_g39811 = lerpResult341_g39811;
				float4 Vect_001349_g39811 = _Vector0;
				float2 _Vector3 = float2(0,0.45);
				float3 temp_cast_2 = (0.5).xxx;
				float3 break277_g39811 = ( temp_output_275_0_g39811 - temp_cast_2 );
				float WN_X280_g39811 = break277_g39811.x;
				float smoothstepResult303_g39811 = smoothstep( _Vector3.x , _Vector3.y , ( WN_X280_g39811 + 1.25 ));
				float smoothstepResult299_g39811 = smoothstep( _Vector3.x , _Vector3.y , ( -WN_X280_g39811 + 0.02 ));
				float lerpResult306_g39811 = lerp( 0.0 , smoothstepResult303_g39811 , smoothstepResult299_g39811);
				float4 lerpResult301_g39811 = lerp( VECTOR_IN_MASKED343_g39811 , Vect_001349_g39811 , lerpResult306_g39811);
				float4 MASK_VERTICAL_X307_g39811 = lerpResult301_g39811;
				float2 _Vector1 = float2(0,0.85);
				float WN_Y281_g39811 = break277_g39811.y;
				float smoothstepResult334_g39811 = smoothstep( _Vector1.x , _Vector1.y , ( -WN_Y281_g39811 + 0.35 ));
				float4 lerpResult333_g39811 = lerp( VECTOR_IN_MASKED343_g39811 , Vect_001349_g39811 , smoothstepResult334_g39811);
				float4 MASK_VERTICAL_Y320_g39811 = lerpResult333_g39811;
				float2 _HelloRicardo = float2(1.8,1.4);
				float WN_Z282_g39811 = break277_g39811.z;
				float smoothstepResult331_g39811 = smoothstep( _HelloRicardo.x , _HelloRicardo.y , ( WN_Z282_g39811 + 2.06 ));
				float lerpResult327_g39811 = lerp( smoothstepResult331_g39811 , 0.0 , ( -WN_Z282_g39811 + 2.05 ));
				float4 lerpResult314_g39811 = lerp( Vect_001349_g39811 , VECTOR_IN_MASKED343_g39811 , lerpResult327_g39811);
				float4 MASK_VERTICAL_Z315_g39811 = lerpResult314_g39811;
				float4 Curvature353_g39811 = saturate( ( MASK_VERTICAL_X307_g39811 + MASK_VERTICAL_Y320_g39811 + MASK_VERTICAL_Z315_g39811 ) );
				float4 lerpResult117_g39811 = lerp( _Gradient_BottomSide , _Gradient_Bottom , WN_HORIZONTAL284_g39811);
				float4 lerpResult111_g39811 = lerp( _Gradient_TopSide , _Gradient_Top , WN_HORIZONTAL284_g39811);
				float clampResult106_g39811 = clamp( ( IN.ase_texcoord3.xyz.y / _Gradient_Height ) , 0.0 , 1.0 );
				float4 lerpResult113_g39811 = lerp( lerpResult117_g39811 , lerpResult111_g39811 , clampResult106_g39811);
				float3 temp_output_145_0_g39811 = (( Curvature353_g39811 + lerpResult113_g39811 )).xyz;
				float _Gradient_Ramp154_g39811 = _Gradient_Ramp;
				float _Gradient_Contrast157_g39811 = _Gradient_Contrast;
				float2 appendResult168_g37947 = (float2(_TilingX , _TilingY));
				float2 appendResult167_g37947 = (float2(_OffsetX , _OffsetY));
				float2 texCoord2_g39802 = IN.ase_texcoord4.xy * appendResult168_g37947 + appendResult167_g37947;
				float2 appendResult6_g39802 = (float2(texCoord2_g39802.x , texCoord2_g39802.y));
				float2 OUT_UV213_g37947 = appendResult6_g39802;
				float2 UV40_g39808 = OUT_UV213_g37947;
				float4 tex2DNode63_g39808 = SAMPLE_TEXTURE2D( _MainTex, sampler_trilinear_repeat, UV40_g39808 );
				float4 ALBEDO_RGBA1381_g37947 = tex2DNode63_g39808;
				float4 ALBEDO151_g39811 = ALBEDO_RGBA1381_g37947;
				float smoothstepResult142_g39811 = smoothstep( ( _Gradient_Ramp154_g39811 - ( _Gradient_Ramp154_g39811 * ( 1.0 - _Gradient_Contrast157_g39811 ) ) ) , ALBEDO151_g39811.x , _Gradient_Ramp154_g39811);
				float4 lerpResult139_g39811 = lerp( TINT147_g39811 , float4( temp_output_145_0_g39811 , 0.0 ) , smoothstepResult142_g39811);
				float4 m_Active143_g39811 = lerpResult139_g39811;
				float4 lerpResult141_g39811 = lerp( float4( temp_output_145_0_g39811 , 0.0 ) , TINT147_g39811 , smoothstepResult142_g39811);
				float4 m_ActiveInverted143_g39811 = lerpResult141_g39811;
				float4 localfloat4switch143_g39811 = float4switch143_g39811( m_switch143_g39811 , m_Off143_g39811 , m_Active143_g39811 , m_ActiveInverted143_g39811 );
				float4 temp_output_3_0_g37947 = ( localfloat4switch143_g39811 * float4( (ALBEDO_RGBA1381_g37947).rgb , 0.0 ) * _Brightness );
				float4 temp_output_6_0_g39827 = ( temp_output_3_0_g37947 + float4(0,0,0,0) );
				float4 m_Off1269_g39827 = temp_output_6_0_g39827;
				float2 appendResult1344_g39827 = (float2(_Horizontalwear_TilingX , _Horizontalwear_TilingY));
				float2 appendResult1343_g39827 = (float2(_Horizontalwear_OffsetX , _Horizontalwear_OffsetY));
				float2 texCoord1342_g39827 = IN.ase_texcoord4.xy * appendResult1344_g39827 + appendResult1343_g39827;
				float4 tex2DNode1327_g39827 = SAMPLE_TEXTURE2D( _Horizontalwear_GrungeMap, sampler_trilinear_repeat, texCoord1342_g39827 );
				float4 temp_output_12_0_g39830 = ( float4( (_Horizontalwear_Tint).rgb , 0.0 ) * tex2DNode1327_g39827 );
				float3 desaturateInitialColor10_g39830 = temp_output_12_0_g39830.xyz;
				float desaturateDot10_g39830 = dot( desaturateInitialColor10_g39830, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39830 = lerp( desaturateInitialColor10_g39830, desaturateDot10_g39830.xxx, ( 1.0 - ( _Horizontalwear_Saturation + 1.0 ) ) );
				float3 FINAL_MASK_HORIZONTAL804_g39827 = desaturateVar10_g39830;
				float4 temp_cast_15 = (0.5).xxxx;
				float4 temp_cast_16 = (0.75).xxxx;
				int _Horizontalwear_Mode1003_g39827 = _Horizontalwear_Mode;
				int m_switch1476_g39827 = _Horizontalwear_Mode1003_g39827;
				float4 break91_g39853 = _Horizontalwear_MaskingColor;
				float3 appendResult87_g39853 = (float3(break91_g39853.r , break91_g39853.g , break91_g39853.b));
				int m_switch1506_g39827 = _Horizontalwear_MaskingSource;
				float4 _Horizontalwear_GrungeMap1508_g39827 = tex2DNode1327_g39827;
				float4 m_MapGrunge1506_g39827 = _Horizontalwear_GrungeMap1508_g39827;
				float4 ALBEDO_IN1447_g39827 = temp_output_6_0_g39827;
				float4 m_MapAlbedo1506_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1506_g39827 = float4switch1506_g39827( m_switch1506_g39827 , m_MapGrunge1506_g39827 , m_MapAlbedo1506_g39827 );
				float4 break89_g39853 = localfloat4switch1506_g39827;
				float3 appendResult88_g39853 = (float3(break89_g39853.x , break89_g39853.y , break89_g39853.z));
				float4 temp_cast_18 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1476_g39827 = temp_cast_18;
				float4 temp_cast_19 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1476_g39827 = temp_cast_19;
				float4 temp_cast_20 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39853 , appendResult88_g39853 ) - _Horizontalwear_MaskingColorRange ) / max( _Horizontalwear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1476_g39827 = temp_cast_20;
				float4 localfloat4switch1476_g39827 = float4switch1476_g39827( m_switch1476_g39827 , m_Off1476_g39827 , m_Active1476_g39827 , m_ActiveInverted1476_g39827 );
				float4 temp_output_44_0_g39857 = localfloat4switch1476_g39827;
				float4 VECTOR_IN212_g39857 = temp_output_44_0_g39857;
				float4 _Vec = float4(0.001,0.001,0.001,0.001);
				float4 Vect_0019_g39857 = _Vec;
				float2 _SmoothHorizontal = float2(0,0.5);
				float3 temp_output_40_0_g39857 = abs( ase_worldNormal );
				float WN_HORIZONTAL141_g39857 = ( ( temp_output_40_0_g39857 * temp_output_40_0_g39857 ).y + 0.07 );
				float smoothstepResult205_g39857 = smoothstep( _SmoothHorizontal.x , _SmoothHorizontal.y , ( ( 1.0 - WN_HORIZONTAL141_g39857 ) + -0.1 ));
				float4 lerpResult200_g39857 = lerp( VECTOR_IN212_g39857 , Vect_0019_g39857 , smoothstepResult205_g39857);
				float4 MASK_HORIZONTAL204_g39857 = lerpResult200_g39857;
				float4 temp_cast_21 = (_Horizontalwear_BlendStrength).xxxx;
				float4 temp_cast_22 = (-1.0).xxxx;
				float4 smoothstepResult75_g39857 = smoothstep( temp_cast_15 , temp_cast_16 , (float4( 0,0,0,0 ) + (( 1.0 - MASK_HORIZONTAL204_g39857 ) - temp_cast_21) * (temp_cast_22 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_21)));
				float4 _MinOldVect4 = float4(0,0,0,0);
				float MASK_HORIZONTAL808_g39827 = ( (( ( smoothstepResult75_g39857 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x * _Horizontalwear_BlendMix );
				float4 lerpResult861_g39827 = lerp( temp_output_6_0_g39827 , float4( FINAL_MASK_HORIZONTAL804_g39827 , 0.0 ) , MASK_HORIZONTAL808_g39827);
				float4 m_Active1269_g39827 = lerpResult861_g39827;
				float4 m_ActiveInverted1269_g39827 = lerpResult861_g39827;
				float4 localfloat4switch1269_g39827 = float4switch1269_g39827( m_switch1269_g39827 , m_Off1269_g39827 , m_Active1269_g39827 , m_ActiveInverted1269_g39827 );
				float4 m_Off1377_g39827 = localfloat4switch1269_g39827;
				float2 appendResult1384_g39827 = (float2(_Verticalwear_TilingX , _Verticalwear_TilingY));
				float2 appendResult1386_g39827 = (float2(_Verticalwear_OffsetX , _Verticalwear_OffsetY));
				float2 texCoord1388_g39827 = IN.ase_texcoord4.xy * appendResult1384_g39827 + appendResult1386_g39827;
				float4 tex2DNode1397_g39827 = SAMPLE_TEXTURE2D( _Verticalwear_GrungeMap, sampler_trilinear_repeat, texCoord1388_g39827 );
				float4 temp_output_12_0_g39842 = ( float4( (_Verticalwear_Tint).rgb , 0.0 ) * tex2DNode1397_g39827 );
				float3 desaturateInitialColor10_g39842 = temp_output_12_0_g39842.xyz;
				float desaturateDot10_g39842 = dot( desaturateInitialColor10_g39842, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39842 = lerp( desaturateInitialColor10_g39842, desaturateDot10_g39842.xxx, ( 1.0 - ( _Verticalwear_Saturation + 1.0 ) ) );
				float3 FINAL_MASK_VERTICAL394_g39827 = desaturateVar10_g39842;
				float4 temp_cast_27 = (0.5).xxxx;
				float4 temp_cast_28 = (0.75).xxxx;
				int _Verticalwear_Mode1004_g39827 = _Verticalwear_Mode;
				int m_switch1400_g39827 = _Verticalwear_Mode1004_g39827;
				float4 break91_g39848 = _Verticalwear_MaskingColor;
				float3 appendResult87_g39848 = (float3(break91_g39848.r , break91_g39848.g , break91_g39848.b));
				int m_switch1512_g39827 = _Verticalwear_MaskingSource;
				float4 _Verticalwear_GrungeMap1509_g39827 = tex2DNode1397_g39827;
				float4 m_MapGrunge1512_g39827 = _Verticalwear_GrungeMap1509_g39827;
				float4 m_MapAlbedo1512_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1512_g39827 = float4switch1512_g39827( m_switch1512_g39827 , m_MapGrunge1512_g39827 , m_MapAlbedo1512_g39827 );
				float4 break89_g39848 = localfloat4switch1512_g39827;
				float3 appendResult88_g39848 = (float3(break89_g39848.x , break89_g39848.y , break89_g39848.z));
				float4 temp_cast_30 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1400_g39827 = temp_cast_30;
				float4 temp_cast_31 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1400_g39827 = temp_cast_31;
				float4 temp_cast_32 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39848 , appendResult88_g39848 ) - _Verticalwear_MaskingColorRange ) / max( _Verticalwear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1400_g39827 = temp_cast_32;
				float4 localfloat4switch1400_g39827 = float4switch1400_g39827( m_switch1400_g39827 , m_Off1400_g39827 , m_Active1400_g39827 , m_ActiveInverted1400_g39827 );
				float4 temp_output_44_0_g39828 = localfloat4switch1400_g39827;
				float3 temp_output_40_0_g39828 = abs( ase_worldNormal );
				float WN_HORIZONTAL141_g39828 = ( ( temp_output_40_0_g39828 * temp_output_40_0_g39828 ).y + 0.07 );
				float4 lerpResult2_g39828 = lerp( temp_output_44_0_g39828 , _Vec , WN_HORIZONTAL141_g39828);
				float4 VECTOR_IN_MASKED112_g39828 = lerpResult2_g39828;
				float4 Vect_0019_g39828 = _Vec;
				float3 break349_g39828 = ase_worldNormal;
				float WN_NY173_g39828 = break349_g39828.y;
				float WN_NZ184_g39828 = break349_g39828.z;
				float WN_NX186_g39828 = break349_g39828.x;
				float temp_output_175_0_g39828 = ( -WN_NY173_g39828 + WN_NZ184_g39828 + -WN_NX186_g39828 );
				float4 lerpResult133_g39828 = lerp( VECTOR_IN_MASKED112_g39828 , Vect_0019_g39828 , ( temp_output_175_0_g39828 + -temp_output_175_0_g39828 + 0.07 ));
				float4 MASK_SIDE163_g39828 = lerpResult133_g39828;
				float4 temp_cast_33 = (_Verticalwear_BlendStrength).xxxx;
				float4 temp_cast_34 = (-2.0).xxxx;
				float4 smoothstepResult75_g39828 = smoothstep( temp_cast_27 , temp_cast_28 , (float4( 0,0,0,0 ) + (( 1.0 - MASK_SIDE163_g39828 ) - temp_cast_33) * (temp_cast_34 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_33)));
				float MASK_VERTICAL602_g39827 = ( (( ( smoothstepResult75_g39828 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x * _Verticalwear_BlendMix );
				float4 lerpResult1227_g39827 = lerp( localfloat4switch1269_g39827 , float4( FINAL_MASK_VERTICAL394_g39827 , 0.0 ) , MASK_VERTICAL602_g39827);
				float4 m_Active1377_g39827 = lerpResult1227_g39827;
				float4 m_ActiveInverted1377_g39827 = lerpResult1227_g39827;
				float4 localfloat4switch1377_g39827 = float4switch1377_g39827( m_switch1377_g39827 , m_Off1377_g39827 , m_Active1377_g39827 , m_ActiveInverted1377_g39827 );
				float4 m_Off1380_g39827 = localfloat4switch1377_g39827;
				float2 appendResult1393_g39827 = (float2(_Curvaturewear_TilingX , _Curvaturewear_TilingY));
				float2 appendResult1390_g39827 = (float2(_Curvaturewear_OffsetX , _Curvaturewear_OffsetY));
				float2 texCoord1389_g39827 = IN.ase_texcoord4.xy * appendResult1393_g39827 + appendResult1390_g39827;
				float4 tex2DNode1396_g39827 = SAMPLE_TEXTURE2D( _Curvaturewear_GrungeMap, sampler_trilinear_repeat, texCoord1389_g39827 );
				float4 temp_output_12_0_g39834 = ( float4( (_Curvaturewear_Tint).rgb , 0.0 ) * tex2DNode1396_g39827 );
				float3 desaturateInitialColor10_g39834 = temp_output_12_0_g39834.xyz;
				float desaturateDot10_g39834 = dot( desaturateInitialColor10_g39834, float3( 0.299, 0.587, 0.114 ));
				float3 desaturateVar10_g39834 = lerp( desaturateInitialColor10_g39834, desaturateDot10_g39834.xxx, ( 1.0 - ( _Curvaturewear_Saturation + 1.0 ) ) );
				float4 lerpResult1544_g39827 = lerp( ALBEDO_IN1447_g39827 , float4( desaturateVar10_g39834 , 0.0 ) , _Curvaturewear_BlendMix);
				float4 FINAL_MASK_CURVATURE586_g39827 = lerpResult1544_g39827;
				float4 temp_cast_39 = (0.5).xxxx;
				float4 temp_cast_40 = (0.75).xxxx;
				int _Curvaturewear_Mode398_g39827 = _Curvaturewear_Mode;
				int m_switch1491_g39827 = _Curvaturewear_Mode398_g39827;
				float4 break91_g39855 = _Curvaturewear_MaskingColor;
				float3 appendResult87_g39855 = (float3(break91_g39855.r , break91_g39855.g , break91_g39855.b));
				int m_switch1516_g39827 = _Curvaturewear_MaskingSource;
				float4 _Curvaturewear_GrungeMap1510_g39827 = tex2DNode1396_g39827;
				float4 m_MapGrunge1516_g39827 = _Curvaturewear_GrungeMap1510_g39827;
				float4 m_MapAlbedo1516_g39827 = ALBEDO_IN1447_g39827;
				float4 localfloat4switch1516_g39827 = float4switch1516_g39827( m_switch1516_g39827 , m_MapGrunge1516_g39827 , m_MapAlbedo1516_g39827 );
				float4 break89_g39855 = localfloat4switch1516_g39827;
				float3 appendResult88_g39855 = (float3(break89_g39855.x , break89_g39855.y , break89_g39855.z));
				float4 temp_cast_42 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Off1491_g39827 = temp_cast_42;
				float4 temp_cast_43 = (saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) )).xxxx;
				float4 m_Active1491_g39827 = temp_cast_43;
				float4 temp_cast_44 = (saturate( ( 1.0 - saturate( ( 1.0 - ( ( distance( appendResult87_g39855 , appendResult88_g39855 ) - _Curvaturewear_MaskingColorRange ) / max( _Curvaturewear_MaskingColorFuzziness , 1E-05 ) ) ) ) ) )).xxxx;
				float4 m_ActiveInverted1491_g39827 = temp_cast_44;
				float4 localfloat4switch1491_g39827 = float4switch1491_g39827( m_switch1491_g39827 , m_Off1491_g39827 , m_Active1491_g39827 , m_ActiveInverted1491_g39827 );
				float4 temp_output_44_0_g39836 = localfloat4switch1491_g39827;
				float3 temp_output_40_0_g39836 = abs( ase_worldNormal );
				float WN_HORIZONTAL141_g39836 = ( ( temp_output_40_0_g39836 * temp_output_40_0_g39836 ).y + 0.07 );
				float4 lerpResult2_g39836 = lerp( temp_output_44_0_g39836 , _Vec , WN_HORIZONTAL141_g39836);
				float4 VECTOR_IN_MASKED112_g39836 = lerpResult2_g39836;
				float4 Vect_0019_g39836 = _Vec;
				float2 _SmoothX = float2(0,0.45);
				float3 temp_cast_45 = (0.5).xxx;
				float3 break24_g39836 = ( temp_output_40_0_g39836 - temp_cast_45 );
				float WN_X134_g39836 = break24_g39836.x;
				float smoothstepResult8_g39836 = smoothstep( _SmoothX.x , _SmoothX.y , ( WN_X134_g39836 + 1.25 ));
				float smoothstepResult22_g39836 = smoothstep( _SmoothX.x , _SmoothX.y , ( -WN_X134_g39836 + 0.02 ));
				float lerpResult11_g39836 = lerp( 0.0 , smoothstepResult8_g39836 , smoothstepResult22_g39836);
				float4 lerpResult14_g39836 = lerp( VECTOR_IN_MASKED112_g39836 , Vect_0019_g39836 , lerpResult11_g39836);
				float4 MASK_VERTICAL_X159_g39836 = lerpResult14_g39836;
				float2 _SmoothY = float2(0,0.85);
				float WN_Y135_g39836 = break24_g39836.y;
				float smoothstepResult29_g39836 = smoothstep( _SmoothY.x , _SmoothY.y , ( -WN_Y135_g39836 + 0.35 ));
				float4 lerpResult6_g39836 = lerp( VECTOR_IN_MASKED112_g39836 , Vect_0019_g39836 , smoothstepResult29_g39836);
				float4 MASK_VERTICAL_Y157_g39836 = lerpResult6_g39836;
				float2 _SmoothZ = float2(1.8,1.4);
				float WN_Z136_g39836 = break24_g39836.z;
				float smoothstepResult26_g39836 = smoothstep( _SmoothZ.x , _SmoothZ.y , ( WN_Z136_g39836 + 2.06 ));
				float lerpResult34_g39836 = lerp( smoothstepResult26_g39836 , 0.0 , ( -WN_Z136_g39836 + 2.05 ));
				float4 lerpResult5_g39836 = lerp( Vect_0019_g39836 , VECTOR_IN_MASKED112_g39836 , lerpResult34_g39836);
				float4 MASK_VERTICAL_Z155_g39836 = lerpResult5_g39836;
				float4 temp_cast_46 = (_Curvaturewear_BlendStrength).xxxx;
				float4 temp_cast_47 = (_Curvaturewear_BlendHardness).xxxx;
				float4 smoothstepResult75_g39836 = smoothstep( temp_cast_39 , temp_cast_40 , (float4( 0,0,0,0 ) + (( 1.0 - ( MASK_VERTICAL_X159_g39836 + MASK_VERTICAL_Y157_g39836 + MASK_VERTICAL_Z155_g39836 ) ) - temp_cast_46) * (temp_cast_47 - float4( 0,0,0,0 )) / (float4( 1,1,1,1 ) - temp_cast_46)));
				float MASK_CURVATURE401_g39827 = (( ( smoothstepResult75_g39836 - _MinOldVect4 ) / ( float4(1,1,1,1) - _MinOldVect4 ) )).x;
				float4 lerpResult860_g39827 = lerp( FINAL_MASK_CURVATURE586_g39827 , localfloat4switch1377_g39827 , MASK_CURVATURE401_g39827);
				float4 m_Active1380_g39827 = lerpResult860_g39827;
				float4 m_ActiveInverted1380_g39827 = lerpResult860_g39827;
				float4 localfloat4switch1380_g39827 = float4switch1380_g39827( m_switch1380_g39827 , m_Off1380_g39827 , m_Active1380_g39827 , m_ActiveInverted1380_g39827 );
				
				float ALBEDO_A926_g37947 = tex2DNode63_g39808.a;
				int m_switch1287_g37947 = _GlancingClipMode;
				float m_Off1287_g37947 = 1.0;
				float3 ase_worldViewDir = ( _WorldSpaceCameraPos.xyz - WorldPosition );
				ase_worldViewDir = SafeNormalize( ase_worldViewDir );
				float3 normalizeResult1294_g37947 = normalize( cross( ddx( WorldPosition ) , ddy( WorldPosition ) ) );
				float dotResult1301_g37947 = dot( ase_worldViewDir , normalizeResult1294_g37947 );
				float temp_output_1321_0_g37947 = ( 1.0 - abs( dotResult1301_g37947 ) );
				float m_Active1287_g37947 = ( 1.0 - ( temp_output_1321_0_g37947 * temp_output_1321_0_g37947 ) );
				float localfloatswitch1287_g37947 = floatswitch1287_g37947( m_switch1287_g37947 , m_Off1287_g37947 , m_Active1287_g37947 );
				float OPACITY_OUTMASK1277_g37947 = localfloatswitch1287_g37947;
				
				float AlphaCutoffBias1289_g37947 = _AlphaCutoffBias;
				
				
				float3 Albedo = localfloat4switch1380_g39827.xyz;
				float Alpha = ( ALBEDO_A926_g37947 * OPACITY_OUTMASK1277_g37947 );
				float AlphaClipThreshold = AlphaCutoffBias1289_g37947;

				half4 color = half4( Albedo, Alpha );

				#ifdef _ALPHATEST_ON
					clip(Alpha - AlphaClipThreshold);
				#endif

				return color;
			}
			ENDHLSL
		}
		
	}
	/*ase_lod*/
	CustomEditor "UnityEditor.ShaderGraph.PBRMasterGUI"
	Fallback " "
	
}
/*ASEBEGIN
Version=18913
4;29.33333;1436;766;5314.704;4005.26;1.3;True;False
Node;AmplifyShaderEditor.CommentaryNode;2820;-3949.45,-3840.181;Inherit;False;385.5028;187.4201;;4;2849;2760;2758;202;DEBUG SETTINGS ;0,0,0,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;2871;-3949.061,-3329.948;Inherit;False;315.6667;113;DESF Common ASE Compile Shaders;1;2872;;0,0.2047877,1,1;0;0
Node;AmplifyShaderEditor.CommentaryNode;2819;-3949.129,-4000.892;Inherit;False;175.5;121.8;;1;2848;GLOBAL SETTINGS ;0,0,0,1;0;0
Node;AmplifyShaderEditor.FunctionNode;2874;-4726.247,-3645.134;Inherit;False;DESF Core Cutout;4;;37947;e0cdd7758f4404849b063afff4596424;26,1217,2,1311,2,1296,2,1156,2,1141,2,442,2,1509,0,1556,1,1557,1,1552,1,1309,1,1308,1,1333,0,1348,0,1596,0,1368,2,1569,0,96,0,1229,0,1236,0,830,0,1415,0,1399,0,1564,0,1566,0,1565,0;2;1234;FLOAT3;0,0,0;False;1274;FLOAT4;0,0,0,0;False;14;FLOAT4;38;FLOAT3;35;FLOAT3;1586;COLOR;37;FLOAT4;33;COLOR;34;FLOAT;441;FLOAT;46;FLOAT;814;FLOAT;552;FLOAT4;656;FLOAT4;657;FLOAT4;655;FLOAT3;1235
Node;AmplifyShaderEditor.IntNode;2760;-3757.712,-3807.285;Inherit;False;Property;_ZWriteMode;ZWrite Mode;0;2;[Header];[Enum];Create;False;1;DEBUG SETTINGS;0;1;Off,0,On,1;True;0;False;1;1;False;0;1;INT;0
Node;AmplifyShaderEditor.FunctionNode;2873;-4327.556,-3641.438;Inherit;False;DESF Module Grunge;119;;39827;b0925d725c9bfd049b8ad9f4b41a5bd0;3,1487,1,1466,1,1473,1;4;6;FLOAT4;0,0,0,0;False;8;FLOAT3;0,0,0;False;977;FLOAT4;0,0,0,0;False;989;FLOAT4;0,0,0,0;False;4;FLOAT4;0;FLOAT3;9;FLOAT;991;FLOAT;990
Node;AmplifyShaderEditor.RangedFloatNode;2849;-3756.048,-3730.398;Inherit;False;Property;_AlphatoCoverage;Alpha to Coverage;2;1;[Enum];Create;False;1;;1;Option1;0;1;Off,0,On,1;True;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.FunctionNode;2872;-3939.061,-3290.948;Inherit;False;DESF Common ASE Compile Shaders;-1;;39826;b85b01c42ba8a8a448b731b68fc0dbd9;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;2758;-3946.076,-3729.627;Inherit;False;Property;_ColorMask;Color Mask Mode;1;1;[Enum];Create;False;1;;0;1;None,0,Alpha,1,Red,8,Green,4,Blue,2,RGB,14,RGBA,15;True;0;False;15;15;False;0;1;INT;0
Node;AmplifyShaderEditor.RangedFloatNode;202;-3941.471,-3806.078;Inherit;False;Constant;_MaskClipValue;Mask Clip Value;64;1;[HideInInspector];Create;True;1;;0;0;True;0;False;0.5;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.IntNode;2848;-3925.649,-3961.208;Inherit;False;Property;_CullMode;Cull Mode;3;2;[Header];[Enum];Create;True;1;GLOBAL SETTINGS;0;1;UnityEngine.Rendering.CullMode;True;0;False;0;2;False;0;1;INT;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2841;-3953.374,-3640.745;Float;False;True;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;DEC/Grunge/Grunge Cutout Mask Color;94348b07e5e8bab40bd6c8a1e3df54cd;True;Forward;0;1;Forward;18;False;False;False;False;False;False;False;False;False;False;False;False;True;0;True;2849;False;True;2;True;2848;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;4;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=-10;NatureRendererInstancing=True;True;3;True;18;all;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;True;2758;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;True;2760;True;3;False;2757;True;True;0;False;-1;0;False;-1;True;2;LightMode=UniversalForward;NatureRendererInstancing=True;False;False;8;Include;;False;;Native;Pragma;multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS;False;;Custom;Pragma;multi_compile _ _ADDITIONAL_LIGHT_SHADOWS;False;;Custom;Pragma;multi_compile_instancing;False;;Custom;Pragma;instancing_options procedural:SetupNatureRenderer forwardadd;False;;Custom;Pragma;multi_compile GPU_FRUSTUM_ON __;False;;Custom;Include;Nature Renderer.cginc;False;ed9205546b797304ea7576ba0b32877e;Custom;Pragma;multi_compile_local _ NATURE_RENDERER;False;;Custom; ;0;0;Standard;38;Workflow;1;Surface;0;  Refraction Model;0;  Blend;0;Two Sided;0;Fragment Normal Space,InvertActionOnDeselection;0;Transmission;0;  Transmission Shadow;0.5,False,2863;Translucency;0;  Translucency Strength;1,False,2864;  Normal Distortion;0.5,False,2865;  Scattering;2,False,2866;  Direct;0.9,False,2862;  Ambient;0.1,False,2868;  Shadow;0.5,False,2867;Cast Shadows;1;  Use Shadow Threshold;1;Receive Shadows;1;GPU Instancing;1;LOD CrossFade;1;Built-in Fog;1;_FinalColorxAlpha;0;Meta Pass;1;Override Baked GI;0;Extra Pre Pass;0;DOTS Instancing;0;Tessellation;0;  Phong;0;  Strength;0.5,False,-1;  Type;0;  Tess;16,False,-1;  Min;10,False,-1;  Max;25,False,-1;  Edge Length;16,False,-1;  Max Displacement;25,False,-1;Write Depth;0;  Early Z;0;Vertex Position,InvertActionOnDeselection;1;0;6;False;True;True;True;True;True;False;;True;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2840;-3601.374,-3640.745;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ExtraPrePass;0;0;ExtraPrePass;5;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;18;all;0;False;True;1;1;False;-1;0;False;-1;0;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;True;True;True;True;0;False;-1;False;False;False;False;False;False;False;True;False;255;False;-1;255;False;-1;255;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;7;False;-1;1;False;-1;1;False;-1;1;False;-1;False;True;1;False;-1;True;3;False;-1;True;True;0;False;-1;0;False;-1;True;0;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2844;-3941.374,-3630.745;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Meta;0;4;Meta;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;18;all;0;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;2;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;LightMode=Meta;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2845;-3941.374,-3630.745;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;Universal2D;0;5;Universal2D;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;18;all;0;False;True;1;1;False;-1;0;False;-1;1;1;False;-1;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;True;True;True;True;0;True;2758;False;False;False;False;False;False;False;False;False;True;1;True;2760;True;3;False;2757;True;True;0;False;-1;0;False;-1;True;1;LightMode=Universal2D;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2842;-3941.374,-3630.745;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;ShadowCaster;0;2;ShadowCaster;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;18;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;True;1;False;-1;True;3;False;-1;False;True;1;LightMode=ShadowCaster;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
Node;AmplifyShaderEditor.TemplateMultiPassMasterNode;2843;-3941.374,-3630.745;Float;False;False;-1;2;UnityEditor.ShaderGraph.PBRMasterGUI;0;2;New Amplify Shader;94348b07e5e8bab40bd6c8a1e3df54cd;True;DepthOnly;0;3;DepthOnly;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;True;0;False;-1;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;3;RenderPipeline=UniversalPipeline;RenderType=Opaque=RenderType;Queue=Geometry=Queue=0;True;0;True;18;all;0;False;False;False;False;False;False;False;False;False;False;False;False;True;0;False;-1;False;False;False;True;False;False;False;False;0;False;-1;False;False;False;False;False;False;False;False;False;True;1;False;-1;False;False;True;1;LightMode=DepthOnly;False;False;0;Hidden/InternalErrorShader;0;0;Standard;0;False;0
WireConnection;2873;6;2874;38
WireConnection;2873;8;2874;35
WireConnection;2873;977;2874;37
WireConnection;2873;989;2874;33
WireConnection;2841;0;2873;0
WireConnection;2841;1;2873;9
WireConnection;2841;3;2873;991
WireConnection;2841;4;2873;990
WireConnection;2841;5;2874;34
WireConnection;2841;6;2874;46
WireConnection;2841;7;2874;814
WireConnection;2841;16;2874;552
ASEEND*/
//CHKSM=73158EFECFAA83D7374AC452FE8B5F2570F2E138