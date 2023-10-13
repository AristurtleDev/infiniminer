#if OPENGL
	#define SV_POSITION POSITION
	#define VS_SHADERMODEL vs_3_0
	#define PS_SHADERMODEL ps_3_0
#else
	#define VS_SHADERMODEL vs_4_0_level_9_1
	#define PS_SHADERMODEL ps_4_0_level_9_1
#endif

struct VertexToPixel
{
    float4 Position : POSITION;
    float4 Color : COLOR0;   
    float LightingFactor: TEXCOORD0; 
};

struct PixelToFrame
{
    float4 Color : COLOR0;
};

//------- XNA-to-HLSL variables --------
float4x4 xView;
float4x4 xProjection;
float4x4 xWorld;
float4	 xColor;

//------- Technique: Block --------

VertexToPixel BlockVS( float4 inPos : POSITION, float2 inTexCoords: TEXCOORD0, float inShade: TEXCOORD1)
{	
	VertexToPixel Output = (VertexToPixel)0;
	float4x4 preViewProjection = mul (xView, xProjection);
	float4x4 preWorldViewProjection = mul (xWorld, preViewProjection);
    
	Output.Position = mul(inPos, preWorldViewProjection);	
	Output.Color = xColor;
	Output.LightingFactor = inShade;
	
	return Output;    
}

PixelToFrame BlockPS(VertexToPixel PSIn) 
{
	PixelToFrame Output = (PixelToFrame)0;		
	
	Output.Color = PSIn.Color;
	Output.Color.rgb *= saturate(PSIn.LightingFactor);

	return Output;
}

technique Projectile
{
	pass Pass0
	{   
		VertexShader = compile VS_SHADERMODEL BlockVS();
		PixelShader  = compile PS_SHADERMODEL BlockPS();
	}
}
