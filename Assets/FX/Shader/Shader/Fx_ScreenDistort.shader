// Made with Amplify Shader Editor
// Available at the Unity Asset Store - http://u3d.as/y3X 
Shader "Custom/Fx_Distort"
{
	Properties
	{
		_MainTex("MainTex", 2D) = "white" {}
		_DistortTex("DistortTex", 2D) = "white" {}
		_DistortAmount("DistortAmount", Range( 0 , 0.1)) = 0.04304536
		_DistortPannerXY("DistortPanner X/Y", Vector) = (0.25,0,0,0)
		_NoiseTex("NoiseTex", 2D) = "white" {}
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
		uniform float _DistortAmount;
		uniform sampler2D _DistortTex;
		uniform float2 _DistortPannerXY;
		uniform float2 _custom1;
		uniform sampler2D _NoiseTex;
		uniform sampler2D _MaskTex;

		inline half4 LightingUnlit( SurfaceOutput s, half3 lightDir, half atten )
		{
			return half4 ( 0, 0, 0, s.Alpha );
		}

		void surf( Input i , inout SurfaceOutput o )
		{
			float2 panner28 = ( 1.0 * _Time.y * float2( 0,0 ) + i.uv_texcoord);
			float2 panner19 = ( 1.0 * _Time.y * _DistortPannerXY + i.uv_texcoord);
			float4 color41 = IsGammaSpace() ? float4(0.3679245,0.3679245,0.3679245,0) : float4(0.1114872,0.1114872,0.1114872,0);
			float temp_output_33_0 = ( i.uv_tex4coord.w + _custom1.y );
			float2 panner27 = ( 1.0 * _Time.y * float2( 0.35,0 ) + i.uv_texcoord);
			float4 tex2DNode32 = tex2D( _NoiseTex, panner27 );
			float smoothstepResult36 = smoothstep( temp_output_33_0 , 1.0 , tex2DNode32.r);
			o.Emission = ( i.vertexColor * tex2D( _MainTex, ( panner28 + ( _DistortAmount * (-1.0 + (tex2D( _DistortTex, panner19 ).r - 0.0) * (1.0 - -1.0) / (1.0 - 0.0)) ) ) ) * i.vertexColor.a * color41 * smoothstepResult36 * tex2D( _MaskTex, i.uv_texcoord ).r * ( _custom1.x + i.uv_tex4coord.z ) ).rgb;
			o.Alpha = 1;
		}

		ENDCG
	}
	CustomEditor "ASEMaterialInspector"
}
/*ASEBEGIN
Version=18301
737;326;1050;1007;3395.508;2040.127;4.118774;True;False
Node;AmplifyShaderEditor.Vector2Node;17;-2758.671,1062.597;Inherit;False;Property;_DistortPannerXY;DistortPanner X/Y;4;0;Create;True;0;0;False;0;False;0.25,0;0.5,0.25;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;18;-2792.67,927.5969;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;19;-2543.671,1004.597;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SamplerNode;20;-2291.314,968.0234;Inherit;True;Property;_DistortTex;DistortTex;2;0;Create;True;0;0;False;0;False;-1;72494ae7842788a45ad8eee4549a37b9;72494ae7842788a45ad8eee4549a37b9;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;21;-2571.613,1328.422;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.TextureCoordinatesNode;22;-2101.94,495.6699;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.Vector2Node;23;-2527.657,1540.054;Inherit;False;Constant;_DistortPanner;DistortPanner;6;0;Create;True;0;0;False;0;False;0.35,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.RangedFloatNode;24;-2105.438,829.1228;Inherit;False;Property;_DistortAmount;DistortAmount;3;0;Create;True;0;0;False;0;False;0.04304536;0.1;0;0.1;0;1;FLOAT;0
Node;AmplifyShaderEditor.TFHCRemapNode;25;-2001.584,996.0817;Inherit;False;5;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;3;FLOAT;-1;False;4;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.Vector2Node;26;-2030.439,646.47;Inherit;False;Constant;_Vector0;Vector 0;2;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.TextureCoordinatesNode;31;-1620.006,1435.917;Inherit;False;0;-1;4;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT4;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.PannerNode;28;-1866.642,537.2698;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;29;-1774.636,962.1228;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.PannerNode;27;-2204.613,1392.422;Inherit;False;3;0;FLOAT2;0,0;False;2;FLOAT2;0,0;False;1;FLOAT;1;False;1;FLOAT2;0
Node;AmplifyShaderEditor.Vector2Node;30;-1584.079,1287.232;Inherit;False;Property;_custom1;custom1;7;0;Create;True;0;0;False;0;False;0,0;0,0;0;3;FLOAT2;0;FLOAT;1;FLOAT;2
Node;AmplifyShaderEditor.SamplerNode;32;-1935.752,1296.296;Inherit;True;Property;_NoiseTex;NoiseTex;5;0;Create;True;0;0;False;0;False;-1;5a8213a4d74bbd24d808a5cec974f189;3dae09c14b9534341ad2d951b2060b34;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;33;-1268.826,1393.594;Inherit;False;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleAddOpNode;34;-1594.739,746.5999;Inherit;True;2;2;0;FLOAT2;0,0;False;1;FLOAT;0;False;1;FLOAT2;0
Node;AmplifyShaderEditor.TextureCoordinatesNode;35;-1154.708,1046.013;Inherit;False;0;-1;2;3;2;SAMPLER2D;;False;0;FLOAT2;1,1;False;1;FLOAT2;0,0;False;5;FLOAT2;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SimpleAddOpNode;39;-931.0454,908.9155;Inherit;True;2;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SmoothstepOpNode;36;-1452.598,1044.408;Inherit;False;3;0;FLOAT;0;False;1;FLOAT;0;False;2;FLOAT;1;False;1;FLOAT;0
Node;AmplifyShaderEditor.SamplerNode;37;-934.8079,1128.113;Inherit;True;Property;_MaskTex;MaskTex;6;0;Create;True;0;0;False;0;False;-1;None;5a8213a4d74bbd24d808a5cec974f189;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.VertexColorNode;38;-1033.975,251.0674;Inherit;False;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.ColorNode;41;-1065.128,452.715;Inherit;False;Constant;_tint;tint;5;0;Create;True;0;0;False;0;False;0.3679245,0.3679245,0.3679245,0;0,0,0,0;True;0;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.SamplerNode;40;-1273.228,718.0903;Inherit;True;Property;_MainTex;MainTex;1;0;Create;True;0;0;False;0;False;-1;de25ca5bf0b786f4fa3e92b17f9b067b;99855230e2f31de4f9a1e14f98c7298c;True;0;False;white;Auto;False;Object;-1;Auto;Texture2D;6;0;SAMPLER2D;;False;1;FLOAT2;0,0;False;2;FLOAT;0;False;3;FLOAT2;0,0;False;4;FLOAT2;0,0;False;5;FLOAT;1;False;5;COLOR;0;FLOAT;1;FLOAT;2;FLOAT;3;FLOAT;4
Node;AmplifyShaderEditor.StepOpNode;43;-1246.767,1034.462;Inherit;False;2;0;FLOAT;0;False;1;FLOAT;0;False;1;FLOAT;0
Node;AmplifyShaderEditor.SimpleMultiplyOpNode;42;-610.2284,533.7903;Inherit;True;7;7;0;COLOR;0,0,0,0;False;1;COLOR;0,0,0,0;False;2;FLOAT;0;False;3;COLOR;0,0,0,0;False;4;FLOAT;0;False;5;FLOAT;0;False;6;FLOAT;0;False;1;COLOR;0
Node;AmplifyShaderEditor.StandardSurfaceOutputNode;0;-2,-210;Float;False;True;-1;2;ASEMaterialInspector;0;0;Unlit;Custom/Fx_Distort;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;False;Off;2;False;-1;3;False;-1;False;0;False;-1;0;False;-1;False;0;Custom;0.5;True;False;0;True;Overlay;;Transparent;All;14;all;True;True;True;True;0;False;-1;False;0;False;-1;255;False;-1;255;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;0;False;-1;False;2;15;10;25;False;0.5;False;4;1;False;-1;1;False;-1;4;1;False;-1;1;False;-1;0;False;-1;0;False;-1;0;False;0;0,0,0,0;VertexOffset;True;False;Cylindrical;False;Relative;0;;0;-1;-1;-1;0;False;0;0;False;-1;-1;0;False;-1;0;0;0;False;0.1;False;-1;0;False;-1;15;0;FLOAT3;0,0,0;False;1;FLOAT3;0,0,0;False;2;FLOAT3;0,0,0;False;3;FLOAT;0;False;4;FLOAT;0;False;6;FLOAT3;0,0,0;False;7;FLOAT3;0,0,0;False;8;FLOAT;0;False;9;FLOAT;0;False;10;FLOAT;0;False;13;FLOAT3;0,0,0;False;11;FLOAT3;0,0,0;False;12;FLOAT3;0,0,0;False;14;FLOAT4;0,0,0,0;False;15;FLOAT3;0,0,0;False;0
WireConnection;19;0;18;0
WireConnection;19;2;17;0
WireConnection;20;1;19;0
WireConnection;25;0;20;1
WireConnection;28;0;22;0
WireConnection;28;2;26;0
WireConnection;29;0;24;0
WireConnection;29;1;25;0
WireConnection;27;0;21;0
WireConnection;27;2;23;0
WireConnection;32;1;27;0
WireConnection;33;0;31;4
WireConnection;33;1;30;2
WireConnection;34;0;28;0
WireConnection;34;1;29;0
WireConnection;39;0;30;1
WireConnection;39;1;31;3
WireConnection;36;0;32;1
WireConnection;36;1;33;0
WireConnection;37;1;35;0
WireConnection;40;1;34;0
WireConnection;43;0;32;1
WireConnection;43;1;33;0
WireConnection;42;0;38;0
WireConnection;42;1;40;0
WireConnection;42;2;38;4
WireConnection;42;3;41;0
WireConnection;42;4;36;0
WireConnection;42;5;37;1
WireConnection;42;6;39;0
WireConnection;0;2;42;0
ASEEND*/
//CHKSM=C708057CD4BACBAEA48E925738C6EA3A22A68525