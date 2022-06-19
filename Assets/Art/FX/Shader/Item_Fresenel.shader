// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/Item"
{
	Properties
	{
		_Bias("Bias", Float) = 0.15
		_Scale("Scale", Float) = 0.64
		_Power("Power", Float) = 1.6
		[HDR]_Color0("Color 0", Color) = (0,0,0,0)
		_TextureSample0("Texture Sample 0", 2D) = "white" {}
		_TextureSample1("Texture Sample 1", 2D) = "white" {}
		_MainTexxy("MainTex x/y", Vector) = (0.1,0.1,0,0)
		_Noisexy("Noisex/y", Vector) = (-0.1,-0.1,0,0)
		_NoiseFloat("NoiseFloat", Range( 0 , 1)) = -0.31
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Off
		Blend One One , One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float3 worldPos;
			float3 worldNormal;
			float2 uv_texcoord;
		};

		uniform float4 _Color0;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;
		uniform sampler2D _TextureSample0;
		uniform float2 _MainTexxy;
		uniform sampler2D _TextureSample1;
		uniform float2 _Noisexy;
		uniform float _NoiseFloat;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV1 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode1 = ( _Bias + _Scale * pow( 1.0 - fresnelNdotV1, _Power ) );
			float2 panner12 = ( 1.0 * _Time.y * _MainTexxy + float2( 0,0 ));
			float2 uv_TexCoord11 = i.uv_texcoord + panner12;
			float2 panner16 = ( 1.0 * _Time.y * _Noisexy + float2( 0,0 ));
			float2 uv_TexCoord17 = i.uv_texcoord + panner16;
			float4 tex2DNode10 = tex2D( _TextureSample0, ( uv_TexCoord11 + ( tex2D( _TextureSample1, uv_TexCoord17 ).r * _NoiseFloat ) ) );
			float4 temp_output_8_0 = ( _Color0 * ( i.vertexColor * fresnelNode1 ) * tex2DNode10 );
			o.Emission = temp_output_8_0.rgb;
			o.Alpha = ( i.vertexColor.a * tex2DNode10.a * temp_output_8_0 ).r;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;203;1920;807;1872.698;46.58331;1;True;False
Node;AmplifyShaderEditor.Vector2Node;18;-1445.08,876.0817;Inherit;False;Property;_Noisexy;Noisex/y;9;0;Create;True;0;0;False;0;False;-0.1,-0.1;-0.1,-0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;16;-1295.985,752.0817;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;13;-1216.251,485.7597;Inherit;False;Property;_MainTexxy;MainTex x/y;8;0;Create;True;0;0;False;0;False;0.1,0.1;0.1,0.1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;17;-1070.985,682.0817;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;15;-926.3188,738.415;Inherit;True;Property;_TextureSample1;Texture Sample 1;7;0;Create;True;0;0;False;0;False;-1;None;8180197acbfdb484d9f4435d71d330f6;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;19;-699.88,977.8981;Inherit;False;Property;_NoiseFloat;NoiseFloat;10;0;Create;True;0;0;False;0;False;-0.31;0.123;0;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;12;-974.3188,462.415;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;20;-554.88,769.8981;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-926.6865,283.4832;Inherit;False;Property;_Power;Power;3;0;Create;True;0;0;False;0;False;1.6;4.4;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;11;-788.3188,393.415;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;2;-970.6865,62.48325;Inherit;False;Property;_Bias;Bias;1;0;Create;True;0;0;False;0;False;0.15;0.1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-960.6865,162.4832;Inherit;False;Property;_Scale;Scale;2;0;Create;True;0;0;False;0;False;0.64;20.13;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;14;-367.3188,596.415;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.FresnelNode;1;-766.7336,54.39129;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;6;-656.7316,-191.7804;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;9;-439.0987,-278.0804;Inherit;False;Property;_Color0;Color 0;5;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;1,1,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;10;-115.0286,442.7987;Inherit;True;Property;_TextureSample0;Texture Sample 0;6;0;Create;True;0;0;False;0;False;-1;None;67a416f2831a4cbcb606f345d4f2a996;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-312.7279,-54.77999;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;32.75208,-40.92184;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-46.43579,-354.2725;Inherit;False;Property;_HDR;HDR;4;0;Create;True;0;0;False;0;False;0;0.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;21;333.8453,227.8145;Inherit;False;3;3;0;FLOAT;0;False;1;FLOAT;0;False;2;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;487.9811,40.24355;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Unlit/Item;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Off;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Opaque;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;16;2;18;0
WireConnection;17;1;16;0
WireConnection;15;1;17;0
WireConnection;12;2;13;0
WireConnection;20;0;15;1
WireConnection;20;1;19;0
WireConnection;11;1;12;0
WireConnection;14;0;11;0
WireConnection;14;1;20;0
WireConnection;1;1;2;0
WireConnection;1;2;3;0
WireConnection;1;3;4;0
WireConnection;10;1;14;0
WireConnection;5;0;6;0
WireConnection;5;1;1;0
WireConnection;8;0;9;0
WireConnection;8;1;5;0
WireConnection;8;2;10;0
WireConnection;21;0;6;4
WireConnection;21;1;10;4
WireConnection;21;2;8;0
WireConnection;0;2;8;0
WireConnection;0;9;21;0
ASEEND*/
//CHKSM=DA62F620E1209BB864595A2A486BE3E0A9CE618B