// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Fx_KailR_Rotation"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_DistortTex("DistortTex", 2D) = "white" {}
		_DistortAmount("DistortAmount", Range( 0 , 0.1)) = 0.04304536
		_DistortPannerXY("DistortPanner X/Y", Vector) = (0.25,0,0,0)
		_NoiseTex("NoiseTex", 2D) = "white" {}
		_tint("tint", Color) = (0.3679245,0.3679245,0.3679245,0)
		_DistortPanner("DistortPanner", Vector) = (0.35,0,0,0)
		_MaskTex("MaskTex", 2D) = "white" {}
		_custom1("custom1", Vector) = (0,0,0,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] _tex4coord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZWrite Off
		ZTest LEqual
		Blend One One , One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		#undef TRANSFORM_TEX
		#define TRANSFORM_TEX(tex,name) float4(tex.xy * name##_ST.xy + name##_ST.zw, tex.z, tex.w)
		struct Input
		{
			float4 vertexColor : COLOR;
			float2 uv_texcoord;
			float4 uv_tex4coord;
		};

		uniform sampler2D _MainTex;
		uniform float4 _MainTex_ST;
		uniform float _DistortAmount;
		uniform sampler2D _DistortTex;
		uniform float2 _DistortPannerXY;
		uniform float4 _tint;
		uniform float2 _custom1;
		uniform sampler2D _NoiseTex;
		uniform float2 _DistortPanner;
		uniform sampler2D _MaskTex;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv0_MainTex = i.uv_texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
			float2 panner6 = ( 1.0 * _Time.y * float2( 0,0 ) + uv0_MainTex);
			float2 panner9 = ( 1.0 * _Time.y * _DistortPannerXY + i.uv_texcoord);
			float temp_output_29_0 = ( i.uv_tex4coord.w + _custom1.y );
			float2 panner18 = ( 1.0 * _Time.y * _DistortPanner + i.uv_texcoord);
			float4 tex2DNode17 = tex2D( _NoiseTex, panner18 );
			float smoothstepResult30 = smoothstep( temp_output_29_0 , 1.0 , tex2DNode17.r);
			o.Emission = ( i.vertexColor * tex2D( _MainTex, ( panner6 + ( _DistortAmount * (-1.0 + (tex2D( _DistortTex, panner9 ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) ) ) * i.vertexColor.a * _tint * smoothstepResult30 * tex2D( _MaskTex, i.uv_texcoord ).r * ( _custom1.x + i.uv_tex4coord.z ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
6;136;1906;820;2855.354;-256.576;1;True;False
Node;AmplifyShaderEditor.Vector2Node;11;-2409.442,380.3066;Inherit;False;Property;_DistortPannerXY;DistortPanner X/Y;4;0;Create;True;0;0;False;0;False;0.25,0;0.25,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;10;-2443.441,245.3065;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;9;-2194.442,322.3066;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;8;-1942.086,285.7331;Inherit;True;Property;_DistortTex;DistortTex;2;0;Create;True;0;0;False;0;False;-1;None;af13a6efed4967243bbaeb49682efc34;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;19;-2222.384,646.1315;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;5;-1752.711,-186.6204;Inherit;False;0;3;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;21;-2178.428,857.764;Inherit;False;Property;_DistortPanner;DistortPanner;7;0;Create;True;0;0;False;0;False;0.35,0;0.35,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;14;-1756.209,146.8325;Inherit;False;Property;_DistortAmount;DistortAmount;3;0;Create;True;0;0;False;0;False;0.04304536;0;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;12;-1652.356,313.7913;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;7;-1681.211,-35.82038;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.Vector2Node;27;-1234.851,604.9415;Inherit;False;Property;_custom1;custom1;9;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;18;-1855.384,710.1315;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;13;-1425.408,279.8325;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;26;-1270.778,753.6266;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;6;-1517.414,-145.0205;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;15;-1245.511,64.30943;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleAddOpNode;29;-919.5972,711.3032;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;25;-805.4791,363.7227;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;17;-1586.524,614.0054;Inherit;True;Property;_NoiseTex;NoiseTex;5;0;Create;True;0;0;False;0;False;-1;None;47ae85ae30923d04ba40b3a619be1e81;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;28;-581.817,226.6251;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;3;-923.9994,35.79996;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;False;-1;None;9e9c58190a6a84b45a75c4bb15ec96ef;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;16;-696.2786,-726.2518;Inherit;False;Property;_tint;tint;6;0;Create;True;0;0;False;0;False;0.3679245,0.3679245,0.3679245,0;0.3679245,0.3679245,0.3679245,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;24;-585.5795,445.8226;Inherit;True;Property;_MaskTex;MaskTex;8;0;Create;True;0;0;False;0;False;-1;None;cb75b00174766a843805258e7d449c22;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;1;-505.7989,-944.3008;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SmoothstepOpNode;30;-1103.37,362.1176;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;39;-2003.15,-536.2571;Inherit;False;Property;_RimPower;RimPower;12;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;37;-2003.409,-740.0925;Inherit;False;Property;_RimBias;RimBias;10;0;Create;True;0;0;False;0;False;0;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;36;-1683.545,-492.3361;Inherit;False;Property;_emission;emission;13;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;38;-2041.769,-637.7036;Inherit;False;Property;_RimScale;RimScale;11;0;Create;True;0;0;False;0;False;1;1;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;40;-1774.522,-1121.534;Inherit;False;Constant;_TintColor;TintColor;14;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;31;-1849.527,-688.9031;Inherit;False;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;43;-1401.656,-867.0026;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;44;-539.9936,-344.5208;Inherit;False;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;33;-1540.598,-693.8808;Inherit;True;3;3;0;COLOR;0,0,0,0;False;1;FLOAT;0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;2;-261,-148.5;Inherit;True;7;7;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StepOpNode;22;-897.5387,352.1712;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.ColorNode;32;-1787.171,-912.6186;Inherit;False;Property;_rimColor;rimColor;14;0;Create;True;0;0;False;0;False;0,0,0,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;218.0679,-25.94326;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Fx_KailR_Rotation;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;9;0;10;0
WireConnection;9;2;11;0
WireConnection;8;1;9;0
WireConnection;12;0;8;1
WireConnection;18;0;19;0
WireConnection;18;2;21;0
WireConnection;13;0;14;0
WireConnection;13;1;12;0
WireConnection;6;0;5;0
WireConnection;6;2;7;0
WireConnection;15;0;6;0
WireConnection;15;1;13;0
WireConnection;29;0;26;4
WireConnection;29;1;27;2
WireConnection;17;1;18;0
WireConnection;28;0;27;1
WireConnection;28;1;26;3
WireConnection;3;1;15;0
WireConnection;24;1;25;0
WireConnection;30;0;17;1
WireConnection;30;1;29;0
WireConnection;31;1;37;0
WireConnection;31;2;38;0
WireConnection;31;3;39;0
WireConnection;43;0;40;0
WireConnection;43;1;33;0
WireConnection;33;0;32;0
WireConnection;33;1;31;0
WireConnection;33;2;36;0
WireConnection;2;0;1;0
WireConnection;2;1;3;0
WireConnection;2;2;1;4
WireConnection;2;3;16;0
WireConnection;2;4;30;0
WireConnection;2;5;24;1
WireConnection;2;6;28;0
WireConnection;22;0;17;1
WireConnection;22;1;29;0
WireConnection;0;2;2;0
ASEEND*/
//CHKSM=7801C39BAA9A11C03F1F04FB1F80D124F4E8505A