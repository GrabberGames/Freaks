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
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Opaque"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend One One , One One
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float4 vertexColor : COLOR;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform float4 _Color0;
		uniform float _Bias;
		uniform float _Scale;
		uniform float _Power;

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
			o.Emission = ( _Color0 * ( i.vertexColor * fresnelNode1 ) ).rgb;
			o.Alpha = i.vertexColor.a;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
485;190;1626;987;1170.371;341.4424;1.003864;True;False
Node;AmplifyShaderEditor.RangedFloatNode;2;-970.6865,62.48325;Inherit;False;Property;_Bias;Bias;1;0;Create;True;0;0;False;0;False;0.15;0.24;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;4;-926.6865,283.4832;Inherit;False;Property;_Power;Power;3;0;Create;True;0;0;False;0;False;1.6;2.32;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;3;-960.6865,162.4832;Inherit;False;Property;_Scale;Scale;2;0;Create;True;0;0;False;0;False;0.64;2.08;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.VertexColorNode;6;-656.7316,-191.7804;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;1;-766.7336,54.39129;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;5;-372.7316,-60.78036;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.ColorNode;9;-439.0987,-278.0804;Inherit;False;Property;_Color0;Color 0;5;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;0.5764706,0.7490196,0.3254902,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;8;-224.4358,-108.2725;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.RangedFloatNode;7;-46.43579,-354.2725;Inherit;False;Property;_HDR;HDR;4;0;Create;True;0;0;False;0;False;0;0.69;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-14,2;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Unlit/Item;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Opaque;;Overlay;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;1;2;0
WireConnection;1;2;3;0
WireConnection;1;3;4;0
WireConnection;5;0;6;0
WireConnection;5;1;1;0
WireConnection;8;0;9;0
WireConnection;8;1;5;0
WireConnection;0;2;8;0
WireConnection;0;9;6;4
ASEEND*/
//CHKSM=2CE3D1B940D1E98D12105B95AE75F865FBA3092F