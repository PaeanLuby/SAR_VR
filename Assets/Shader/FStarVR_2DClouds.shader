Shader "FStarVR/2DClouds" {
	Properties {
		_CloudColor ("Color", Vector) = (1,1,1,0.5)
		_Density ("Density", Range(0, 2)) = 0.2
		_Speed ("Speed", Range(-1, 1)) = 0.1
		_ScatterMap0 ("Scatter Map 1", 2D) = "white" {}
		_ScatterMap1 ("Scatter Map 2", 2D) = "white" {}
		_DensityMap ("Density Map", 2D) = "white" {}
		_TextureMap ("Texture Map", 2D) = "white" {}
	}
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard
#pragma target 3.0

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			o.Albedo = 1;
		}
		ENDCG
	}
}