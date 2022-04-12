// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Unlit/FX_TowerDissolve"
{
	Properties
	{
		_MainTexture("MainTexture", 2D) = "white" {}
		_Noise("Noise", 2D) = "white" {}
		[HDR]_EdgeColor("EdgeColor", Color) = (0,0,0,0)
		_Dissolve("Dissolve", Range( -0.1 , 1)) = 0
		_AmbientOcclusion("Ambient Occlusion", Float) = 0
		_Metalic("Metalic", Float) = 0
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Transparent"  "Queue" = "Overlay+0" "IgnoreProjector" = "True" "IsEmissive" = "true"  }
		Cull Back
		Blend SrcAlpha OneMinusSrcAlpha
		
		CGPROGRAM
		#pragma target 3.0
		#pragma surface surf Standard keepalpha noshadow exclude_path:deferred 
		struct Input
		{
			float2 uv_texcoord;
		};

		uniform sampler2D _MainTexture;
		uniform float4 _MainTexture_ST;
		uniform float4 _EdgeColor;
		uniform sampler2D _Noise;
		uniform float4 _Noise_ST;
		uniform float _Dissolve;
		uniform float _Metalic;
		uniform float _AmbientOcclusion;

		void surf( Input i , inout SurfaceOutputStandard o )
		{
			float2 uv_MainTexture = i.uv_texcoord * _MainTexture_ST.xy + _MainTexture_ST.zw;
			o.Albedo = tex2D( _MainTexture, uv_MainTexture ).rgb;
			float2 uv_Noise = i.uv_texcoord * _Noise_ST.xy + _Noise_ST.zw;
			float4 tex2DNode10 = tex2D( _Noise, uv_Noise );
			o.Emission = ( _EdgeColor * step( ( tex2DNode10.r - 0.18 ) , _Dissolve ) ).rgb;
			o.Metallic = _Metalic;
			o.Occlusion = _AmbientOcclusion;
			o.Alpha = ( 1.0 - step( tex2DNode10.r , _Dissolve ) );
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;19;1906;992;1286.762;113.8876;1;True;False
Node;AmplifyShaderEditor.SamplerNode;10;-1800.234,676.8563;Inherit;True;Property;_Noise;Noise;2;0;Create;True;0;0;False;0;False;-1;2384e7efadb2b944f8dc03c05b5b3b52;a5c29e9ff4caff744b617e47e72ce765;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleSubtractOpNode;9;-1371.321,508.9231;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0.18;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-1850.67,981.0344;Inherit;False;Property;_Dissolve;Dissolve;4;0;Create;True;0;0;False;0;False;0;-0.1;-0.1;1;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;5;-1497.622,240.7793;Inherit;False;Property;_EdgeColor;EdgeColor;3;1;[HDR];Create;True;0;0;False;0;False;0,0,0,0;4,0.972549,0.972549,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;8;-1095.247,638.0163;Inherit;True;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.StepOpNode;12;-1306.584,1027.324;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;6;-746.3082,346.5172;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.OneMinusNode;13;-917.2016,878.8445;Inherit;True;1;0;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;4;-772.2663,-32.78513;Inherit;True;Property;_MainTexture;MainTexture;1;0;Create;True;0;0;False;0;False;-1;96bbeab06e689ba49aa9b08ab84262b1;0e7f3a0008bf39448b28a169f8b32534;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.RangedFloatNode;20;-204.1172,101.1784;Inherit;False;Property;_Metalic;Metalic;6;0;Create;True;0;0;False;0;False;0;0.44;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;22;-209.1172,248.1784;Inherit;False;Property;_AmbientOcclusion;Ambient Occlusion;5;0;Create;True;0;0;False;0;False;0;4.93;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;0,0;Float;False;True;-1;2;ASEMaterialInspector;0;0;Standard;Unlit/FX_TowerDissolve;False;False;False;False;False;False;False;False;False;False;False;False;False;False;True;False;False;False;False;False;False;Back;0;False;-1;0;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Transparent;;Overlay;ForwardOnly;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;2;5;False;-1;10;False;-1;0;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;16;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;10;1
WireConnection;8;0;9;0
WireConnection;8;1;11;0
WireConnection;12;0;10;1
WireConnection;12;1;11;0
WireConnection;6;0;5;0
WireConnection;6;1;8;0
WireConnection;13;0;12;0
WireConnection;0;0;4;0
WireConnection;0;2;6;0
WireConnection;0;3;20;0
WireConnection;0;5;22;0
WireConnection;0;9;13;0
ASEEND*/
//CHKSM=4980CD56E15F75D2732B5ACB6F053C25F9FABC28