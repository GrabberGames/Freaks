// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Christal_Small_SH"
{
	Properties
	{
		_Texture0("Texture 0", 2D) = "white" {}
		_speed(" speed", Vector) = (1,0.25,0,0)
		_FresnelBias("FresnelBias", Float) = 0.08
		_FresnelScale("FresnelScale", Float) = 1
		_FresnelPower("FresnelPower", Float) = 1
		_Color0("Color 0", Color) = (0,0.990309,1,0)
		_Color1("Color 1", Color) = (0,0.1481996,1,0)
		[HideInInspector] _texcoord( "", 2D ) = "white" {}
		[HideInInspector] __dirty( "", Int ) = 1
	}

	SubShader
	{
		Tags{ "RenderType" = "Overlay"  "Queue" = "Transparent+0" "IsEmissive" = "true"  }
		Cull Off
		ZTest LEqual
		Blend One One , One One
		
		CGPROGRAM
		#include "UnityShaderVariables.cginc"
		#pragma target 3.0
		#pragma surface surf Unlit keepalpha noshadow 
		struct Input
		{
			float2 uv_texcoord;
			float3 worldPos;
			float3 worldNormal;
		};

		uniform sampler2D _Texture0;
		uniform float2 _speed;
		uniform float4 _Color1;
		uniform float4 _Color0;
		uniform float _FresnelBias;
		uniform float _FresnelScale;
		uniform float _FresnelPower;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 uv_TexCoord1 = i.uv_texcoord * float2( 1,1 );
			float2 panner2 = ( 1.0 * _Time.y * _speed + uv_TexCoord1);
			float3 ase_worldPos = i.worldPos;
			float3 ase_worldViewDir = normalize( UnityWorldSpaceViewDir( ase_worldPos ) );
			float3 ase_worldNormal = i.worldNormal;
			float fresnelNdotV16 = dot( ase_worldNormal, ase_worldViewDir );
			float fresnelNode16 = ( _FresnelBias + _FresnelScale * pow( 1.0 - fresnelNdotV16, _FresnelPower ) );
			float clampResult17 = clamp( fresnelNode16 , 0.0 , 1.0 );
			float4 lerpResult13 = lerp( _Color1 , _Color0 , clampResult17);
			o.Emission = ( tex2D( _Texture0, panner2 ) * lerpResult13 ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
0;94;1367;702;2222.68;376.7113;2.176902;True;False
Node;AmplifyShaderEditor.Vector2Node;7;-1349.014,2.206329;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;False;1,1;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;12;-955.5175,550.5815;Inherit;False;Property;_FresnelPower;FresnelPower;5;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;10;-956.5175,379.5815;Inherit;False;Property;_FresnelBias;FresnelBias;3;0;Create;True;0;0;False;0;False;0.08;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.RangedFloatNode;11;-958.5175,467.5815;Inherit;False;Property;_FresnelScale;FresnelScale;4;0;Create;True;0;0;False;0;False;1;0;0;0;0;1;FLOAT;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;1;-1154.835,35.1718;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.FresnelNode;16;-773.1232,335.8381;Inherit;True;Standard;WorldNormal;ViewDir;False;False;5;0;FLOAT3;0,0,1;False;4;FLOAT3;0,0,0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;5;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;8;-1121.662,165.4088;Inherit;False;Property;_speed; speed;2;0;Create;True;0;0;False;0;False;1,0.25;1,1;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.PannerNode;2;-917.7541,115.7426;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.ColorNode;14;-712.797,570.5943;Inherit;False;Property;_Color0;Color 0;6;0;Create;True;0;0;False;0;False;0,0.990309,1,0;0,0.990309,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ClampOpNode;17;-464.1232,403.8381;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.TexturePropertyNode;4;-989.2861,-185.3126;Inherit;True;Property;_Texture0;Texture 0;1;0;Create;True;0;0;False;0;False;2e255826ee5821d458beed8b277573e4;2e255826ee5821d458beed8b277573e4;False;white;Auto;Texture2D;-1;0;1;SAMPLER2D;0
Node;AmplifyShaderEditor.ColorNode;15;-709.797,761.5943;Inherit;False;Property;_Color1;Color 1;7;0;Create;True;0;0;False;0;False;0,0.1481996,1,0;0,0.1481996,1,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;3;-638.5996,-152.3128;Inherit;True;Property;_MainTexture;MainTexture;1;0;Create;True;0;0;False;0;False;-1;None;None;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.LerpOp;13;-273.8179,439.8514;Inherit;True;3;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;18;6.080444,149.7209;Inherit;True;2;2;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;419.1529,-39.6293;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Christal_Small_SH;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;0;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;1;0;7;0
WireConnection;16;1;10;0
WireConnection;16;2;11;0
WireConnection;16;3;12;0
WireConnection;2;0;1;0
WireConnection;2;2;8;0
WireConnection;17;0;16;0
WireConnection;3;0;4;0
WireConnection;3;1;2;0
WireConnection;13;0;15;0
WireConnection;13;1;14;0
WireConnection;13;2;17;0
WireConnection;18;0;3;0
WireConnection;18;1;13;0
WireConnection;0;2;18;0
ASEEND*/
//CHKSM=137900DAF8FDD3D334B6F55C7A79CEBFB5D05802